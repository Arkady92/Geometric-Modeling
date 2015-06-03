using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Mathematics;

namespace Models
{
    public class BezierSurface : ParametricGeometricModel
    {
        public readonly double Width;
        public readonly double Height;
        public readonly int PatchesLengthCount;
        public readonly int PatchesBreadthCount;
        public readonly bool IsCylindrical;
        protected List<Point[,]> PatchesPoints;

        protected readonly int GridBreadthCount;
        protected readonly int GridLengthFlatCount;
        protected readonly int GridLengthCylindricalCount;

        protected const int Degree = 3;
        private static int _increment = 1;

        public bool ChainEnabled { get; set; }

        public BezierSurface(Vector4 position, double width, double height, int patchesLengthCount, int patchesBreadthCount,
            bool isCylindrical, ModelType modelType = ModelType.BezierSurface)
            : base(modelType, position)
        {
            Width = width;
            Height = height;
            PatchesLengthCount = patchesLengthCount;
            PatchesBreadthCount = patchesBreadthCount;
            IsCylindrical = isCylindrical;
            GridBreadthCount = (Type == ModelType.BezierSurface) ? PatchesBreadthCount * Degree : PatchesBreadthCount + Degree - 1;
            GridLengthCylindricalCount = (Type == ModelType.BezierSurface) ? PatchesLengthCount * Degree :
                PatchesLengthCount + Degree - 3;
            GridLengthFlatCount = (Type == ModelType.BezierSurface) ? PatchesLengthCount * Degree : PatchesLengthCount + Degree - 1;
            CreateVertices();
            CreateEdges();
            CreatePatches();
            CustomName = (_increment++).ToString(CultureInfo.InvariantCulture);
        }

        public BezierSurface(IEnumerable<Point> points, Vector4 position, double width, double height, int patchesLengthCount,
            int patchesBreadthCount, bool isCylindrical, ModelType modelType = ModelType.BezierSurface)
            : base(modelType, position)
        {
            Width = width;
            Height = height;
            PatchesLengthCount = patchesLengthCount;
            PatchesBreadthCount = patchesBreadthCount;
            IsCylindrical = isCylindrical;
            GridBreadthCount = (Type == ModelType.BezierSurface) ? PatchesBreadthCount * Degree : PatchesBreadthCount + Degree - 1;
            GridLengthCylindricalCount = (Type == ModelType.BezierSurface) ? PatchesLengthCount * Degree :
                PatchesLengthCount + Degree - 3;
            GridLengthFlatCount = (Type == ModelType.BezierSurface) ? PatchesLengthCount * Degree : PatchesLengthCount + Degree -1;
            var enumerable = points as Point[] ?? points.ToArray();
            for (int i = 0; i < enumerable.Count(); i++)
            {
                var vector = enumerable[i].GetCurrentPosition();
                Vertices.Add(vector);
                enumerable[i].AddParent(this, i);
                enumerable[i].IsRemovableFromModel = false;
                Children.Add(enumerable[i]);
            }
            CreateEdges();
            CreatePatches();
            CustomName = (_increment++).ToString(CultureInfo.InvariantCulture);
            ChainEnabled = true;
        }

        protected virtual void CreatePatches()
        {
            if (IsCylindrical)
            {
                PatchesPoints = new List<Point[,]>();

                for (var i = 0; i < PatchesBreadthCount; i++)
                {
                    for (var j = 0; j < PatchesLengthCount; j++)
                    {
                        var vertices = new Point[Degree + 1, Degree + 1];
                        for (int k = i * Degree; k < (i + 1) * Degree + 1; k++)
                            for (int l = j * Degree; l < (j + 1) * Degree + 1; l++)
                                vertices[k - i * Degree, l - j * Degree] = Children[k * (GridLengthCylindricalCount)
                                    + (l % (GridLengthCylindricalCount))] as Point;
                        PatchesPoints.Add(vertices);
                    }
                }
            }
            else
            {
                PatchesPoints = new List<Point[,]>();

                for (var i = 0; i < PatchesBreadthCount; i++)
                {
                    for (var j = 0; j < PatchesLengthCount; j++)
                    {
                        var vertices = new Point[Degree + 1, Degree + 1];
                        for (int k = i * Degree; k < (i + 1) * Degree + 1; k++)
                            for (int l = j * Degree; l < (j + 1) * Degree + 1; l++)
                                vertices[k - i * Degree, l - j * Degree] =
                                    Children[k * (GridLengthFlatCount + 1) + l] as Point;
                        PatchesPoints.Add(vertices);
                    }
                }
            }
        }

        protected override void CreateEdges()
        {

            if (IsCylindrical)
            {
                int idx;
                for (var i = 0; i < GridBreadthCount; i++)
                {
                    for (var j = 0; j < GridLengthCylindricalCount; j++)
                    {
                        idx = i * (GridLengthCylindricalCount) + j;
                        Edges.Add(new Edge(idx, i * (GridLengthCylindricalCount) + ((j + 1) % (GridLengthCylindricalCount))));
                        Edges.Add(new Edge(idx, (i + 1) * (GridLengthCylindricalCount) + j));
                    }
                }
                idx = (GridLengthCylindricalCount) * GridBreadthCount;
                for (int i = 0; i < GridLengthCylindricalCount - 1; i++)
                    Edges.Add(new Edge(idx, ++idx));
                Edges.Add(new Edge(idx, (GridLengthCylindricalCount) * GridBreadthCount));
            }
            else
            {
                int idx;
                for (var i = 0; i < GridBreadthCount; i++)
                {
                    for (var j = 0; j < GridLengthFlatCount; j++)
                    {
                        idx = i * (GridLengthFlatCount + 1) + j;
                        Edges.Add(new Edge(idx, idx + 1));
                        Edges.Add(new Edge(idx, (i + 1) * (GridLengthFlatCount + 1) + j));
                    }
                }
                idx = (GridLengthFlatCount + 1) * GridBreadthCount;
                for (int i = 0; i < GridLengthFlatCount; i++)
                    Edges.Add(new Edge(idx, ++idx));
                for (int i = 0; i < GridBreadthCount; i++)
                {
                    idx = (GridLengthFlatCount + 1) * i + GridLengthFlatCount;
                    Edges.Add(new Edge(idx, idx + GridLengthFlatCount + 1));

                }
            }
        }

        protected override void CreateVertices()
        {
            if (IsCylindrical)
            {
                var stepAlpha = 2 * Math.PI / (GridLengthCylindricalCount);
                var stepY = Height / (GridBreadthCount);
                for (var i = 0; i < GridBreadthCount + 1; i++)
                {
                    var posY = -Height / 2 + i * stepY;
                    for (var j = 0; j < GridLengthCylindricalCount; j++)
                    {
                        var posX = Width * Math.Cos(j * stepAlpha);
                        var posZ = Width * Math.Sin(j * stepAlpha);
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
                var stepX = Width / (GridLengthFlatCount);
                var stepY = Height / (GridBreadthCount);
                for (var i = 0; i < GridBreadthCount + 1; i++)
                {
                    var posY = -Height / 2 + i * stepY;
                    for (var j = 0; j < GridLengthFlatCount + 1; j++)
                    {
                        var posX = -Width / 2 + j * stepX;
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

        protected virtual void DrawPatches(Graphics graphics, Matrix currentProjectionMatrix, Color color, Bitmap bitmap = null, bool blendingEnabled = false)
        {
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.OperationMatrix);
            currentMatrix = currentMatrix * OperationMatrix;
            var pen = new Pen(color);

            var uStep = 1.0 / (Parameters.SurfaceGridResolutionX - 1);
            var vStep = 1.0 / (Parameters.SurfaceGridResolutionY - 1);
            foreach (var points in PatchesPoints)
            {
                var xMatrix = new Matrix(Degree + 1, Degree + 1);
                var yMatrix = new Matrix(Degree + 1, Degree + 1);
                var zMatrix = new Matrix(Degree + 1, Degree + 1);
                for (int i = 0; i < Degree + 1; i++)
                {
                    for (int j = 0; j < Degree + 1; j++)
                    {
                        var point = currentMatrix * points[i, j].GetCurrentPositionWithoutMineTransformations(this);
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
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.OperationMatrix);
            currentMatrix = currentProjectionMatrix * currentMatrix * OperationMatrix;

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
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.OperationMatrix);
            currentMatrix = currentProjectionMatrix * currentMatrix * OperationMatrix;

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

        protected override void RecreateStructure(int number = 0)
        {
            base.RecreateStructure(number);
            CreatePatches();
        }

        public List<Point[,]> GetPatches()
        {
            return PatchesPoints;
        }
    }
}
