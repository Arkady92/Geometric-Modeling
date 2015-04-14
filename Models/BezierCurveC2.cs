using System;
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
            ControlPointsEnabled = false;
            _hiddenModels = hiddenModels;
            _bezierPoints = new List<Point>();
            CalculateBezierPoints();
        }

        public BezierCurveC2(IEnumerable<Point> points, Vector4 position, List<GeometricModel> hiddenModels = null)
            : base(points, position, ModelType.BezierCurveC2)
        {
            ControlPointsEnabled = false;
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
            if (ControlPointsEnabled)
                _bezierCurve.Draw(graphics, currentProjectionMatrix);
        }

        public override void DrawStereoscopy(Graphics graphics, Matrix leftMatrix, Matrix rightMatrix, bool additiveColorBlending = false)
        {
            base.DrawStereoscopy(graphics, leftMatrix, rightMatrix, additiveColorBlending);
            if (ControlPointsEnabled)
                _bezierCurve.DrawStereoscopy(graphics, leftMatrix, rightMatrix, additiveColorBlending);
        }

        protected override void DrawModel(Graphics graphics, Matrix currentProjectionMatrix, Color color)
        {
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.CurrentOperationMatrix);
            currentMatrix = currentMatrix * CurrentOperationMatrix;
            var vertices = Vertices.Select(vertex => currentMatrix * vertex).ToList();
            var finalVertices = vertices.Select(vertex => currentProjectionMatrix * vertex).ToList();

            if (!ControlPointsEnabled && vertices.Count > 1)
            {
                var brush = new SolidBrush(color);
                var points = new System.Drawing.Point[vertices.Count];
                int division = 0;
                for (int i = 0; i < vertices.Count; i++)
                {
                    points[i] = new System.Drawing.Point((int)(finalVertices[i].X * Parameters.WorldPanelSizeFactor),
                        (int)(finalVertices[i].Y * Parameters.WorldPanelSizeFactor));
                    if (i > 0)
                        division +=
                            (int)Math.Sqrt((points[i].X - points[i - 1].X) * (points[i].X - points[i - 1].X) +
                            (points[i].Y - points[i - 1].Y) * (points[i].Y - points[i - 1].Y));
                }
                var degree = vertices.Count - 1;
                if (vertices.Count > 4)
                    degree = 3;
                var knotsDivider = Math.Abs(vertices.Count + degree - 4);
                var knotsDivision = 1.0 / knotsDivider;
                var knots = new double[vertices.Count + degree + 1];
                knots[0] = 0;
                knots[1] = 0;
                for (int i = 2; i < knots.Length - 2; i++)
                    knots[i] = (i - 2) * knotsDivision;
                knots[knots.Length - 2] = 1;
                knots[knots.Length - 1] = 1;
                var divider = (vertices.Count - degree) * knotsDivision / division;
                for (double t = (degree - 2) * knotsDivision; t <= (vertices.Count - degree + 1) * knotsDivision; t += divider)
                {
                    var point = Vector4.Zero();
                    for (int i = 0; i < vertices.Count; i++)
                    {
                        point = point + vertices[i] * CalculateNSplineValues(knots, i + 1, degree, t);
                    }
                    point = currentProjectionMatrix * point;
                    graphics.FillRectangle(brush, (float)(point.X * Parameters.WorldPanelSizeFactor),
                        (float)(point.Y * Parameters.WorldPanelSizeFactor), 1, 1);
                }
            }

            if (ChainEnabled && !ControlPointsEnabled)
            {
                foreach (var edge in Edges)
                {
                    graphics.DrawLine(new Pen(color), (float)finalVertices[edge.StartVertex].X * Parameters.WorldPanelSizeFactor,
                        (float)finalVertices[edge.StartVertex].Y * Parameters.WorldPanelSizeFactor,
                        (float)finalVertices[edge.EndVertex].X * Parameters.WorldPanelSizeFactor,
                        (float)finalVertices[edge.EndVertex].Y * Parameters.WorldPanelSizeFactor);
                }
            }
        }

        protected override void DrawWithAdditiveBlending(Bitmap bitmap, Graphics graphics, Matrix currentProjectionMatrix, Color color)
        {
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.CurrentOperationMatrix);
            currentMatrix = currentMatrix * CurrentOperationMatrix;
            var vertices = Vertices.Select(vertex => currentMatrix * vertex).ToList();
            var finalVertices = vertices.Select(vertex => currentProjectionMatrix * vertex).ToList();

            if (!ControlPointsEnabled && vertices.Count > 1)
            {
                var brush = new SolidBrush(color);
                var points = new System.Drawing.Point[vertices.Count];
                int division = 0;
                for (int i = 0; i < vertices.Count; i++)
                {
                    points[i] = new System.Drawing.Point((int)(finalVertices[i].X * Parameters.WorldPanelSizeFactor),
                        (int)(finalVertices[i].Y * Parameters.WorldPanelSizeFactor));
                    if (i < vertices.Count - 1)
                        division +=
                            (int)Math.Sqrt((points[i].X - points[i + 1].X) * (points[i].X - points[i + 1].X) +
                            (points[i].Y - points[i + 1].Y) * (points[i].Y - points[i + 1].Y));
                }
                var count = vertices.Count;
                if (vertices.Count > 4)
                    count = 4;
                var knotsDivider = vertices.Count + count - 5;
                var knotsDivision = 1.0 / knotsDivider;
                var knots = new double[knotsDivider + 5];
                knots[0] = 0;
                knots[1] = 0;
                for (int i = 0; i < knots.Length - 2; i++)
                    knots[i + 2] = i * knotsDivision;
                knots[knots.Length - 2] = 1;
                knots[knots.Length - 1] = 1;
                var divider = (vertices.Count - count + 1) * knotsDivision / division / 2;
                for (double t = (count - 3) * knotsDivision; t < (vertices.Count - count + 2) * knotsDivision; t += divider)
                {
                    var point = Vector4.Zero();
                    for (int i = 0; i < vertices.Count; i++)
                    {
                        point = point + vertices[i] * CalculateNSplineValues(knots, i + 1, count - 1, t);
                    }
                    point = currentProjectionMatrix * point;
                    int width = Parameters.WorldPanelWidth / 2;
                    int height = Parameters.WorldPanelHeight / 2;
                    var screenX = (int)(point.X * Parameters.WorldPanelSizeFactor);
                    var screenY = (int)(point.Y * Parameters.WorldPanelSizeFactor);
                    if (screenX + width >= bitmap.Width || screenX + width < 0 || screenY + height >= bitmap.Height || screenY + height < 0) continue;
                    graphics.FillRectangle(new SolidBrush(CombineColor(bitmap.GetPixel(screenX + width, screenY + height), color)),
                        screenX, screenY, 1, 1);
                }
            }

            if (ChainEnabled && !ControlPointsEnabled)
            {
                foreach (var edge in Edges)
                {
                    var customLine = new CustomLine(
                        new System.Drawing.Point(
                            (int)(finalVertices[edge.StartVertex].X * Parameters.WorldPanelSizeFactor),
                            (int)(finalVertices[edge.StartVertex].Y * Parameters.WorldPanelSizeFactor)),
                        new System.Drawing.Point(
                            (int)(finalVertices[edge.EndVertex].X * Parameters.WorldPanelSizeFactor),
                            (int)(finalVertices[edge.EndVertex].Y * Parameters.WorldPanelSizeFactor)));
                    customLine.Draw(bitmap, graphics, color, 1);
                }
            }
        }

        private double CalculateZeroSplineValues(double[] knots, int i, double t)
        {
            var result = 0.0;
            if (i <= 0 || i >= knots.Length) return result;
            if (knots[i - 1] <= t && knots[i] > t)
                result = 1;
            return result;
        }

        private double CalculateNSplineValues(double[] knots, int i, int n, double t)
        {
            if (n == 0)
                return CalculateZeroSplineValues(knots, i, t);
            var result = 0.0;
            if (i > 0 && i + n - 1 < knots.Length)
            {
                var fun = CalculateNSplineValues(knots, i, n - 1, t);
                var factor = knots[i + n - 1] - knots[i - 1];
                if (Math.Abs(factor) > Double.Epsilon)
                    result += (t - knots[i - 1]) / factor * fun;
            }
            if (i >= 0 && i + n < knots.Length - 1)
            {
                var fun = CalculateNSplineValues(knots, i + 1, n - 1, t);
                var factor = knots[i + n] - knots[i];
                if (Math.Abs(factor) > Double.Epsilon)
                    result += (knots[i + n] - t) / factor * fun;
            }
            return result;
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
            if (points.Count < 4)
                points.ForEach((item) => _bezierPoints.Add(new Point(item.GetCurrentPosition())));
            else
            {
                //_bezierPoints.Add(new Point(points[0].GetCurrentPosition()));
                //_bezierPoints.Add(new Point(points[0].GetCurrentPosition()));
                for (int i = 1; i < Children.Count - 1; i++)
                {
                    var lKnot = points[i - 1];
                    var mKnot = points[i];
                    var rKnot = points[i + 1];
                    var lPoint = i == 1
                        ? Vector4.Center(lKnot.GetCurrentPosition(), mKnot.GetCurrentPosition())
                        : Vector4.OneThird(mKnot.GetCurrentPosition(), lKnot.GetCurrentPosition());
                    var rPoint = i == Children.Count - 2
                        ? Vector4.Center(mKnot.GetCurrentPosition(), rKnot.GetCurrentPosition())
                        : Vector4.OneThird(mKnot.GetCurrentPosition(), rKnot.GetCurrentPosition());
                    if (i > 1)
                        _bezierPoints.Add(new Point(lPoint));
                    _bezierPoints.Add(new Point(Vector4.Center(lPoint, rPoint)));
                    if (i < Children.Count - 2)
                        _bezierPoints.Add(new Point(rPoint));
                }
                //_bezierPoints.Add(new Point(points[points.Count - 1].GetCurrentPosition()));
                //_bezierPoints.Add(new Point(points[points.Count - 1].GetCurrentPosition()));
            }
            _hiddenModels.AddRange(_bezierPoints);
            _bezierCurve = new BezierCurve(_bezierPoints, SpacePosition)
            {
                ChainEnabled = ChainEnabled,
                C2CurveParent = this
            };
            _bezierCurve.SetControlPointsEnablability(ControlPointsEnabled);
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

        public override void TogglePolygonialChain()
        {
            base.TogglePolygonialChain();
            if (_bezierCurve != null)
                _bezierCurve.ChainEnabled = ChainEnabled;
        }

        public override void SetControlPointsEnablability(bool state)
        {
            base.SetControlPointsEnablability(state);
            if (_bezierCurve != null)
                _bezierCurve.SetControlPointsEnablability(state);
        }

        internal void UpdateDeBoorPoints(int number, Point point)
        {
            var points = Children.Cast<Point>().ToList();
            if (points.Count < 4)
            {
                points[number].X = point.X;
                points[number].Y = point.Y;
                points[number].Z = point.Z;
            }
            var index = number / 3;
            Vector4 distance;
            Vector4 position;
            switch (number % 3)
            {
                case 0:
                    if (index == points.Count - 3)
                    {
                        position = Vector4.OneThird(points[index + 1].GetCurrentPosition(),
                           points[index].GetCurrentPosition());
                        distance = point.GetCurrentPosition() - position;
                        position = position + distance * 2;
                        distance = position - points[index + 1].GetCurrentPosition();
                        position = position + distance;
                        points[index + 2].X = position.X;
                        points[index + 2].Y = position.Y;
                        points[index + 2].Z = position.Z;
                    }
                    else
                    {
                        position = Vector4.OneThird(points[index + 1].GetCurrentPosition(),
                           points[index + 2].GetCurrentPosition());
                        distance = point.GetCurrentPosition() - position;
                        position = position + distance * 2;
                        distance = position - points[index + 1].GetCurrentPosition();
                        position = position + distance * ((number > 0) ? 2 : 1);
                        points[index].X = position.X;
                        points[index].Y = position.Y;
                        points[index].Z = position.Z;
                    }
                    break;
                case 1:
                    distance = point.GetCurrentPosition() - points[index + 1].GetCurrentPosition();
                    position = points[index + 1].GetBasePosition() + distance * 3;
                    points[index + 2].X = position.X;
                    points[index + 2].Y = position.Y;
                    points[index + 2].Z = position.Z;
                    break;
                case 2:
                    distance = point.GetCurrentPosition() - points[index + 2].GetCurrentPosition();
                    position = points[index + 2].GetBasePosition() + distance * 3;
                    points[index + 1].X = position.X;
                    points[index + 1].Y = position.Y;
                    points[index + 1].Z = position.Z;
                    break;
            }
            RecreateStructure();
        }
    }

}
