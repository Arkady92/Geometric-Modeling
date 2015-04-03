using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Mathematics;

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
        public BezierCurve(IEnumerable<Point> points, Vector4 position)
            : base(ModelType.BezierCurve, position, true)
        {
            var enumerable = points as Point[] ?? points.ToArray();
            for (int i = 0; i < enumerable.Count(); i++)
            {
                var vector = enumerable[i].GetCurrentPosition();
                Vertices.Add(vector);
                enumerable[i].AddParent(this, i);
                Children.Add(enumerable[i]);
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
            Vertices.Add(new Vector4(-0.25, -0.25, -0.25) + SpacePosition);
            Vertices.Add(new Vector4(0.15, -0.15, 0.15) + SpacePosition);
            Vertices.Add(new Vector4(-0.15, 0.15, -0.15) + SpacePosition);
            Vertices.Add(new Vector4(0.25, 0.25, 0.25) + SpacePosition);
            for (int i = 0; i < Vertices.Count(); i++)
            {
                Children.Add(new Point(Vertices[i], this, i));             
            }
        }

        public override string ToString()
        {
            if (CustomName != null)
                return "Bezier Curve <" + CustomName + ">";
            return "Bezier Curve <" + _increment++ + ">";
        }

        protected override void RecreateStructure()
        {
            Vertices.Clear();
            Edges.Clear();
            for (int i = 0; i < Children.Count(); i++)
            {
                var vector = Children[i].GetCurrentPosition();
                Vertices.Add(vector);
                Children[i].SetParentIndex(this, i);
            }
            CreateEdges();
        }

        protected override void DrawModel(Graphics graphics, Matrix currentProjectionMatrix, Color color)
        {
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.CurrentOperationMatrix);
            currentMatrix = currentProjectionMatrix * currentMatrix * CurrentOperationMatrix;

            var pen = new Pen(color);
            var vertices = Vertices.Select(vertex => currentMatrix * vertex).ToList();
            if (Parameters.PolygonalChainEnabled)
            {
                foreach (var edge in Edges)
                {
                    graphics.DrawLine(pen, (float) vertices[edge.StartVertex].X*Parameters.WorldPanelSizeFactor,
                        (float) vertices[edge.StartVertex].Y*Parameters.WorldPanelSizeFactor,
                        (float) vertices[edge.EndVertex].X*Parameters.WorldPanelSizeFactor,
                        (float) vertices[edge.EndVertex].Y*Parameters.WorldPanelSizeFactor);
                }
            }
            var points = new System.Drawing.Point[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                points[i] = new System.Drawing.Point((int)(vertices[i].X * Parameters.WorldPanelSizeFactor),
                    (int)(vertices[i].Y * Parameters.WorldPanelSizeFactor));
            }
            graphics.DrawBeziers(pen, points);
        }
    }
}
