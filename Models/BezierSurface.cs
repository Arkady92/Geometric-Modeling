using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Mathematics;

namespace Models
{
    public class BezierSurface : ParametricGeometricModel
    {
        private readonly double _width;
        private readonly double _height;
        private readonly int _patchesLengthCount;
        private readonly int _patchesBreadthCount;
        private readonly bool _isCylindrical;
        private List<Vector4[,]> _patchesVertices;

        private const int Degree = 3;

        public bool ChainEnabled { get; set; }

        public BezierSurface(Vector4 position, double width, double height, int patchesLengthCount, int patchesBreadthCount,
            bool isCylindrical)
            : base(ModelType.BezierSurface, position)
        {
            _width = width;
            _height = height;
            _patchesLengthCount = patchesLengthCount;
            _patchesBreadthCount = patchesBreadthCount;
            _isCylindrical = isCylindrical;
            CreateVertices();
            CreateEdges();
            CreatePatches();
        }

        private void CreatePatches()
        {
            if (_isCylindrical)
            {
                _patchesVertices = new List<Vector4[,]>();

                for (var i = 0; i < _patchesBreadthCount; i++)
                {
                    for (var j = 0; j < _patchesLengthCount; j++)
                    {
                        var vertices = new Vector4[Degree + 1, Degree + 1];
                        for (int k = i * Degree; k < (i + 1) * Degree + 1; k++)
                            for (int l = j * Degree; l < (j + 1) * Degree + 1; l++)
                                vertices[k - i * Degree, l - j * Degree] = Vertices[k * (_patchesLengthCount * Degree)
                                    + (l % (_patchesLengthCount * Degree))];
                        _patchesVertices.Add(vertices);
                    }
                }
            }
            else
            {
                _patchesVertices = new List<Vector4[,]>();

                for (var i = 0; i < _patchesBreadthCount; i++)
                {
                    for (var j = 0; j < _patchesLengthCount; j++)
                    {
                        var vertices = new Vector4[Degree + 1, Degree + 1];
                        for (int k = i * Degree; k < (i + 1) * Degree + 1; k++)
                            for (int l = j * Degree; l < (j + 1) * Degree + 1; l++)
                                vertices[k - i * Degree, l - j * Degree] = Vertices[k * (_patchesLengthCount * Degree + 1) + l];
                        _patchesVertices.Add(vertices);
                    }
                }
            }
        }

        protected override void CreateEdges()
        {

            if (_isCylindrical)
            {
                int idx;
                for (var i = 0; i < _patchesBreadthCount * Degree; i++)
                {
                    for (var j = 0; j < _patchesLengthCount * Degree; j++)
                    {
                        idx = i * (_patchesLengthCount * Degree) + j;
                        Edges.Add(new Edge(idx, i * (_patchesLengthCount * Degree) + ((j + 1) % (_patchesLengthCount * Degree))));
                        Edges.Add(new Edge(idx, (i + 1) * (_patchesLengthCount * Degree) + j));
                    }
                }
                idx = (_patchesLengthCount * Degree) * _patchesBreadthCount * Degree;
                for (int i = 0; i < _patchesLengthCount * Degree - 1; i++)
                    Edges.Add(new Edge(idx, ++idx));
                Edges.Add(new Edge(idx, (_patchesLengthCount * Degree) * _patchesBreadthCount * Degree));
            }
            else
            {
                int idx;
                for (var i = 0; i < _patchesBreadthCount * Degree; i++)
                {
                    for (var j = 0; j < _patchesLengthCount * Degree; j++)
                    {
                        idx = i * (_patchesLengthCount * Degree + 1) + j;
                        Edges.Add(new Edge(idx, idx + 1));
                        Edges.Add(new Edge(idx, (i + 1) * (_patchesLengthCount * Degree + 1) + j));
                    }
                }
                idx = (_patchesLengthCount * Degree + 1) * _patchesBreadthCount * Degree;
                for (int i = 0; i < _patchesLengthCount * Degree; i++)
                    Edges.Add(new Edge(idx, ++idx));
                for (int i = 0; i < _patchesBreadthCount * Degree; i++)
                {
                    idx = (_patchesLengthCount * Degree + 1) * i + _patchesLengthCount * Degree;
                    Edges.Add(new Edge(idx, idx + _patchesLengthCount * Degree + 1));

                }
            }
        }

        protected override void CreateVertices()
        {
            if (_isCylindrical)
            {
                var stepAlpha = 2 * Math.PI / (_patchesLengthCount * Degree);
                var stepY = _height / (_patchesBreadthCount * Degree);
                for (var i = 0; i < _patchesBreadthCount * Degree + 1; i++)
                {
                    var posY = -_height / 2 + i * stepY;
                    for (var j = 0; j < _patchesLengthCount * Degree; j++)
                    {
                        var posX = _width * Math.Cos(j *stepAlpha);
                        var posZ = _width * Math.Sin(j * stepAlpha);
                        Vertices.Add(new Vector4(posX, posY, posZ));
                    }
                }
                for (var i = 0; i < Vertices.Count; i++)
                {
                    Children.Add(new Point(new Vector4(Vertices[i].X, Vertices[i].Y, Vertices[i].Z), this, i) { IsRemovableFromModel = false });
                }
            }
            else
            {
                var stepX = _width / (_patchesLengthCount * Degree);
                var stepY = _height / (_patchesBreadthCount * Degree);
                for (var i = 0; i < _patchesBreadthCount * Degree + 1; i++)
                {
                    var posY = -_height / 2 + i * stepY;
                    for (var j = 0; j < _patchesLengthCount * Degree + 1; j++)
                    {
                        var posX = -_width / 2 + j * stepX;
                        Vertices.Add(new Vector4(posX, posY, 0));
                    }
                }
                for (var i = 0; i < Vertices.Count; i++)
                {
                    Children.Add(new Point(new Vector4(Vertices[i].X, Vertices[i].Y, Vertices[i].Z), this, i) { IsRemovableFromModel = false });
                }
            }
        }

        public virtual void TogglePolygonialChain()
        {
            ChainEnabled = !ChainEnabled;
        }

        public override void UpdateModel() { }

        private void DrawPatches(Graphics graphics, Matrix currentProjectionMatrix, Color color, Bitmap bitmap = null, bool blendingEnabled = false)
        {
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.CurrentOperationMatrix);
            currentMatrix = currentMatrix * CurrentOperationMatrix;
            var pen = new Pen(color);

            var uStep = 1.0 / (Parameters.SurfaceGridResolutionX - 1);
            var vStep = 1.0 / (Parameters.SurfaceGridResolutionY - 1);
            foreach (var points in _patchesVertices)
            {
                var xMatrix = new Matrix(Degree + 1, Degree + 1);
                var yMatrix = new Matrix(Degree + 1, Degree + 1);
                var zMatrix = new Matrix(Degree + 1, Degree + 1);
                for (int i = 0; i < Degree + 1; i++)
                {
                    for (int j = 0; j < Degree + 1; j++)
                    {
                        var point = currentMatrix * points[i, j];
                        xMatrix[i, j] = point.X;
                        yMatrix[i, j] = point.Y;
                        zMatrix[i, j] = point.Z;
                    }
                }

                for (double u = 0; u < 1 + uStep / 2; u += uStep)
                {
                    bool first = true;
                    var lastPoint = Vector4.Zero();
                    const double vDiv = 0.01;
                    for (double v = 0; v < 1 + vDiv / 2; v += vDiv)
                    {
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
        }

        private Vector4 CalculateBezierVector(double t)
        {
            return new Vector4((1 - t) * (1 - t) * (1 - t), 3 * t * (1 - t) * (1 - t), 3 * t * t * (1 - t), t * t * t);
        }

        protected override void DrawModel(Graphics graphics, Matrix currentProjectionMatrix, Color color)
        {
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.CurrentOperationMatrix);
            currentMatrix = currentProjectionMatrix * currentMatrix * CurrentOperationMatrix;

            if (ChainEnabled)
            {
                var pen = new Pen(color);
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

        protected override void DrawWithAdditiveBlending(Bitmap bitmap, Graphics graphics, Matrix currentProjectionMatrix, Color color)
        {
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.CurrentOperationMatrix);
            currentMatrix = currentProjectionMatrix * currentMatrix * CurrentOperationMatrix;

            if (ChainEnabled)
            {
                var vertices = Vertices.Select(vertex => currentMatrix * vertex).ToList();
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
            DrawPatches(graphics, currentProjectionMatrix, color, bitmap, true);
        }
    }
}
