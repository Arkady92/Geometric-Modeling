using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Mathematics;
using Matrix = Mathematics.Matrix;

namespace Models
{
    public class BezierCurve : ParametricGeometricModel
    {
        private static int _increment = 1;

        public BezierCurve(Vector4 position)
            : base(ModelType.BezierCurve, position)
        {
            CreateVertices();
            CreateEdges();
        }
        public BezierCurve(List<Point> points, Vector4 position)
            : base(ModelType.BezierCurve, position)
        {
            for (int i = 0; i < points.Count(); i++)
            {
                var vector = points[i].GetCurrentPosition();
                Vertices.Add(vector);
                Children.Add(new Point(vector, this, i));
            }
            CreateEdges();
        }

        protected override void CreateEdges()
        {
            for (int i = 0; i < Vertices.Count - 1; i++)
                Edges.Add(new Edge(i, i + 1));
        }

        protected override void CreateVertices()
        {
            Vertices.Add(new Vector4(-1, -1, -1) + SpacePosition);
            Vertices.Add(new Vector4(0.5, -0.5, 0.5) + SpacePosition);
            Vertices.Add(new Vector4(-0.5, 0.5, -0.5) + SpacePosition);
            Vertices.Add(new Vector4(1, 1, 1) + SpacePosition);
            for (int i = 0; i < Vertices.Count(); i++)
            {
                Children.Add(new Point(Vertices[i], this, i));             
            }
        }

        public override string ToString()
        {
            if (CustomName != null)
                return CustomName;
            return "Bezier Curve <" + _increment++ + ">";
        }
    }
}
