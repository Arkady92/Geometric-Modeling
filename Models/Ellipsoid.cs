﻿using System;
using System.Drawing;
using System.Globalization;
using Mathematics;
using Matrix = Mathematics.Matrix;

namespace Models
{
    public class Ellipsoid : ImplicitGeometricModel
    {
        private readonly Vector4 _lightPosition;

        private static int _increment = 1;

        public Ellipsoid(Vector4 position)
            : base(ModelType.Ellipsoid, position)
        {
            UpdateModel();
            _lightPosition = new Vector4(500, 500, 500, 0);
            CustomName = (_increment++).ToString(CultureInfo.InvariantCulture);
        }
        public override void Draw(Graphics graphics, Matrix currentProjectionMatrix)
        {
            var inverseMatrix = OperationMatrix.Invert();
            var resultMatrix = Matrix.Transpose(inverseMatrix) * ModelMatrix * inverseMatrix;
            DrawFrame(graphics, resultMatrix, 1);
        }

        public override void Draw(Graphics graphics, int pixelSize, Matrix currentProjectionMatrix = null)
        {
            var inverseMatrix = OperationMatrix.Invert();
            var resultMatrix = Matrix.Transpose(inverseMatrix) * ModelMatrix * inverseMatrix;
            DrawFrame(graphics, resultMatrix, pixelSize);
        }

        private void DrawFrame(Graphics graphics, Matrix resultMatrix, int pixelSize)
        {
            var shiftW = (Parameters.WorldPanelWidth / 2 / pixelSize) * pixelSize;
            var shiftH = (Parameters.WorldPanelHeight / 2 / pixelSize) * pixelSize;
            for (int x = pixelSize / 2; x < shiftW; x += pixelSize)
            {
                for (int y = pixelSize / 2; y < shiftH; y += pixelSize)
                {
                    CastRay(x, y, graphics, resultMatrix, pixelSize);
                    CastRay(-x, y, graphics, resultMatrix, pixelSize);
                    CastRay(x, -y, graphics, resultMatrix, pixelSize);
                    CastRay(-x, -y, graphics, resultMatrix, pixelSize);
                }
            }
        }

        void CastRay(int x, int y, Graphics graphics, Matrix resultMatrix, int pixelSize)
        {
            var xMove = (int)(SpacePosition.X * Parameters.XAxisFactor);
            var yMove = (int)(SpacePosition.Y * Parameters.YAxisFactor);
            double z;
            if (!CalculateZValue(resultMatrix, x - xMove, y - yMove, out z) || z < 0) return;
            var vertex = new Vector4(x, y, z);
            var normal = (vertex * resultMatrix) * 2;
            normal.NormalizeSecond();
            normal.W = 0;
            var light = new Vector4(-x + _lightPosition.X, -y + _lightPosition.Y, _lightPosition.Z, 0);
            //light = light * resultMatrix;
            light.NormalizeSecond();
            var factor = normal * light;
            if (factor < 0) factor = 0;
            factor = Math.Pow(factor, Parameters.Illuminance);
            var r = (int)(factor * Color.R);
            var g = (int)(factor * Color.G);
            var b = (int)(factor * Color.B);
            if (r < 0) r = 0;
            if (r > 255) r = 255;
            if (g < 0) g = 0;
            if (g > 255) g = 255;
            if (b < 0) b = 0;
            if (b > 255) b = 255;
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(Color.A, r, g, b)),
                x - pixelSize / 2,
                y - pixelSize / 2,
                pixelSize,
                pixelSize);
        }

        private bool CalculateZValue(Matrix matrix, int x, int y, out double z)
        {
            z = 0;
            var a = matrix[2, 2];
            var b = x * (matrix[2, 0] + matrix[0, 2]) + y * (matrix[2, 1] + matrix[1, 2]) + matrix[3, 2] + matrix[2, 3];
            var c = x * (x * matrix[0, 0] + y * matrix[1, 0] + matrix[3, 0] + matrix[0, 3])
                       + y * (x * matrix[0, 1] + y * matrix[1, 1] + matrix[3, 1] + matrix[1, 3])
                       + matrix[3, 3];
            var delta = b * b - 4 * a * c;
            if (delta < 0) return false;
            delta = Math.Sqrt(delta);
            z = (-b + delta) * 0.5 / a;
            return true;
        }

        public override void UpdateModel()
        {
            ModelMatrix = OperationsMatrices.Diagonal(Parameters.XAxisFactor, Parameters.YAxisFactor, Parameters.ZAxisFactor, -1);
        }
    }
}
