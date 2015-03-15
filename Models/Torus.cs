using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Mathematics;
using Matrix = Mathematics.Matrix;

namespace Models
{
    public class Torus : ParametricGeometricModel
    {
        public double BigRadius { get; private set; }
        public double SmallRadius { get; private set; }

        public Torus(double bigRadius = 1, double smallRadius = 0.5)
            : base(ModelType.Torus)
        {
            BigRadius = bigRadius;
            SmallRadius = smallRadius;
            CreateVertices();
            CreateEdges();
        }

        protected override void CreateEdges()
        {
            var verticesCount = Parameters.GridResolutionX * Parameters.GridResolutionY;
            if (verticesCount > Parameters.GridResolutionX)
            {
                Edges.Add(new Edge(0, Parameters.GridResolutionX));
                for (int i = 1; i < Parameters.GridResolutionY; i++)
                {
                    var shift = i * Parameters.GridResolutionX;
                    for (int j = 0; j < Parameters.GridResolutionX; j++)
                    {
                        Edges.Add(new Edge(shift + j, shift + (j + 1) % Parameters.GridResolutionX));
                    }
                    Edges.Add(new Edge(shift, (shift + Parameters.GridResolutionX) % verticesCount));
                }
            }
            if (verticesCount > 1)
            {
                Edges.Add(new Edge(0, 1));
                for (int i = 1; i < Parameters.GridResolutionX; i++)
                {
                    for (int j = 0; j < Parameters.GridResolutionY; j++)
                    {
                        var shift = j * Parameters.GridResolutionX;
                        Edges.Add(new Edge(shift + i, (shift + Parameters.GridResolutionX + i) % verticesCount));
                    }
                    Edges.Add(new Edge(i, (i + 1) % Parameters.GridResolutionX));
                }
            }
        }

        protected override void CreateVertices()
        {
            var alphaStep = 2 * Math.PI / Parameters.GridResolutionY;
            var betaStep = 2 * Math.PI / Parameters.GridResolutionX;

            for (double alpha = alphaStep; alpha < 2 * Math.PI + alphaStep / 2; alpha += alphaStep)
            {
                for (double beta = betaStep; beta < 2 * Math.PI + betaStep / 2; beta += betaStep)
                {
                    Vertices.Add(ParametricEquation(alpha, beta));
                }
            }
        }

        private Vector4 ParametricEquation(double alpha, double beta)
        {
            return new Vector4((BigRadius + SmallRadius * Math.Cos(alpha)) * Math.Cos(beta),
                (BigRadius + SmallRadius * Math.Cos(alpha)) * Math.Sin(beta), SmallRadius * Math.Sin(alpha));
        }

        private void DrawTorus(Graphics graphics, Matrix matrix, Pen pen)
        {
            var currentMatrix = matrix * CurrentOperationsMatrix;
            var vertices = Vertices.Select(vertex => currentMatrix * vertex).ToList();
            float factor = (Parameters.WorldPanelWidth < Parameters.WorldPanelHeight) ?
                Parameters.WorldPanelWidth * 0.25f : Parameters.WorldPanelHeight * 0.25f;
            var graphicsPath = new GraphicsPath();
            foreach (var edge in Edges)
            {
                graphicsPath.AddLine(
                    (float)vertices[edge.StartVertex].X * factor,
                    (float)vertices[edge.StartVertex].Y * factor,
                    (float)vertices[edge.EndVertex].X * factor,
                    (float)vertices[edge.EndVertex].Y * factor);
            }
            graphics.DrawPath(pen, graphicsPath);
        }

        public override void Draw(Graphics graphics, Matrix currentProjectionMatrix = null)
        {
            DrawTorus(graphics, currentProjectionMatrix, Pens.Black);
        }

        public override void DrawStereoscopy(Graphics graphics, Matrix leftMatrix, Matrix rightMatrix,
            bool intersectionsDetection = false)
        {
            if (!intersectionsDetection)
            {
                DrawTorus(graphics, leftMatrix * CurrentOperationsMatrix, Pens.Red);
                DrawTorus(graphics, rightMatrix * CurrentOperationsMatrix, Pens.Blue);
            }
            else
            {
                var bitmapLeft = new Bitmap(Parameters.WorldPanelWidth, Parameters.WorldPanelHeight);
                var bitmapRight = new Bitmap(Parameters.WorldPanelWidth, Parameters.WorldPanelHeight);
                var bitmapResult = new Bitmap(Parameters.WorldPanelWidth, Parameters.WorldPanelHeight);
                var graphicsLeft = Graphics.FromImage(bitmapLeft);
                graphicsLeft.TranslateTransform(Parameters.WorldPanelWidth * 0.5f, Parameters.WorldPanelHeight * 0.5f);
                var graphicsRight = Graphics.FromImage(bitmapRight);
                graphicsRight.TranslateTransform(Parameters.WorldPanelWidth * 0.5f, Parameters.WorldPanelHeight * 0.5f);
                DrawTorus(graphicsLeft, leftMatrix * CurrentOperationsMatrix, Pens.Red);
                DrawTorus(graphicsRight, rightMatrix * CurrentOperationsMatrix, Pens.Blue);

                for (int y = 0; y < Parameters.WorldPanelHeight; y++)
                {
                    for (int x = 0; x < Parameters.WorldPanelWidth; x++)
                    {
                        var cA = bitmapLeft.GetPixel(x, y);
                        var cB = bitmapRight.GetPixel(x, y);
                        var A = cA.A + cB.A;
                        if (A > 255) A = 255;
                        var cC = Color.FromArgb(A, cA.R + cB.R, cA.G + cB.G, cA.B + cB.B);
                        bitmapResult.SetPixel(x, y, cC);
                    }
                }
                graphics.DrawImage(bitmapResult, new Rectangle(-Parameters.WorldPanelWidth / 2, -Parameters.WorldPanelHeight / 2,
                    Parameters.WorldPanelWidth, Parameters.WorldPanelHeight));
            }
        }
    }
}
