﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Mathematics;
using Matrix = Mathematics.Matrix;

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
            CustomName = (_increment++).ToString(CultureInfo.InvariantCulture);
        }

        public BezierCurveC2(IEnumerable<Point> points, Vector4 position, List<GeometricModel> hiddenModels = null,
            bool polygonialChainEnabled = true, bool controlPointsEnabled = false)
            : base(points, position, ModelType.BezierCurveC2, polygonialChainEnabled)
        {
            ControlPointsEnabled = controlPointsEnabled;
            _hiddenModels = hiddenModels;
            _bezierPoints = new List<Point>();
            CalculateBezierPoints();
            CustomName = (_increment++).ToString(CultureInfo.InvariantCulture);
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
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.OperationMatrix);
            currentMatrix = currentMatrix * OperationMatrix;
            var vertices = Vertices.Select(vertex => currentMatrix * vertex).ToList();
            var finalVertices = vertices.Select(vertex => currentProjectionMatrix * vertex).ToList();

            if (!ControlPointsEnabled && vertices.Count > 1)
            {
                Vector4 lastPoint = Vector4.Zero();
                var first = true;
                var pen = new Pen(color);
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
                division *= 2;
                var degree = vertices.Count - 1;
                if (vertices.Count > 4)
                    degree = 3;
                //var knotsDivider = Math.Abs(vertices.Count + degree - 2);
                //var knotsDivision = 1.0 / knotsDivider;
                //var knots = new double[vertices.Count + degree + 1];
                //knots[0] = 0;
                //knots[1] = 0;
                //for (int i = 2; i < knots.Length - 2; i++)
                //    knots[i] = (i - 1) * knotsDivision;
                //knots[knots.Length - 2] = 1;
                //knots[knots.Length - 1] = 1;
                //var divider = (vertices.Count - degree) * knotsDivision / division;
                //for (double t = (degree - 1) * knotsDivision; t <= (vertices.Count - 1) * knotsDivision;
                //    t += divider)
                //{
                //    var point = Vector4.Zero();
                //    for (int i = 0; i < vertices.Count; i++)
                //    {
                //        point = point + vertices[i] * Splines.CalculateNSplineValues(knots, i + 1, degree, t);
                //    }
                //    point = currentProjectionMatrix * point;
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
                for (double t = ((degree > 2) ? 1 : 0) * knotsDivision; t <= ((degree > 2) ? vertices.Count - 2 : 1) * knotsDivision;
                    t += divider)
                {
                    var point = Vector4.Zero();
                    for (int i = 0; i < vertices.Count; i++)
                    {
                        point = point + vertices[i] * Splines.CalculateNSplineValues(knots, i + 1, degree, t);
                    }
                    point = currentProjectionMatrix * point;
                    if (first)
                    {
                        first = false;
                        lastPoint = point;
                        continue;
                    }
                    graphics.DrawLine(pen, (float)(lastPoint.X * Parameters.WorldPanelSizeFactor),
                        (float)(lastPoint.Y * Parameters.WorldPanelSizeFactor),
                        (float)(point.X * Parameters.WorldPanelSizeFactor),
                        (float)(point.Y * Parameters.WorldPanelSizeFactor));
                    lastPoint = point;
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
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.OperationMatrix);
            currentMatrix = currentMatrix * OperationMatrix;
            var vertices = Vertices.Select(vertex => currentMatrix * vertex).ToList();
            var finalVertices = vertices.Select(vertex => currentProjectionMatrix * vertex).ToList();

            if (!ControlPointsEnabled && vertices.Count > 1)
            {
                Vector4 lastPoint = Vector4.Zero();
                var first = true;
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
                division *= 2;
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
                for (double t = ((degree > 2) ? 1 : 0) * knotsDivision; t <= ((degree > 2) ? vertices.Count - 2 : 1) * knotsDivision;
                    t += divider)
                {
                    var point = Vector4.Zero();
                    for (int i = 0; i < vertices.Count; i++)
                    {
                        point = point + vertices[i] * Splines.CalculateNSplineValues(knots, i + 1, degree, t);
                    }
                    point = currentProjectionMatrix * point;
                    if (first)
                    {
                        first = false;
                        lastPoint = point;
                        continue;
                    }
                    var customLine = new CustomLine(
                        new System.Drawing.Point(
                            (int)(lastPoint.X * Parameters.WorldPanelSizeFactor),
                            (int)(lastPoint.Y * Parameters.WorldPanelSizeFactor)),
                        new System.Drawing.Point(
                            (int)(point.X * Parameters.WorldPanelSizeFactor),
                            (int)(point.Y * Parameters.WorldPanelSizeFactor)));
                    customLine.Draw(bitmap, graphics, color, 1);
                    lastPoint = point;
                    /*int width = Parameters.WorldPanelWidth / 2;
                    int height = Parameters.WorldPanelHeight / 2;
                    var screenX = (int)(point.X * Parameters.WorldPanelSizeFactor);
                    var screenY = (int)(point.Y * Parameters.WorldPanelSizeFactor);
                    if (screenX + width >= bitmap.Width || screenX + width < 0 || screenY + height >= bitmap.Height || screenY + height < 0) continue;
                    graphics.FillRectangle(new SolidBrush(CombineColor(bitmap.GetPixel(screenX + width, screenY + height), color)),
                        screenX, screenY, 1, 1);*/
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

        public void CalculateBezierPoints()
        {
            var points = Children.Cast<Point>().ToList();
            if (_hiddenModels != null)
                foreach (var bezierPoint in _bezierPoints)
                {
                    _hiddenModels.Remove(bezierPoint);
                }
            _bezierPoints.Clear();
            if (points.Count == 0) return;
            if (points.Count < 4)
                points.ForEach(item => _bezierPoints.Add(new Point(item.GetCurrentPosition())));
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
            foreach (var bezierPoint in _bezierPoints)
            {
                bezierPoint.IsRemovableFromScene = false;
            }
            if (_hiddenModels != null)
                _hiddenModels.AddRange(_bezierPoints);
            _bezierCurve = new BezierCurve(_bezierPoints, SpacePosition)
            {
                ChainEnabled = ChainEnabled,
                CurveParent = this
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

        public override void TogglePolygonialChain()
        {
            base.TogglePolygonialChain();
            if (_bezierCurve != null)
                _bezierCurve.ChainEnabled = ChainEnabled;
        }

        public override void SetControlPointsEnablability(bool state)
        {
            base.SetControlPointsEnablability(state);
            CalculateBezierPoints();
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
