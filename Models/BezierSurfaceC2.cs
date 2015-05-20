using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Mathematics;

namespace Models
{
    public class BezierSurfaceC2 : BezierSurface
    {
        private static int _increment = 1;

        public BezierSurfaceC2(Vector4 position, double width, double height, int patchesLengthCount,
            int patchesBreadthCount, bool isCylindrical)
            : base(position, width, height, patchesLengthCount,
                patchesBreadthCount, isCylindrical, ModelType.BezierSurfaceC2)
        {
            CustomName = (_increment++).ToString(CultureInfo.InvariantCulture);
        }

        public BezierSurfaceC2(IEnumerable<Point> points, Vector4 position, double width, double height, int patchesLengthCount,
            int patchesBreadthCount, bool isCylindrical)
            : base(points, position, width, height, patchesLengthCount,
                patchesBreadthCount, isCylindrical, ModelType.BezierSurfaceC2)
        {
            CustomName = (_increment++).ToString(CultureInfo.InvariantCulture);
        }

        protected override void CreatePatches()
        {
            if (IsCylindrical)
            {
                PatchesPoints = new List<Point[,]>();

                for (var i = 0; i < PatchesBreadthCount; i++)
                {
                    for (var j = 0; j < PatchesLengthCount; j++)
                    {
                        var vertices = new Point[Degree + 1, Degree + 1];
                        for (int k = i; k < i + Degree + 1; k++)
                            for (int l = j; l < j + Degree + 1; l++)
                                vertices[k - i, l - j] = Children[k * (GridLengthCylindricalCount)
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
                        for (int k = i; k < i + Degree + 1; k++)
                            for (int l = j; l < j + Degree + 1; l++)
                                vertices[k - i, l - j] =
                                    Children[k * (GridLengthFlatCount + 1) + l] as Point;
                        PatchesPoints.Add(vertices);
                    }
                }
            }
        }

        protected override void DrawPatches(Graphics graphics, Matrix currentProjectionMatrix, Color color, Bitmap bitmap = null, bool blendingEnabled = false)
        {
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.OperationMatrix);
            currentMatrix = currentMatrix * OperationMatrix;
            var pen = new Pen(color);

            var knots = Splines.GetKnotsDoubledEnds(Degree + 5);
            const double knotsDivision = 1.0 / (Degree + 2);
            var uStep = 1.0 / (Parameters.SurfaceGridResolutionX - 1) * knotsDivision;
            var vStep = 1.0 / (Parameters.SurfaceGridResolutionY - 1) * knotsDivision;
            foreach (var points in PatchesPoints)
            {
                var currentPoints = new Vector4[Degree + 1, Degree + 1];
                for (int i = 0; i < Degree + 1; i++)
                    for (int j = 0; j < Degree + 1; j++)
                        currentPoints[i, j] = currentMatrix * points[i, j].GetCurrentPositionWithoutMineTransformations(this);

                for (double u = 2 * knotsDivision; u < 3 * knotsDivision + uStep / 2; u += uStep)
                {
                    bool first = true;
                    var lastPoint = Vector4.Zero();
                    const double vDiv = 0.01;
                    var uSplineValues = new double[Degree + 1];
                    for (int i = 0; i < Degree + 1; i++)
                        uSplineValues[i] = Splines.CalculateNSplineValues(knots, i + 1, Degree, u);
                    for (double v = 2 * knotsDivision; v < 3 * knotsDivision + vDiv / 2; v += vDiv)
                    {
                        var vSplineValues = new double[Degree + 1];
                        for (int i = 0; i < Degree + 1; i++)
                            vSplineValues[i] = Splines.CalculateNSplineValues(knots, i + 1, Degree, v);
                        var point = Vector4.Zero();
                        for (int i = 0; i < Degree + 1; i++)
                            for (int j = 0; j < Degree + 1; j++)
                                point = point + currentPoints[i, j] * vSplineValues[i] * uSplineValues[j];
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
                for (double v = 2 * knotsDivision; v < 3 * knotsDivision + vStep / 2; v += vStep)
                {
                    bool first = true;
                    var lastPoint = Vector4.Zero();
                    const double uDiv = 0.01;
                    var vSplineValues = new double[Degree + 1];
                    for (int i = 0; i < Degree + 1; i++)
                        vSplineValues[i] = Splines.CalculateNSplineValues(knots, i + 1, Degree, v);
                    for (double u = 2 * knotsDivision; u < 3 * knotsDivision + uDiv / 2; u += uDiv)
                    {
                        var uSplineValues = new double[Degree + 1];
                        for (int i = 0; i < Degree + 1; i++)
                            uSplineValues[i] = Splines.CalculateNSplineValues(knots, i + 1, Degree, u);
                        var point = Vector4.Zero();
                        for (int i = 0; i < Degree + 1; i++)
                            for (int j = 0; j < Degree + 1; j++)
                                point = point + currentPoints[i, j]*vSplineValues[i]*uSplineValues[j];
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
    }
}
