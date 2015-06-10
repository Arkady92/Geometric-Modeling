using System;
using System.Drawing;
using Mathematics;
using System.Collections.Generic;

namespace Models
{
    public class GapFiller : ParametricGeometricModel
    {
        private readonly List<BezierSurface> _surfaces;
        private List<GregorySurface> _gregorySurfaces;

        private Matrix _interpolationMatrix;

        public GapFiller(List<BezierSurface> surfaces)
            : base(ModelType.GapFiller, Vector4.Zero())
        {
            _interpolationMatrix = OperationsMatrices.InterpolationBezier();
            _surfaces = surfaces;
            foreach (var bezierSurface in surfaces)
            {
                bezierSurface.AddGapFiller(this);
            }
            _gregorySurfaces = new List<GregorySurface>();
            FillGap();
        }

        public void FillGap()
        {
            var borderPoints = new Vector4[3, 4];
            var cornerPoints = new Vector4[3];
            var borderMiddlePoints = new List<Vector4[]>();
            var internalMiddlePoints = new List<Vector4[]>();
            var qpoints = new Vector4[3];
            var results = new Vector4[4];
            const double oneThird = 1.0 / 3.0;
            const double twoThird = 2.0 / 3.0;
            for (int i = 0; i < 3; i++)
            {
                var type = _surfaces[i].FindBorderType();
                var prevHalfPosPoint = Vector4.Zero();
                var halfPosPoint = Vector4.Zero();
                Vector4[] interpolationPoints;
                Vector4[] prevInterpolationPoints;
                Vector4[] nextInterpolationPoints;
                switch (type)
                {
                    case BorderType.Bottom:
                        cornerPoints[i] = _surfaces[i].GetSurfacePoint(0.0, 0.0);
                        results[0] = _surfaces[i].GetSurfacePoint(0.0, 0.5);
                        results[1] = _surfaces[i].GetSurfacePoint(1.0 / 6.0, 0.5);
                        results[2] = _surfaces[i].GetSurfacePoint(2.0 / 6.0, 0.5);
                        results[3] = _surfaces[i].GetSurfacePoint(0.5, 0.5);
                        interpolationPoints = InterpolatePoints(results);
                        prevHalfPosPoint = interpolationPoints[1];
                        halfPosPoint = interpolationPoints[0];

                        results[0] = _surfaces[i].GetSurfacePoint(0.0, 0.0);
                        results[1] = _surfaces[i].GetSurfacePoint(0.0, 1.0 / 6.0);
                        results[2] = _surfaces[i].GetSurfacePoint(0.0, 2.0 / 6.0);
                        results[3] = _surfaces[i].GetSurfacePoint(0.0, 0.5);
                        interpolationPoints = InterpolatePoints(results);
                        nextInterpolationPoints = new Vector4[4];
                        for (int j = 0; j < 4; j++)
                            nextInterpolationPoints[3 - j] = interpolationPoints[j];
                        borderMiddlePoints.Add(nextInterpolationPoints);

                        results[0] = _surfaces[i].GetSurfacePoint(1.0 / 6.0, 0.0);
                        results[1] = _surfaces[i].GetSurfacePoint(1.0 / 6.0, 1.0 / 6.0);
                        results[2] = _surfaces[i].GetSurfacePoint(1.0 / 6.0, 2.0 / 6.0);
                        results[3] = _surfaces[i].GetSurfacePoint(1.0 / 6.0, 0.5);
                        prevInterpolationPoints = InterpolatePoints(results);
                        nextInterpolationPoints = new Vector4[4];
                        for (int j = 0; j < 4; j++)
                            nextInterpolationPoints[3 - j] = interpolationPoints[j] * 2 - prevInterpolationPoints[j];
                        internalMiddlePoints.Add(nextInterpolationPoints);

                        results[0] = _surfaces[i].GetSurfacePoint(0.0, 0.5);
                        results[1] = _surfaces[i].GetSurfacePoint(0.0, 4.0 / 6.0);
                        results[2] = _surfaces[i].GetSurfacePoint(0.0, 5.0 / 6.0);
                        results[3] = _surfaces[i].GetSurfacePoint(0.0, 1.0);
                        interpolationPoints = InterpolatePoints(results);
                        nextInterpolationPoints = new Vector4[4];
                        for (int j = 0; j < 4; j++)
                            nextInterpolationPoints[3 - j] = interpolationPoints[j];
                        borderMiddlePoints.Add(nextInterpolationPoints);

                        results[0] = _surfaces[i].GetSurfacePoint(1.0 / 6.0, 0.5);
                        results[1] = _surfaces[i].GetSurfacePoint(1.0 / 6.0, 4.0 / 6.0);
                        results[2] = _surfaces[i].GetSurfacePoint(1.0 / 6.0, 5.0 / 6.0);
                        results[3] = _surfaces[i].GetSurfacePoint(1.0 / 6.0, 1.0);
                        prevInterpolationPoints = InterpolatePoints(results);
                        nextInterpolationPoints = new Vector4[4];
                        for (int j = 0; j < 4; j++)
                            nextInterpolationPoints[3 - j] = interpolationPoints[j] * 2 - prevInterpolationPoints[j];
                        internalMiddlePoints.Add(nextInterpolationPoints);
                        break;
                    case BorderType.Top:
                        cornerPoints[i] = _surfaces[i].GetSurfacePoint(1.0, 1.0);
                        results[0] = _surfaces[i].GetSurfacePoint(0.5, 0.5);
                        results[1] = _surfaces[i].GetSurfacePoint(4.0 / 6.0, 0.5);
                        results[2] = _surfaces[i].GetSurfacePoint(5.0 / 6.0, 0.5);
                        results[3] = _surfaces[i].GetSurfacePoint(1.0, 0.5);
                        interpolationPoints = InterpolatePoints(results);
                        prevHalfPosPoint = interpolationPoints[2];
                        halfPosPoint = interpolationPoints[3];

                        results[0] = _surfaces[i].GetSurfacePoint(1.0, 0.5);
                        results[1] = _surfaces[i].GetSurfacePoint(1.0, 4.0 / 6.0);
                        results[2] = _surfaces[i].GetSurfacePoint(1.0, 5.0 / 6.0);
                        results[3] = _surfaces[i].GetSurfacePoint(1.0, 1.0);
                        interpolationPoints = InterpolatePoints(results);
                        borderMiddlePoints.Add(interpolationPoints);

                        results[0] = _surfaces[i].GetSurfacePoint(5.0 / 6.0, 0.5);
                        results[1] = _surfaces[i].GetSurfacePoint(5.0 / 6.0, 4.0 / 6.0);
                        results[2] = _surfaces[i].GetSurfacePoint(5.0 / 6.0, 5.0 / 6.0);
                        results[3] = _surfaces[i].GetSurfacePoint(5.0 / 6.0, 1.0);
                        prevInterpolationPoints = InterpolatePoints(results);
                        nextInterpolationPoints = new Vector4[4];
                        for (int j = 0; j < 4; j++)
                            nextInterpolationPoints[j] = interpolationPoints[j] * 2 - prevInterpolationPoints[j];
                        internalMiddlePoints.Add(nextInterpolationPoints);

                        results[0] = _surfaces[i].GetSurfacePoint(1.0, 0.0);
                        results[1] = _surfaces[i].GetSurfacePoint(1.0, 1.0 / 6.0);
                        results[2] = _surfaces[i].GetSurfacePoint(1.0, 2.0 / 6.0);
                        results[3] = _surfaces[i].GetSurfacePoint(1.0, 0.5);
                        interpolationPoints = InterpolatePoints(results);
                        borderMiddlePoints.Add(interpolationPoints);

                        results[0] = _surfaces[i].GetSurfacePoint(5.0 / 6.0, 0.0);
                        results[1] = _surfaces[i].GetSurfacePoint(5.0 / 6.0, 1.0 / 6.0);
                        results[2] = _surfaces[i].GetSurfacePoint(5.0 / 6.0, 2.0 / 6.0);
                        results[3] = _surfaces[i].GetSurfacePoint(5.0 / 6.0, 0.5);
                        prevInterpolationPoints = InterpolatePoints(results);
                        nextInterpolationPoints = new Vector4[4];
                        for (int j = 0; j < 4; j++)
                            nextInterpolationPoints[j] = interpolationPoints[j] * 2 - prevInterpolationPoints[j];
                        internalMiddlePoints.Add(nextInterpolationPoints);

                        break;
                    case BorderType.Left:
                        cornerPoints[i] = _surfaces[i].GetSurfacePoint(1.0, 0.0);
                        results[0] = _surfaces[i].GetSurfacePoint(0.5, 0.0);
                        results[1] = _surfaces[i].GetSurfacePoint(0.5, 1.0 / 6.0);
                        results[2] = _surfaces[i].GetSurfacePoint(0.5, 2.0 / 6.0);
                        results[3] = _surfaces[i].GetSurfacePoint(0.5, 0.5);
                        interpolationPoints = InterpolatePoints(results);
                        prevHalfPosPoint = interpolationPoints[1];
                        halfPosPoint = interpolationPoints[0];

                        results[0] = _surfaces[i].GetSurfacePoint(0.5, 0.0);
                        results[1] = _surfaces[i].GetSurfacePoint(4.0 / 6.0, 0.0);
                        results[2] = _surfaces[i].GetSurfacePoint(5.0 / 6.0, 0.0);
                        results[3] = _surfaces[i].GetSurfacePoint(1.0, 0.0);
                        interpolationPoints = InterpolatePoints(results);
                        borderMiddlePoints.Add(interpolationPoints);

                        results[0] = _surfaces[i].GetSurfacePoint(0.5, 1.0 / 6.0);
                        results[1] = _surfaces[i].GetSurfacePoint(4.0 / 6.0, 1.0 / 6.0);
                        results[2] = _surfaces[i].GetSurfacePoint(5.0 / 6.0, 1.0 / 6.0);
                        results[3] = _surfaces[i].GetSurfacePoint(1.0, 1.0 / 6.0);
                        prevInterpolationPoints = InterpolatePoints(results);
                        nextInterpolationPoints = new Vector4[4];
                        for (int j = 0; j < 4; j++)
                            nextInterpolationPoints[j] = interpolationPoints[j] * 2 - prevInterpolationPoints[j];
                        internalMiddlePoints.Add(nextInterpolationPoints);

                        results[0] = _surfaces[i].GetSurfacePoint(0.0, 0.0);
                        results[1] = _surfaces[i].GetSurfacePoint(1.0 / 6.0, 0.0);
                        results[2] = _surfaces[i].GetSurfacePoint(2.0 / 6.0, 0.0);
                        results[3] = _surfaces[i].GetSurfacePoint(0.5, 0.0);
                        interpolationPoints = InterpolatePoints(results);
                        borderMiddlePoints.Add(interpolationPoints);

                        results[0] = _surfaces[i].GetSurfacePoint(0.0, 1.0 / 6.0);
                        results[1] = _surfaces[i].GetSurfacePoint(1.0 / 6.0, 1.0 / 6.0);
                        results[2] = _surfaces[i].GetSurfacePoint(2.0 / 6.0, 1.0 / 6.0);
                        results[3] = _surfaces[i].GetSurfacePoint(0.5, 1.0 / 6.0);
                        prevInterpolationPoints = InterpolatePoints(results);
                        nextInterpolationPoints = new Vector4[4];
                        for (int j = 0; j < 4; j++)
                            nextInterpolationPoints[j] = interpolationPoints[j] * 2 - prevInterpolationPoints[j];
                        internalMiddlePoints.Add(nextInterpolationPoints);
                        break;
                    case BorderType.Right:
                        cornerPoints[i] = _surfaces[i].GetSurfacePoint(0.0, 1.0);
                        results[0] = _surfaces[i].GetSurfacePoint(0.5, 0.5);
                        results[1] = _surfaces[i].GetSurfacePoint(0.5, 4.0 / 6.0);
                        results[2] = _surfaces[i].GetSurfacePoint(0.5, 5.0 / 6.0);
                        results[3] = _surfaces[i].GetSurfacePoint(0.5, 1.0);
                        interpolationPoints = InterpolatePoints(results);
                        prevHalfPosPoint = interpolationPoints[2];
                        halfPosPoint = interpolationPoints[3];

                        results[0] = _surfaces[i].GetSurfacePoint(0.0, 1.0);
                        results[1] = _surfaces[i].GetSurfacePoint(1.0 / 6.0, 1.0);
                        results[2] = _surfaces[i].GetSurfacePoint(2.0 / 6.0, 1.0);
                        results[3] = _surfaces[i].GetSurfacePoint(0.5, 1.0);
                        interpolationPoints = InterpolatePoints(results);
                        nextInterpolationPoints = new Vector4[4];
                        for (int j = 0; j < 4; j++)
                            nextInterpolationPoints[3 - j] = interpolationPoints[j];
                        borderMiddlePoints.Add(nextInterpolationPoints);

                        results[0] = _surfaces[i].GetSurfacePoint(0.0, 5.0 / 6.0);
                        results[1] = _surfaces[i].GetSurfacePoint(1.0 / 6.0, 5.0 / 6.0);
                        results[2] = _surfaces[i].GetSurfacePoint(2.0 / 6.0, 5.0 / 6.0);
                        results[3] = _surfaces[i].GetSurfacePoint(0.5, 5.0 / 6.0);
                        prevInterpolationPoints = InterpolatePoints(results);
                        nextInterpolationPoints = new Vector4[4];
                        for (int j = 0; j < 4; j++)
                            nextInterpolationPoints[3 - j] = interpolationPoints[j] * 2 - prevInterpolationPoints[j];
                        internalMiddlePoints.Add(nextInterpolationPoints);

                        results[0] = _surfaces[i].GetSurfacePoint(0.5, 1.0);
                        results[1] = _surfaces[i].GetSurfacePoint(4.0 / 6.0, 1.0);
                        results[2] = _surfaces[i].GetSurfacePoint(5.0 / 6.0, 1.0);
                        results[3] = _surfaces[i].GetSurfacePoint(1.0, 1.0);
                        interpolationPoints = InterpolatePoints(results);
                        nextInterpolationPoints = new Vector4[4];
                        for (int j = 0; j < 4; j++)
                            nextInterpolationPoints[3 - j] = interpolationPoints[j];
                        borderMiddlePoints.Add(nextInterpolationPoints);

                        results[0] = _surfaces[i].GetSurfacePoint(0.5, 5.0 / 6.0);
                        results[1] = _surfaces[i].GetSurfacePoint(4.0 / 6.0, 5.0 / 6.0);
                        results[2] = _surfaces[i].GetSurfacePoint(5.0 / 6.0, 5.0 / 6.0);
                        results[3] = _surfaces[i].GetSurfacePoint(1.0, 5.0 / 6.0);
                        prevInterpolationPoints = InterpolatePoints(results);
                        nextInterpolationPoints = new Vector4[4];
                        for (int j = 0; j < 4; j++)
                            nextInterpolationPoints[3 - j] = interpolationPoints[j] * 2 - prevInterpolationPoints[j];
                        internalMiddlePoints.Add(nextInterpolationPoints);
                        break;
                }
                borderPoints[i, 3] = halfPosPoint;
                var nextPoint = halfPosPoint * 2 - prevHalfPosPoint;
                borderPoints[i, 2] = nextPoint;
                qpoints[i] = nextPoint * 1.5 - halfPosPoint * 0.5;
            }
            var finalPoint = (qpoints[0] + qpoints[1] + qpoints[2]) * oneThird;
            for (int i = 0; i < 3; i++)
            {
                borderPoints[i, 0] = finalPoint;
                borderPoints[i, 1] = qpoints[i] * twoThird + finalPoint * oneThird;
            }
            _gregorySurfaces.Clear();
            var gregoryPatch = new GregorySurface(new List<Point>
            {
               new Point(cornerPoints[0]),
               new Point(borderMiddlePoints[5][1]),
               new Point(borderMiddlePoints[5][2]),
               new Point(borderPoints[2, 3]),
               new Point(borderPoints[2, 2]),
               new Point(borderPoints[2, 1]),
               new Point(borderPoints[2, 0]),
               new Point(borderPoints[0, 1]),
               new Point(borderPoints[0, 2]),
               new Point(borderPoints[0, 3]),
               new Point(borderMiddlePoints[0][1]),
               new Point(borderMiddlePoints[0][2]),
               new Point(internalMiddlePoints[0][2]),
               new Point(internalMiddlePoints[5][1]),
               new Point(internalMiddlePoints[5][2]),
               new Point(internalMiddlePoints[5][2]),
               new Point(Vector4.OneThird(borderPoints[2, 1], borderMiddlePoints[0][1])),
               new Point(Vector4.OneThird(borderPoints[0, 1], borderMiddlePoints[5][2])),
               new Point(internalMiddlePoints[0][1]),
               new Point(internalMiddlePoints[0][1]),
            }, Vector4.Zero()) { Color = Color.Red };
            _gregorySurfaces.Add(gregoryPatch);
            gregoryPatch = new GregorySurface(new List<Point>
            {
               new Point(cornerPoints[1]),
               new Point(borderMiddlePoints[1][1]),
               new Point(borderMiddlePoints[1][2]),
               new Point(borderPoints[0, 3]),
               new Point(borderPoints[0, 2]),
               new Point(borderPoints[0, 1]),
               new Point(borderPoints[0, 0]),
               new Point(borderPoints[1, 1]),
               new Point(borderPoints[1, 2]),
               new Point(borderPoints[1, 3]),
               new Point(borderMiddlePoints[2][1]),
               new Point(borderMiddlePoints[2][2]),
               new Point(internalMiddlePoints[2][2]),
               new Point(internalMiddlePoints[1][1]),
               new Point(internalMiddlePoints[1][2]),
               new Point(internalMiddlePoints[1][2]),
               new Point(Vector4.OneThird(borderPoints[0, 1], borderMiddlePoints[2][1])),
               new Point(Vector4.OneThird(borderPoints[1, 1], borderMiddlePoints[1][2])),
               new Point(internalMiddlePoints[2][1]),
               new Point(internalMiddlePoints[2][1]),
            }, Vector4.Zero()) { Color = Color.Red };
            _gregorySurfaces.Add(gregoryPatch);
            gregoryPatch = new GregorySurface(new List<Point>
            {
               new Point(cornerPoints[2]),
               new Point(borderMiddlePoints[3][1]),
               new Point(borderMiddlePoints[3][2]),
               new Point(borderPoints[1, 3]),
               new Point(borderPoints[1, 2]),
               new Point(borderPoints[1, 1]),
               new Point(borderPoints[1, 0]),
               new Point(borderPoints[2, 1]),
               new Point(borderPoints[2, 2]),
               new Point(borderPoints[2, 3]),
               new Point(borderMiddlePoints[4][1]),
               new Point(borderMiddlePoints[4][2]),
               new Point(internalMiddlePoints[4][2]),
               new Point(internalMiddlePoints[3][1]),
               new Point(internalMiddlePoints[3][2]),
               new Point(internalMiddlePoints[3][2]),
               new Point(Vector4.OneThird(borderPoints[1, 1], borderMiddlePoints[4][1])),
               new Point(Vector4.OneThird(borderPoints[2, 1], borderMiddlePoints[3][2])),
               new Point(internalMiddlePoints[4][1]),
               new Point(internalMiddlePoints[4][1]),
            }, Vector4.Zero()) { Color = Color.Red };
            _gregorySurfaces.Add(gregoryPatch);
        }

        private Vector4[] InterpolatePoints(Vector4[] points)
        {
            var mPoints = new Matrix(4, 3);
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    mPoints[j, k] = points[j].PointsArray[k];
                }
            }
            var finalResults = _interpolationMatrix * mPoints;
            var finalPoints = new Vector4[4];
            for (int j = 0; j < 4; j++)
                finalPoints[j] = new Vector4(finalResults[j, 0], finalResults[j, 1], finalResults[j, 2]);
            return finalPoints;
        }

        public override void Draw(Graphics graphics, Matrix currentProjectionMatrix)
        {
            foreach (var gregorySurface in _gregorySurfaces)
            {
                gregorySurface.Draw(graphics, currentProjectionMatrix);
            }
        }

        public override void DrawStereoscopy(Graphics graphics, Matrix leftMatrix, Matrix rightMatrix, bool additiveColorBlending = false)
        {
            foreach (var gregorySurface in _gregorySurfaces)
            {
                gregorySurface.DrawStereoscopy(graphics, leftMatrix, rightMatrix, additiveColorBlending);
            }
        }

        protected override void CreateEdges() { }

        protected override void CreateVertices() { }

        public override void UpdateModel() { }

        public void TogglePolygonialChain()
        {
            foreach (var gregorySurface in _gregorySurfaces)
            {
                gregorySurface.TogglePolygonialChain();
            }
        }
    }
}
