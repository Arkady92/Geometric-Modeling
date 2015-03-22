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

        private static int _increment = 1;

        public Torus(Vector4 position, double bigRadius = 1, double smallRadius = 0.5)
            : base(ModelType.Torus, position)
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
                    var result = ParametricEquation(alpha, beta) + SpacePosition;
                    Vertices.Add(result);
                    Children.Add(new Point(result, this, Vertices.Count-1));
                }
            }
        }

        private Vector4 ParametricEquation(double alpha, double beta)
        {
            return new Vector4((BigRadius + SmallRadius * Math.Cos(alpha)) * Math.Cos(beta),
                (BigRadius + SmallRadius * Math.Cos(alpha)) * Math.Sin(beta), SmallRadius * Math.Sin(alpha));
        }

        protected override void DrawModel(Graphics graphics, Matrix currentProjectionMatrix, Color color)
        {
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.CurrentOperationMatrix);
            currentMatrix = currentProjectionMatrix * CurrentOperationMatrix * currentMatrix;

            var pen = new Pen(color);
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
        public override string ToString()
        {
            if (CustomName != null)
                return CustomName;
            return "Torus <" + _increment++ + ">";
        }
    }
}
