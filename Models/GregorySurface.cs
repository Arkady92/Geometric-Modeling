using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Mathematics;

namespace Models
{
    public class GregorySurface : BezierSurface
    {
        private Vector4[,] _parametrizedPoints;

        public GregorySurface(IEnumerable<Point> points, Vector4 position)
            : base(points, position, 1, 1, 1, 1, false, ModelType.GregorySurface) { }

        protected override void CreateVertices() { }

        protected override void CreateEdges()
        {
            for (int i = 0; i < 11; i++)
                Edges.Add(new Edge(i, i + 1));
            Edges.Add(new Edge(11, 0));
            Edges.Add(new Edge(1, 13));
            Edges.Add(new Edge(2, 14));
            Edges.Add(new Edge(4, 15));
            Edges.Add(new Edge(5, 16));
            Edges.Add(new Edge(7, 17));
            Edges.Add(new Edge(8, 18));
            Edges.Add(new Edge(10, 19));
            Edges.Add(new Edge(11, 12));
        }

        protected override void CreatePatches()
        {
            _parametrizedPoints = new Vector4[4, 4];
            _parametrizedPoints[0, 0] = Children[0].GetCurrentPosition();
            _parametrizedPoints[0, 1] = Children[1].GetCurrentPosition();
            _parametrizedPoints[0, 2] = Children[2].GetCurrentPosition();
            _parametrizedPoints[0, 3] = Children[3].GetCurrentPosition();
            _parametrizedPoints[1, 3] = Children[4].GetCurrentPosition();
            _parametrizedPoints[2, 3] = Children[5].GetCurrentPosition();
            _parametrizedPoints[3, 3] = Children[6].GetCurrentPosition();
            _parametrizedPoints[3, 2] = Children[7].GetCurrentPosition();
            _parametrizedPoints[3, 1] = Children[8].GetCurrentPosition();
            _parametrizedPoints[3, 0] = Children[9].GetCurrentPosition();
            _parametrizedPoints[2, 0] = Children[10].GetCurrentPosition();
            _parametrizedPoints[1, 0] = Children[11].GetCurrentPosition();
            _parametrizedPoints[1, 1] = Children[12].GetCurrentPosition();
            _parametrizedPoints[1, 2] = Children[14].GetCurrentPosition();
            _parametrizedPoints[2, 2] = Children[16].GetCurrentPosition();
            _parametrizedPoints[2, 1] = Children[18].GetCurrentPosition();
        }

        private void ParametrizePoints(double u, double v)
        {
            if (Math.Abs(u + v) < Double.Epsilon)
                _parametrizedPoints[1, 1] = (Children[12].GetCurrentPosition() + Children[13].GetCurrentPosition()) * 0.5;
            else
                _parametrizedPoints[1, 1] = (Children[12].GetCurrentPosition() * u + Children[13].GetCurrentPosition() * v) *
                    (1.0 / (u + v));
            if (Math.Abs(1 - u + v) < Double.Epsilon)
                _parametrizedPoints[1, 2] = (Children[14].GetCurrentPosition() + Children[15].GetCurrentPosition()) * 0.5;
            else
                _parametrizedPoints[1, 2] = (Children[14].GetCurrentPosition() * (1 - u) + Children[15].GetCurrentPosition() * v) *
                    (1.0 / (1 - u + v));
            if (Math.Abs(1 - u + 1 - v) < Double.Epsilon)
                _parametrizedPoints[2, 2] = (Children[16].GetCurrentPosition() + Children[17].GetCurrentPosition()) * 0.5;
            else
                _parametrizedPoints[2, 2] = (Children[16].GetCurrentPosition() * (1 - u) + Children[17].GetCurrentPosition() * (1 - v)) *
                    (1.0 / (1 - u + 1 - v));
            if (Math.Abs(u + 1 - v) < Double.Epsilon)
                _parametrizedPoints[2, 1] = (Children[18].GetCurrentPosition() + Children[19].GetCurrentPosition()) * 0.5;
            else
                _parametrizedPoints[2, 1] = (Children[18].GetCurrentPosition() * u + Children[19].GetCurrentPosition() * (1 - v)) *
                    (1.0 / (u + 1 - v));
        }

        protected override void DrawPatches(Graphics graphics, Matrix currentProjectionMatrix, Color color, Bitmap bitmap = null,
            bool blendingEnabled = false)
        {
            var pen = new Pen(color);

            var uStep = 1.0 / (Parameters.SurfaceGridResolutionX - 1);
            var vStep = 1.0 / (Parameters.SurfaceGridResolutionY - 1);
            var xMatrix = new Matrix(4, 4);
            var yMatrix = new Matrix(4, 4);
            var zMatrix = new Matrix(4, 4);

            for (double u = 0; u < 1 + uStep / 2; u += uStep)
            {
                bool first = true;
                var lastPoint = Vector4.Zero();
                const double vDiv = 0.01;
                for (double v = 0; v < 1 + vDiv / 2; v += vDiv)
                {
                    ParametrizePoints(u, v);
                    for (int i = 0; i < Degree + 1; i++)
                    {
                        for (int j = 0; j < Degree + 1; j++)
                        {
                            xMatrix[i, j] = _parametrizedPoints[i, j].X;
                            yMatrix[i, j] = _parametrizedPoints[i, j].Y;
                            zMatrix[i, j] = _parametrizedPoints[i, j].Z;
                        }
                    }
                    var leftVector = CalculateBezierVector(u);
                    var rightVector = CalculateBezierVector(v);
                    var point = new Vector4(leftVector * xMatrix * rightVector, leftVector * yMatrix * rightVector,
                        leftVector * zMatrix * rightVector);
                    point = currentProjectionMatrix * point;
                    if (first)
                    {
                        first = false;
                        lastPoint = point;
                        continue;
                    }
                    if (blendingEnabled)
                    {
                        var customLine = new CustomLine(new System.Drawing.Point(
                            (int)(lastPoint.X * Parameters.WorldPanelSizeFactor),
                            (int)(lastPoint.Y * Parameters.WorldPanelSizeFactor)),
                            new System.Drawing.Point(
                                (int)(point.X * Parameters.WorldPanelSizeFactor),
                                (int)(point.Y * Parameters.WorldPanelSizeFactor)));
                        customLine.Draw(bitmap, graphics, color, 1);
                    }
                    else
                    {
                        graphics.DrawLine(pen, (float)(lastPoint.X * Parameters.WorldPanelSizeFactor),
                            (float)(lastPoint.Y * Parameters.WorldPanelSizeFactor),
                            (float)(point.X * Parameters.WorldPanelSizeFactor),
                            (float)(point.Y * Parameters.WorldPanelSizeFactor));
                    }
                    lastPoint = point;
                }
            }
            for (double v = 0; v < 1 + vStep / 2; v += vStep)
            {
                bool first = true;
                var lastPoint = Vector4.Zero();
                const double uDiv = 0.01;
                for (double u = 0; u < 1 + uDiv / 2; u += uDiv)
                {
                    ParametrizePoints(u, v);
                    for (int i = 0; i < Degree + 1; i++)
                    {
                        for (int j = 0; j < Degree + 1; j++)
                        {
                            xMatrix[i, j] = _parametrizedPoints[i, j].X;
                            yMatrix[i, j] = _parametrizedPoints[i, j].Y;
                            zMatrix[i, j] = _parametrizedPoints[i, j].Z;
                        }
                    }
                    var leftVector = CalculateBezierVector(u);
                    var rightVector = CalculateBezierVector(v);
                    var point = new Vector4(leftVector * xMatrix * rightVector, leftVector * yMatrix * rightVector,
                        leftVector * zMatrix * rightVector);
                    point = currentProjectionMatrix * point;
                    if (first)
                    {
                        first = false;
                        lastPoint = point;
                        continue;
                    }
                    if (blendingEnabled)
                    {
                        var customLine = new CustomLine(new System.Drawing.Point(
                       (int)(lastPoint.X * Parameters.WorldPanelSizeFactor),
                       (int)(lastPoint.Y * Parameters.WorldPanelSizeFactor)),
                       new System.Drawing.Point(
                           (int)(point.X * Parameters.WorldPanelSizeFactor),
                           (int)(point.Y * Parameters.WorldPanelSizeFactor)));
                        customLine.Draw(bitmap, graphics, color, 1);
                    }
                    else
                    {
                        graphics.DrawLine(pen, (float)(lastPoint.X * Parameters.WorldPanelSizeFactor),
                            (float)(lastPoint.Y * Parameters.WorldPanelSizeFactor),
                            (float)(point.X * Parameters.WorldPanelSizeFactor),
                            (float)(point.Y * Parameters.WorldPanelSizeFactor));
                    }
                    lastPoint = point;
                }
            }
        }

        protected override void DrawModel(Graphics graphics, Matrix currentProjectionMatrix, Color color)
        {
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.OperationMatrix);
            currentMatrix = currentProjectionMatrix * currentMatrix * OperationMatrix;

            if (ChainEnabled)
            {
                var pen = new Pen(Color.Chartreuse);
                var vertices = Vertices.Select(vertex => currentMatrix * vertex).ToList();
                foreach (var edge in Edges)
                {
                    graphics.DrawLine(pen, (float)vertices[edge.StartVertex].X * Parameters.WorldPanelSizeFactor,
                        (float)vertices[edge.StartVertex].Y * Parameters.WorldPanelSizeFactor,
                        (float)vertices[edge.EndVertex].X * Parameters.WorldPanelSizeFactor,
                        (float)vertices[edge.EndVertex].Y * Parameters.WorldPanelSizeFactor);
                }
            }
            DrawPatches(graphics, currentProjectionMatrix, color);
        }
    }
}
