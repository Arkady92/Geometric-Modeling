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
            var halfSize = pixelSize / 2;
            for (int x = 0; x < shiftW; x += pixelSize)
            {
                for (int y = 0; y < shiftH; y += pixelSize)
                {
                    double z;
                    if (!CalculateZValue(resultMatrix, x, y, out z)) continue;
                    var vertex = new Vector4(x, y, z);
                    vertex = currentProjectionMatrix * vertex;
                    graphics.FillRectangle(new SolidBrush(Parameters.DefaultModelColor),
                        (int)vertex.X - halfSize,
                        (int)vertex.Y - halfSize,
                        pixelSize,
                        pixelSize);
                    if (!CalculateZValue(resultMatrix, -x, y, out z)) continue;
                    vertex = new Vector4(-x, y, z);
                    vertex = currentProjectionMatrix * vertex;
                    graphics.FillRectangle(new SolidBrush(Parameters.DefaultModelColor),
                        (int)vertex.X - halfSize,
                        (int)vertex.Y - halfSize,
                        pixelSize,
                        pixelSize);
                    if (!CalculateZValue(resultMatrix, x, -y, out z)) continue;
                    vertex = new Vector4(x, -y, z);
                    vertex = currentProjectionMatrix * vertex;
                    graphics.FillRectangle(new SolidBrush(Parameters.DefaultModelColor),
                        (int)vertex.X - halfSize,
                        (int)vertex.Y - halfSize,
                        pixelSize,
                        pixelSize);
                    if (!CalculateZValue(resultMatrix, -x, -y, out z)) continue;
                    vertex = new Vector4(-x, -y, z);
                    vertex = currentProjectionMatrix * vertex;
                    graphics.FillRectangle(new SolidBrush(Parameters.DefaultModelColor),
                        (int)vertex.X - halfSize,
                        (int)vertex.Y - halfSize,
                        pixelSize,
                        pixelSize);
                }
            }
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
