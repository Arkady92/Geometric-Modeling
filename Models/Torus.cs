using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Mathematics;

namespace Models
{
    public class Torus : GeometricModel
    {
        public double BigRadius;
        public double SmallRadius;

        public Torus(double bigRadius = 50, double smallRadius = 30)
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

            for (double alpha = alphaStep; alpha < 2 * Math.PI + alphaStep/2 ; alpha += alphaStep)
            {
                for (double beta = betaStep; beta < 2 * Math.PI + betaStep/2; beta += betaStep)
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
            foreach (var edge in Edges)
            {
                graphics.DrawLine(Pens.Black, (float)vertices[edge.StartVertex].X, (float)vertices[edge.StartVertex].Y,
                    (float)vertices[edge.EndVertex].X, (float)vertices[edge.EndVertex].Y);
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
