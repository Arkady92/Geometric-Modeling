using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Mathematics;

namespace Models
{
    public class BezierCurveC2 : BezierCurve
    {
        private readonly List<Point> _bezierPoints;
        private BezierCurve _bezierCurve;
        private readonly List<GeometricModel> _hiddenModels;

        private static int _increment = 1;

        public BezierCurveC2(Vector4 position, List<GeometricModel> hiddenModels = null)
            : base(position, ModelType.BezierCurveC2)
        {
            _hiddenModels = hiddenModels;
            _bezierPoints = new List<Point>();
            CalculateBezierPoints();
        }

        public BezierCurveC2(IEnumerable<Point> points, Vector4 position, List<GeometricModel> hiddenModels = null)
            : base(points, position, ModelType.BezierCurveC2)
        {
            _hiddenModels = hiddenModels;
            _bezierPoints = new List<Point>();
            CalculateBezierPoints();
        }

        protected override void CreateVertices()
        {
            Vertices.Add(new Vector4(-1, 0.5, 0) + SpacePosition);
            Vertices.Add(new Vector4(-0.75, -0.5, 0) + SpacePosition);
            Vertices.Add(new Vector4(-0.5, 0.5, 0) + SpacePosition);
            Vertices.Add(new Vector4(-0.25, -0.5, 0) + SpacePosition);
            Vertices.Add(new Vector4(0, 0.5, 0) + SpacePosition);
            Vertices.Add(new Vector4(0.25, -0.5, 0) + SpacePosition);
            Vertices.Add(new Vector4(0.5, 0.5, 0) + SpacePosition);
            Vertices.Add(new Vector4(0.75, -0.5, 0) + SpacePosition);
            Vertices.Add(new Vector4(1, 0.5, 0) + SpacePosition);
            for (int i = 0; i < Vertices.Count(); i++)
            {
                Children.Add(new Point(new Vector4(Vertices[i].X, Vertices[i].Y, Vertices[i].Z), this, i));
            }
        }

        public override string ToString()
        {
            if (CustomName != null)
                return "Bezier Curve C2 <" + CustomName + ">";
            return "Bezier Curve C2 <" + _increment++ + ">";
        }

        public override void Draw(Graphics graphics, Matrix currentProjectionMatrix)
        {
            base.Draw(graphics, currentProjectionMatrix);
            _bezierCurve.Draw(graphics, currentProjectionMatrix);
        }

        public override void DrawStereoscopy(Graphics graphics, Matrix leftMatrix, Matrix rightMatrix, bool additiveColorBlending = false)
        {
            base.DrawStereoscopy(graphics, leftMatrix, rightMatrix, additiveColorBlending);
            _bezierCurve.DrawStereoscopy(graphics, leftMatrix, rightMatrix, additiveColorBlending);
        }

        protected override void DrawModel(Graphics graphics, Matrix currentProjectionMatrix, Color color)
        {
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.CurrentOperationMatrix);
            currentMatrix = currentProjectionMatrix * currentMatrix * CurrentOperationMatrix;

            _bezierCurve.Draw(graphics, currentProjectionMatrix);

            var vertices = Vertices.Select(vertex => currentMatrix * vertex).ToList();
            if (Parameters.PolygonalChainEnabled && !Parameters.ControlPointsEnabled)
            {
                foreach (var edge in Edges)
                {
                    graphics.DrawLine(new Pen(color), (float)vertices[edge.StartVertex].X * Parameters.WorldPanelSizeFactor,
                        (float)vertices[edge.StartVertex].Y * Parameters.WorldPanelSizeFactor,
                        (float)vertices[edge.EndVertex].X * Parameters.WorldPanelSizeFactor,
                        (float)vertices[edge.EndVertex].Y * Parameters.WorldPanelSizeFactor);
                }
            }
        }

        protected override void DrawWithAdditiveBlending(Bitmap bitmap, Graphics graphics, Matrix currentProjectionMatrix, Color color)
        {
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.CurrentOperationMatrix);
            currentMatrix = currentProjectionMatrix * currentMatrix * CurrentOperationMatrix;

            var vertices = Vertices.Select(vertex => currentMatrix * vertex).ToList();
            if (Parameters.PolygonalChainEnabled && !Parameters.ControlPointsEnabled)
            {
                foreach (var edge in Edges)
                {
                    var customLine = new CustomLine(
                        new System.Drawing.Point(
                            (int)(vertices[edge.StartVertex].X * Parameters.WorldPanelSizeFactor),
                            (int)(vertices[edge.StartVertex].Y * Parameters.WorldPanelSizeFactor)),
                        new System.Drawing.Point(
                            (int)(vertices[edge.EndVertex].X * Parameters.WorldPanelSizeFactor),
                            (int)(vertices[edge.EndVertex].Y * Parameters.WorldPanelSizeFactor)));
                    customLine.Draw(bitmap, graphics, color, 1);
                }
            }
        }

        public void CalculateBezierPoints()
        {
            var points = Children.Cast<Point>().ToList();
            foreach (var bezierPoint in _bezierPoints)
            {
                _hiddenModels.Remove(bezierPoint);
            }
            _bezierPoints.Clear();
            if (points.Count == 0) return;
            if (points.Count < 5)
                _bezierPoints.AddRange(points);
            else
            {
                _bezierPoints.Add(new Point(points[0].GetCurrentPosition()));
                _bezierPoints.Add(new Point(points[1].GetCurrentPosition()));
                for (int i = 2; i < Children.Count - 2; i++)
                {
                    var lKnot = points[i - 1];
                    var mKnot = points[i];
                    var rKnot = points[i + 1];
                    var lPoint = i == 2 ? Vector4.Center(lKnot.GetCurrentPosition(), mKnot.GetCurrentPosition()) :
                        Vector4.OneThird(mKnot.GetCurrentPosition(), lKnot.GetCurrentPosition());
                    var rPoint = i == Children.Count - 3 ? Vector4.Center(mKnot.GetCurrentPosition(), rKnot.GetCurrentPosition()) :
                        Vector4.OneThird(mKnot.GetCurrentPosition(), rKnot.GetCurrentPosition());
                    _bezierPoints.Add(new Point(lPoint));
                    _bezierPoints.Add(new Point(Vector4.Center(lPoint, rPoint)));
                    _bezierPoints.Add(new Point(rPoint));
                }
                _bezierPoints.Add(new Point(points[points.Count - 2].GetCurrentPosition()));
                _bezierPoints.Add(new Point(points[points.Count - 1].GetCurrentPosition()));
            }
            _hiddenModels.AddRange(_bezierPoints);
            _bezierCurve = new BezierCurve(_bezierPoints, SpacePosition);
        }

        public override void UpdateModel()
        {
            base.UpdateModel();
            CalculateBezierPoints();
        }

        protected override void RecreateStructure(int number = 0)
        {
            base.RecreateStructure(number);
            CalculateBezierPoints();
        }

        public override void UpdateVertex(int number)
        {
            base.UpdateVertex(number);
            CalculateBezierPoints();
        }
    }

}
