using System;
using System.Drawing;
using System.Linq;
using Mathematics;

namespace Models
{
    public class Torus : GeometricModel
    {
        public double BigRadius;
        public double SmallRadius;

        public Torus(double bigRadius = 1, double smallRadius = 0.5)
            : base(ModelType.Torus)
        {
            BigRadius = bigRadius;
            SmallRadius = smallRadius;
            CreatePoints();
            CreateEdges();
        }

        private void CreateEdges()
        {
            var verticesCount = Parameters.GridResolutionX * Parameters.GridResolutionY;
            for (int i = 0; i < Parameters.GridResolutionY; i++)
            {
                var shift = i * Parameters.GridResolutionX;
                for (int j = 0; j < Parameters.GridResolutionX; j++)
                {
                    Edges.Add(new Edge(shift + j, shift + (j + 1) % Parameters.GridResolutionX));
                    Edges.Add(new Edge(shift + j, (shift + Parameters.GridResolutionX + j) % verticesCount));
                }
            }
        }

        private void CreatePoints()
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

        public override void Draw(Graphics graphics, Matrix currentMatrix)
        {
            var vertices = Vertices.Select(vertex => currentMatrix * vertex).ToList();
            float factor = (Parameters.WorldPanelWidth < Parameters.WorldPanelHeight) ?
                Parameters.WorldPanelWidth * 0.25f : Parameters.WorldPanelHeight * 0.25f;
            foreach (var edge in Edges)
            {
                graphics.DrawLine(Pens.Black,
                    (float)vertices[edge.StartVertex].X * factor,
                    (float)vertices[edge.StartVertex].Y * factor,
                    (float)vertices[edge.EndVertex].X * factor,
                    (float)vertices[edge.EndVertex].Y * factor);
            }
        }

        public override void UpdateMesh()
        {
            Vertices.Clear();
            Edges.Clear();
            CreatePoints();
            CreateEdges();
        }
    }
}
