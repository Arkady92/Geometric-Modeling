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

        public BezierCurve()
            : base(ModelType.BezierCurve)
        {
            CreateVertices();
            CreateEdges();
        }
        public BezierCurve(IEnumerable<Point> points)
            : base(ModelType.BezierCurve)
        {
            foreach (var point in points)
            {
                var vector = point.ToVector4();
                vector = point.CurrentOperationMatrix * vector;
                Vertices.Add(vector);
                Children.Add(new Point(vector, this));
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
            Vertices.Add(new Vector4(-1, -1, -1));
            Vertices.Add(new Vector4(0.5, -0.5, 0.5));
            Vertices.Add(new Vector4(-0.5, 0.5, -0.5));
            Vertices.Add(new Vector4(1, 1, 1));
            foreach (var vertex in Vertices)
            {
                Children.Add(new Point(vertex, this));             
            }
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
            return "Bezier Curve <" + _increment++ + ">";
        }
    }
}
