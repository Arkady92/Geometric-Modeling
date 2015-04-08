using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Mathematics;

namespace Models
{
    public class BezierCurve : ParametricGeometricModel
    {
        private static int _increment = 1;

        public BezierCurve(Vector4 position, ModelType modelType = ModelType.BezierCurve)
            : base(modelType, position, true)
        {
            CreateVertices();
            CreateEdges();
        }

        public BezierCurve(IEnumerable<Point> points, Vector4 position, ModelType modelType = ModelType.BezierCurve)
            : base(modelType, position, true)
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
                Children.Add(new Point(new Vector4(Vertices[i].X, Vertices[i].Y, Vertices[i].Z), this, i));
            }
        }

        public override string ToString()
        {
            if (CustomName != null)
                return "Bezier Curve C0 <" + CustomName + ">";
            return "Bezier Curve C0 <" + _increment++ + ">";
        }

        protected override void RecreateStructure(int number = 0)
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
            var brush = new SolidBrush(color);
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.CurrentOperationMatrix);
            currentMatrix = currentMatrix * CurrentOperationMatrix;

            var vertices = Vertices.Select(vertex => currentMatrix * vertex).ToList();
            var finalVertices = vertices.Select(vertex => currentProjectionMatrix * vertex).ToList();
            var points = new System.Drawing.Point[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                points[i] = new System.Drawing.Point((int)(finalVertices[i].X * Parameters.WorldPanelSizeFactor),
                    (int)(finalVertices[i].Y * Parameters.WorldPanelSizeFactor));
            }

            int count = 0;
            int division;
            for (int i = 0; i + 3 < points.Length; i += 3)
            {
                division = 0;
                for (int j = 0; j < 3; j++)
                {
                    division +=
                        (int)Math.Sqrt((points[j].X - points[j + 1].X) * (points[j].X - points[j + 1].X) +
                        (points[j].Y - points[j + 1].Y) * (points[j].Y - points[j + 1].Y));
                }
                DrawBezierSegment(graphics, currentProjectionMatrix, brush, new[] {vertices[i], 
                    vertices[i + 1], vertices[i + 2], vertices[i + 3]}, 2 * division);
                count += 3;
            }
            division = 0;
            if (count != 1)
            {
                var lastPoints = new Vector4[points.Length - count];
                for (int i = count; i < points.Length; i++)
                {
                    lastPoints[i - count] = vertices[i];
                    if (i < points.Length - 1)
                        division +=
                            (int)Math.Sqrt((points[i].X - points[i + 1].X) * (points[i].X - points[i + 1].X) +
                                            (points[i].Y - points[i + 1].Y) * (points[i].Y - points[i + 1].Y));
                }
                DrawBezierSegment(graphics, currentProjectionMatrix, brush, lastPoints, 2 * division);
            }

            if (Parameters.PolygonalChainEnabled && Parameters.ControlPointsEnabled)
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

        protected void DrawBezierSegment(Graphics graphics, Matrix currentProjectionMatrix, SolidBrush brush, Vector4[] points, int division)
        {
            var divider = 1.0 / division;
            const double epsilon = 0.000000001;
            switch (points.Length)
            {
                case 4:
                    for (double t = 0; t - epsilon <= 1; t += divider)
                    {
                        DrawCubicBezierPoint(t, graphics, currentProjectionMatrix, brush, points[0], points[1], points[2],
                            points[3]);
                    }
                    break;
                case 3:
                    for (double t = 0; t - epsilon <= 1; t += divider)
                    {
                        DrawTriangleBezierPoint(t, graphics, currentProjectionMatrix, brush, points[0], points[1], points[2]);
                    }
                    break;
                case 2:
                    for (double t = 0; t - epsilon <= 1; t += divider)
                    {
                        DrawLinearBezierPoint(t, graphics, currentProjectionMatrix, brush, points[0], points[1]);
                    }
                    break;
            }
        }

        private void DrawLinearBezierPoint(double t, Graphics graphics, Matrix currentProjectionMatrix, SolidBrush brush, Vector4 p1, Vector4 p2)
        {
            var point = new Vector4
            {
                X = (1 - t) * p1.X + t * p2.X,
                Y = (1 - t) * p1.Y + t * p2.Y,
                Z = (1 - t) * p1.Z + t * p2.Z,
                W = 1
            };
            point = currentProjectionMatrix * point;
            graphics.FillRectangle(brush, (float)(point.X * Parameters.WorldPanelSizeFactor),
                (float)(point.Y * Parameters.WorldPanelSizeFactor), 1, 1);
        }

        private void DrawTriangleBezierPoint(double t, Graphics graphics, Matrix currentProjectionMatrix, SolidBrush brush, Vector4 p1, Vector4 p2, Vector4 p3)
        {
            var point = new Vector4
            {
                X = (1 - t) * (1 - t) * p1.X + 2 * t * (1 - t) * p2.X + t * t * p3.X,
                Y = (1 - t) * (1 - t) * p1.Y + 2 * t * (1 - t) * p2.Y + t * t * p3.Y,
                Z = (1 - t) * (1 - t) * p1.Z + 2 * t * (1 - t) * p2.Z + t * t * p3.Z,
                W = 1
            };
            point = currentProjectionMatrix * point;
            graphics.FillRectangle(brush, (float)(point.X * Parameters.WorldPanelSizeFactor),
                (float)(point.Y * Parameters.WorldPanelSizeFactor), 1, 1);
        }

        private void DrawCubicBezierPoint(double t, Graphics graphics, Matrix currentProjectionMatrix, SolidBrush pen, Vector4 p1, Vector4 p2, Vector4 p3, Vector4 p4)
        {
            var point = new Vector4
            {
                X = (1 - t) * (1 - t) * (1 - t) * p1.X + 3 * t * (1 - t) * (1 - t) * p2.X + 3 * t * t * (1 - t) * p3.X + t * t * t * p4.X,
                Y = (1 - t) * (1 - t) * (1 - t) * p1.Y + 3 * t * (1 - t) * (1 - t) * p2.Y + 3 * t * t * (1 - t) * p3.Y + t * t * t * p4.Y,
                Z = (1 - t) * (1 - t) * (1 - t) * p1.Z + 3 * t * (1 - t) * (1 - t) * p2.Z + 3 * t * t * (1 - t) * p3.Z + t * t * t * p4.Z,
                W = 1
            };
            point = currentProjectionMatrix * point;
            graphics.FillRectangle(pen, (float)(point.X * Parameters.WorldPanelSizeFactor),
                (float)(point.Y * Parameters.WorldPanelSizeFactor), 1, 1);
        }

        protected override void DrawWithAdditiveBlending(Bitmap bitmap, Graphics graphics, Matrix currentProjectionMatrix, Color color)
        {
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.CurrentOperationMatrix);
            currentMatrix = currentMatrix * CurrentOperationMatrix;

            var vertices = Vertices.Select(vertex => currentMatrix * vertex).ToList();
            var finalVertices = vertices.Select(vertex => currentProjectionMatrix * vertex).ToList();
            var points = new System.Drawing.Point[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                points[i] = new System.Drawing.Point((int)(finalVertices[i].X * Parameters.WorldPanelSizeFactor),
                    (int)(finalVertices[i].Y * Parameters.WorldPanelSizeFactor));
            }

            int count = 0;
            int division;
            for (int i = 0; i + 3 < points.Length; i += 3)
            {
                division = 0;
                for (int j = 0; j < 3; j++)
                {
                    division +=
                        (int)Math.Sqrt((points[j].X - points[j + 1].X) * (points[j].X - points[j + 1].X) +
                        (points[j].Y - points[j + 1].Y) * (points[j].Y - points[j + 1].Y));
                }
                DrawCubicBezierSegmentWithAdditiveBlending(bitmap, graphics, currentProjectionMatrix, color, new[] {vertices[i], 
                    vertices[i + 1], vertices[i + 2], vertices[i + 3]}, 2 * division);
                count += 3;
            }
            division = 0;
            if (count != 1)
            {
                var lastPoints = new Vector4[points.Length - count];
                for (int i = count; i < points.Length; i++)
                {
                    lastPoints[i - count] = vertices[i];
                    if (i < points.Length - 1)
                        division +=
                        (int)Math.Sqrt((points[i].X - points[i + 1].X) * (points[i].X - points[i + 1].X) +
                                        (points[i].Y - points[i + 1].Y) * (points[i].Y - points[i + 1].Y));
                }
                DrawCubicBezierSegmentWithAdditiveBlending(bitmap, graphics, currentProjectionMatrix, color, lastPoints,
                    2 * division);
            }

            if (Parameters.PolygonalChainEnabled && Parameters.ControlPointsEnabled)
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

        private void DrawCubicBezierSegmentWithAdditiveBlending(Bitmap bitmap, Graphics graphics, Matrix currentProjectionMatrix,
            Color color, Vector4[] points, int division)
        {
            var divider = 1.0 / division;
            const double epsilon = 0.000000001;
            switch (points.Length)
            {
                case 4:
                    for (double t = 0; t - epsilon <= 1; t += divider)
                    {
                        DrawCubicBezierPointWithAdditiveBlending(t, bitmap, graphics, currentProjectionMatrix, color, points[0],
                            points[1], points[2], points[3]);
                    }
                    break;
                case 3:
                    for (double t = 0; t - epsilon <= 1; t += divider)
                    {
                        DrawTriangleBezierPointWithAdditiveBlending(t, bitmap, graphics, currentProjectionMatrix, color,
                            points[0], points[1], points[2]);
                    }
                    break;
                case 2:
                    for (double t = 0; t - epsilon <= 1; t += divider)
                    {
                        DrawLinearBezierPointWithAdditiveBlending(t, bitmap, graphics, currentProjectionMatrix, color, points[0],
                            points[1]);
                    }
                    break;
            }
        }

        private void DrawLinearBezierPointWithAdditiveBlending(double t, Bitmap bitmap, Graphics graphics,
            Matrix currentProjectionMatrix, Color color, Vector4 p1, Vector4 p2)
        {
            var point = new Vector4
            {
                X = (1 - t) * p1.X + t * p2.X,
                Y = (1 - t) * p1.Y + t * p2.Y,
                Z = (1 - t) * p1.Z + t * p2.Z,
                W = 1
            };
            point = currentProjectionMatrix * point;
            int width = Parameters.WorldPanelWidth / 2;
            int height = Parameters.WorldPanelHeight / 2;
            var screenX = (int)(point.X * Parameters.WorldPanelSizeFactor);
            var screenY = (int)(point.Y * Parameters.WorldPanelSizeFactor);
            if (screenX + width >= bitmap.Width || screenX + width < 0 || screenY + height >= bitmap.Height || screenY + height < 0) return;
            graphics.FillRectangle(new SolidBrush(CombineColor(bitmap.GetPixel(screenX + width, screenY + height), color)),
                screenX, screenY, 1, 1);
        }

        private void DrawTriangleBezierPointWithAdditiveBlending(double t, Bitmap bitmap, Graphics graphics,
            Matrix currentProjectionMatrix, Color color, Vector4 p1, Vector4 p2, Vector4 p3)
        {
            var point = new Vector4
            {
                X = (1 - t) * (1 - t) * p1.X + 2 * t * (1 - t) * p2.X + t * t * p3.X,
                Y = (1 - t) * (1 - t) * p1.Y + 2 * t * (1 - t) * p2.Y + t * t * p3.Y,
                Z = (1 - t) * (1 - t) * p1.Z + 2 * t * (1 - t) * p2.Z + t * t * p3.Z,
                W = 1
            };
            point = currentProjectionMatrix * point;
            int width = Parameters.WorldPanelWidth / 2;
            int height = Parameters.WorldPanelHeight / 2;
            var screenX = (int)(point.X * Parameters.WorldPanelSizeFactor);
            var screenY = (int)(point.Y * Parameters.WorldPanelSizeFactor);
            if (screenX + width >= bitmap.Width || screenX + width < 0 || screenY + height >= bitmap.Height || screenY + height < 0) return;
            graphics.FillRectangle(new SolidBrush(CombineColor(bitmap.GetPixel(screenX + width, screenY + height), color)),
                screenX, screenY, 1, 1);
        }

        private void DrawCubicBezierPointWithAdditiveBlending(double t, Bitmap bitmap, Graphics graphics, Matrix currentProjectionMatrix, Color color, Vector4 p1, Vector4 p2, Vector4 p3, Vector4 p4)
        {
            var point = new Vector4
            {
                X = (1 - t) * (1 - t) * (1 - t) * p1.X + 3 * t * (1 - t) * (1 - t) * p2.X + 3 * t * t * (1 - t) * p3.X + t * t * t * p4.X,
                Y = (1 - t) * (1 - t) * (1 - t) * p1.Y + 3 * t * (1 - t) * (1 - t) * p2.Y + 3 * t * t * (1 - t) * p3.Y + t * t * t * p4.Y,
                Z = (1 - t) * (1 - t) * (1 - t) * p1.Z + 3 * t * (1 - t) * (1 - t) * p2.Z + 3 * t * t * (1 - t) * p3.Z + t * t * t * p4.Z,
                W = 1
            };

            point = currentProjectionMatrix * point;
            int width = Parameters.WorldPanelWidth / 2;
            int height = Parameters.WorldPanelHeight / 2;
            var screenX = (int)(point.X * Parameters.WorldPanelSizeFactor);
            var screenY = (int)(point.Y * Parameters.WorldPanelSizeFactor);
            if (screenX + width >= bitmap.Width || screenX + width < 0 || screenY + height >= bitmap.Height || screenY + height < 0) return;
            graphics.FillRectangle(new SolidBrush(CombineColor(bitmap.GetPixel(screenX + width, screenY + height), color)),
                screenX, screenY, 1, 1);
        }

        private static Color CombineColor(Color color1, Color color2)
        {
            var a = color1.A + color2.A;
            if (a > 255) a = 255;
            var r = color1.R + color2.R;
            if (r > 255) r = 255;
            var g = color1.G + color2.G;
            if (g > 255) g = 255;
            var b = color1.B + color2.B;
            if (b > 255) b = 255;
            return Color.FromArgb(a, r, g, b);
        }
    }
}
