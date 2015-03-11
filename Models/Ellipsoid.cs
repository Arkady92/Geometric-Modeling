using System;
using System.Drawing;
using Mathematics;
using Matrix = Mathematics.Matrix;

namespace Models
{
    public class Ellipsoid : ImplicitGeometricModel
    {
        private Matrix _diagonalMatrix;

        public Ellipsoid()
            : base(ModelType.Ellipsoid)
        {
            UpdateModel();
        }
        public override void Draw(Graphics graphics, /*Matrix currentOperationsMatrix,*/ Matrix currentProjectionMatrix)
        {
            var inverseMatrix = CurrentOperationsMatrix.Invert();
            var resultMatrix = Matrix.Transpose(inverseMatrix) * _diagonalMatrix * inverseMatrix;
            DrawFrame(graphics, resultMatrix, currentProjectionMatrix, 1);
        }

        public override void Draw(Graphics graphics, Matrix currentProjectionMatrix, int pixelSize)
        {
            var inverseMatrix = CurrentOperationsMatrix.Invert();
            var resultMatrix = Matrix.Transpose(inverseMatrix) * _diagonalMatrix * inverseMatrix;
            DrawFrame(graphics, resultMatrix, currentProjectionMatrix, pixelSize);
        }

        private void DrawFrame(Graphics graphics, Matrix resultMatrix, Matrix currentProjectionMatrix, int pixelSize)
        {
            var shiftW = (Parameters.WorldPanelWidth / 2 / pixelSize) * pixelSize;
            var shiftH = (Parameters.WorldPanelHeight / 2 / pixelSize) * pixelSize;
            for (int x = pixelSize / 2; x < shiftW; x += pixelSize)
            {
                for (int y = pixelSize / 2; y < shiftH; y += pixelSize)
                {
                    CastRay(x, y, graphics, resultMatrix, currentProjectionMatrix, pixelSize);
                    CastRay(-x, y, graphics, resultMatrix, currentProjectionMatrix, pixelSize);
                    CastRay(x, -y, graphics, resultMatrix, currentProjectionMatrix, pixelSize);
                    CastRay(-x, -y, graphics, resultMatrix, currentProjectionMatrix, pixelSize);
                }
            }
        }

        void CastRay(int x, int y, Graphics graphics, Matrix resultMatrix, Matrix currentProjectionMatrix, int pixelSize)
        {
            double z;
            if (!CalculateZValue(resultMatrix, x, y, out z)) return;
            var vertex = new Vector4(x, y, z);
            var normal = (vertex * resultMatrix) * 2;
            normal.Normalize();
            normal.W = 0;
            var view = new Vector4(-x, -y, 1 / currentProjectionMatrix[3,2], 0);
            view.Normalize();
            var factor = normal * view;
            if (factor < 0) factor = 0;
            factor = Math.Pow(factor, Parameters.Illuminance);
            var r = (int)(factor * 255 + Parameters.DefaultModelColor.R);
            var g = (int)(factor * 255 + Parameters.DefaultModelColor.G);
            var b = (int)(factor * 255 + Parameters.DefaultModelColor.B);
            if (r < 0) r = 0;
            if (r > 255) r = 255;
            if (g < 0) g = 0;
            if (g > 255) g = 255;
            if (b < 0) b = 0;
            if (b > 255) b = 255;
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(Parameters.DefaultModelColor.A, r, g, b)),
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
            _diagonalMatrix = OperationsMatrices.Diagonal(Parameters.XAxisFactor, Parameters.YAxisFactor, Parameters.ZAxisFactor, -1);
        }
    }
}
