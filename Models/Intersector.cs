using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Mathematics;
using MathNet.Numerics.LinearAlgebra.Complex;
using Matrix = Mathematics.Matrix;

namespace Models
{
    public class Intersector
    {
        private readonly BezierSurface _baseSurface;
        private readonly BezierSurface _intersectSurface;
        private const double H = 0.01;
        private const double InvH = 1.0 / H;
        private const int MaxIterationsCount = 1000;
        private const double NewtonEpsilon = 0.01;
        private const double DistanceEpsilon = 0.01;
        private Random _random;

        public Intersector(BezierSurface baseSurface, BezierSurface intersectSurface)
        {
            _baseSurface = baseSurface;
            _intersectSurface = intersectSurface;
            _random = new Random(DateTime.Now.Millisecond);
        }

        public IntersectionCurve PerformIntersection(double x, double y)
        {
            var rightResult = new List<Vector4>();
            var uvRightResult = new List<Vector4>();
            var stRightResult = new List<Vector4>();

            var startApproxPoint = new Vector4(x, y, 0);
            Vector4 uvPrevPoint, stPrevPoint;
            Vector4 uvNewPoint, stNewPoint;
            if (!FindStartPoint(startApproxPoint, out uvPrevPoint, out stPrevPoint))
                return null;
            if (!FindNextPoint(uvPrevPoint, stPrevPoint, out uvNewPoint, out stNewPoint))
                return null;
            uvRightResult.Add(uvNewPoint);
            stRightResult.Add(stNewPoint);
            rightResult.Add(_baseSurface.GetSurfacePoint(uvNewPoint.X, uvNewPoint.Y, 0));

            var last = rightResult.Last();
            var stopped = false;
            for (int i = 0; i < MaxIterationsCount / 20; i++)
            {
                var randomVector = new Vector4(_random.NextDouble() - 0.5, _random.NextDouble() - 0.5, _random.NextDouble() - 0.5, _random.NextDouble() - 0.5);
                randomVector.NormalizeForIntersection();
                startApproxPoint = last + randomVector;
                if (FindStartPoint(startApproxPoint, out uvPrevPoint, out stPrevPoint))
                {
                    if (FindNextPoint(uvPrevPoint, stPrevPoint, out uvNewPoint, out stNewPoint))
                    {
                        var newPoint = _baseSurface.GetSurfacePoint(uvNewPoint.X, uvNewPoint.Y, 0);
                        if (Vector4.Distance3(newPoint, last) > Parameters.IntersectionAccuracy / 2)
                        {
                            uvRightResult.Add(uvNewPoint);
                            stRightResult.Add(stNewPoint);
                            rightResult.Add(newPoint);
                            stopped = true;
                            break;
                        }
                    }
                }
            }
            if (!stopped)
                return null;
            double minDistance = Vector4.Distance3(rightResult[1], rightResult[0]) / 2.0;
            for (int i = 0; i < MaxIterationsCount; i++)
            {
                startApproxPoint = rightResult[rightResult.Count - 1] * 2 - rightResult[rightResult.Count - 2];
                if (FindStartPoint(startApproxPoint, out uvPrevPoint, out stPrevPoint)
                    && FindNextPoint(uvPrevPoint, stPrevPoint, out uvNewPoint, out stNewPoint))
                {
                    var newPoint = _baseSurface.GetSurfacePoint(uvNewPoint.X, uvNewPoint.Y, 0);
                    if (Vector4.Distance3(newPoint, rightResult[rightResult.Count - 1]) > minDistance
                        && Vector4.Distance3(newPoint, rightResult[0]) > minDistance)
                    {
                        uvRightResult.Add(uvNewPoint);
                        stRightResult.Add(stNewPoint);
                        rightResult.Add(newPoint);
                        continue;
                    }
                }
                break;
            }

            var leftResult = new List<Vector4>();
            var uvLeftResult = new List<Vector4>();
            var stLeftResult = new List<Vector4>();

            leftResult.Add(rightResult[1]);
            leftResult.Add(rightResult[0]);
            for (int i = 0; i < MaxIterationsCount; i++)
            {
                startApproxPoint = leftResult[leftResult.Count - 1] * 2 - leftResult[leftResult.Count - 2];
                if (FindStartPoint(startApproxPoint, out uvPrevPoint, out stPrevPoint)
                    && FindNextPoint(uvPrevPoint, stPrevPoint, out uvNewPoint, out stNewPoint))
                {
                    var newPoint = _baseSurface.GetSurfacePoint(uvNewPoint.X, uvNewPoint.Y, 0);
                    if (Vector4.Distance3(newPoint, leftResult[leftResult.Count - 1]) > minDistance
                        && Vector4.Distance3(newPoint, leftResult[0]) > minDistance)
                    {
                        uvLeftResult.Add(uvNewPoint);
                        stLeftResult.Add(stNewPoint);
                        leftResult.Add(newPoint);
                        continue;
                    }
                }
                break;
            }

            var result = new List<Vector4>();
            var uvResult = new List<Vector4>();
            var stResult = new List<Vector4>();
            for (int i = leftResult.Count - 1; i >= 2; i--)
            {
                result.Add(leftResult[i]);
                uvResult.Add(uvLeftResult[i - 2]);
                stResult.Add(stLeftResult[i - 2]);
            }
            result.AddRange(rightResult);
            uvResult.AddRange(uvRightResult);
            stResult.AddRange(stRightResult);
            return new IntersectionCurve(result, uvResult, stResult, new Vector4(0, 0, 0)) { Color = Color.GreenYellow };
        }

        private bool FindNextStartPoint(Vector4 uvPoint, Vector4 stPoint, Vector4 point, out Vector4 uvNewPoint, out Vector4 stNewPoint)
        {
            const int finderSize = 100;
            const double finderDif = 0.1;
            const double finderStep = 2 * finderDif / finderSize;
            var uvMinDistance = double.MaxValue;
            var stMinDistance = double.MaxValue;
            var u = 0.5;
            var v = 0.5;
            var s = 0.5;
            var t = 0.5;
            for (double i = uvPoint.X - finderDif; i < uvPoint.X + finderDif; i += finderStep)
            {
                for (double j = uvPoint.Y - finderDif; j < uvPoint.Y + finderDif; j += finderStep)
                {
                    var distance = Vector4.Distance3(_baseSurface.GetSurfacePoint(i * finderStep, j * finderStep, 0), point);
                    if (distance < uvMinDistance)
                    {
                        uvMinDistance = distance;
                        u = i * finderStep;
                        v = j * finderStep;
                    }
                }
            }
            for (double i = stPoint.X - finderDif; i < stPoint.X + finderDif; i += finderStep)
            {
                for (double j = stPoint.Y - finderDif; j < stPoint.Y + finderDif; j += finderStep)
                {
                    var distance = Vector4.Distance3(_intersectSurface.GetSurfacePoint(i * finderStep, j * finderStep, 0), point);
                    if (distance < stMinDistance)
                    {
                        stMinDistance = distance;
                        s = i * finderStep;
                        t = j * finderStep;
                    }
                }
            }
            uvNewPoint = new Vector4(u, v, 0, 0);
            stNewPoint = new Vector4(s, t, 0, 0);
            return true;
        }

        private bool FindStartPoint(Vector4 point, out Vector4 uvNewPoint, out Vector4 stNewPoint)
        {
            const int finderSize = 100;
            const double finderStep = 1.0 / finderSize;
            var uvMinDistance = 1.0;
            var stMinDistance = 1.0;
            var u = 0.5;
            var v = 0.5;
            var s = 0.5;
            var t = 0.5;
            for (int i = 0; i < finderSize; i++)
            {
                for (int j = 0; j < finderSize; j++)
                {
                    var distance = Vector4.Distance3(_baseSurface.GetSurfacePoint(i * finderStep, j * finderStep, 0), point);
                    if (distance < uvMinDistance)
                    {
                        uvMinDistance = distance;
                        u = i * finderStep;
                        v = j * finderStep;
                    }
                    distance = Vector4.Distance3(_intersectSurface.GetSurfacePoint(i * finderStep, j * finderStep, 0), point);
                    if (distance < stMinDistance)
                    {
                        stMinDistance = distance;
                        s = i * finderStep;
                        t = j * finderStep;
                    }
                }
            }

            uvNewPoint = new Vector4(u, v, 0, 0);
            stNewPoint = new Vector4(s, t, 0, 0);
            return true;
            //Func<double, double, Vector4> uvFunc = (uu, vv) => _baseSurface.GetSurfacePoint(uu, vv, 0) - point;
            //Func<double, double, Vector4> stFunc = (ss, tt) => _intersectSurface.GetSurfacePoint(ss, tt, 0) - point;

            //for (int it = 0; it < MaxIterationsCount; it++)
            //{
            //    var uvDerivatives = new Vector4[2];
            //    var stDerivatives = new Vector4[2];
            //    uvDerivatives[0] = (uvFunc(u + H, v) - uvFunc(u - H, v)) * 0.5 * InvH;
            //    uvDerivatives[1] = (uvFunc(u, v + H) - uvFunc(u, v - H)) * 0.5 * InvH;
            //    stDerivatives[0] = (stFunc(s + H, t) - stFunc(s - H, t)) * 0.5 * InvH;
            //    stDerivatives[1] = (stFunc(s, t + H) - stFunc(s, t - H)) * 0.5 * InvH;

            //    var uvJacobian = new Matrix(2, 2);
            //    var stJacobian = new Matrix(2, 2);
            //    for (int i = 0; i < 2; i++)
            //    {
            //        for (int j = 0; j < 2; j++)
            //        {
            //            uvJacobian[i, j] = uvDerivatives[j].PointsArray[i];
            //            stJacobian[i, j] = stDerivatives[j].PointsArray[i];
            //        }
            //    }
            //    uvJacobian = uvJacobian.Invert();
            //    stJacobian = stJacobian.Invert();

            //    var uvFuncResult = uvFunc(u, v);
            //    var stFuncResult = stFunc(s, t);

            //    var uvShiftX = uvJacobian[0, 0] * uvFuncResult.X + uvJacobian[0, 1] * uvFuncResult.Y;
            //    var uvShiftY = uvJacobian[1, 0] * uvFuncResult.X + uvJacobian[1, 1] * uvFuncResult.Y;
            //    var stShiftX = stJacobian[0, 0] * stFuncResult.X + stJacobian[0, 1] * stFuncResult.Y;
            //    var stShiftY = stJacobian[1, 0] * stFuncResult.X + stJacobian[1, 1] * stFuncResult.Y;
            //    uvNewPoint = new Vector4(u - uvShiftX, v - uvShiftY, 0);
            //    stNewPoint = new Vector4(s - stShiftX, t - stShiftY, 0);
            //    u = uvNewPoint.X;
            //    v = uvNewPoint.Y;
            //    s = stNewPoint.X;
            //    t = stNewPoint.Y;
            //    if (_intersectSurface.IsCylindrical)
            //    {
            //        if (s < 0) s = 0;
            //        if (t < 0) t = 0;
            //        if (s > 1) s = 1;
            //        if (t > 1) t = 1;
            //    }
            //    if (u < 0 || u > 1 || v < 0 || v > 1 || s < 0 || s > 1 || t < 0 || t > 1)
            //        return false;
            //    if (Math.Abs(uvShiftX) + Math.Abs(uvShiftY) + Math.Abs(stShiftX) + Math.Abs(stShiftY) < NewtonEpsilon)
            //        return true;
            //}
            //uvNewPoint = null;
            //stNewPoint = null;
            //return false;
        }

        private bool FindNextPoint(Vector4 uvPoint, Vector4 stPoint, out Vector4 uvNewPoint, out Vector4 stNewPoint)
        {
            var u = uvPoint.X;
            var v = uvPoint.Y;
            var s = stPoint.X;
            var t = stPoint.Y;
            Func<double, double, double, double, Vector4> func = (uu, vv, ss, tt) =>
                _baseSurface.GetSurfacePoint(uu, vv, 0) - _intersectSurface.GetSurfacePoint(ss, tt, 0);
            Func<double, double, Vector4> funcBase = (uu, vv) => _baseSurface.GetSurfacePoint(uu, vv, 0);
            Func<double, double, Vector4> funcIntersect = (ss, tt) => _intersectSurface.GetSurfacePoint(ss, tt, 0);

            for (int it = 0; it < MaxIterationsCount; it++)
            {
                var derivatives = new Vector4[4];
                derivatives[0] = (funcBase(u + H, v) - funcBase(u - H, v)) * 0.5 * InvH;
                derivatives[1] = (funcBase(u, v + H) - funcBase(u, v - H)) * 0.5 * InvH;
                derivatives[2] = (funcIntersect(s + H, t) - funcIntersect(s - H, t)) * -0.5 * InvH;
                derivatives[3] = (funcIntersect(s, t + H) - funcIntersect(s, t - H)) * -0.5 * InvH;
                //var jacobian = new Matrix(4, 4);
                //for (int i = 0; i < 4; i++)
                //    for (int j = 0; j < 4; j++)
                //        jacobian[i, j] = derivatives[i].PointsArray[j];
                //jacobian[0, 3] = jacobian[1, 3] = jacobian[2, 3] = 0;
                //jacobian[3, 3] = 1;
                //try
                //{
                //    jacobian = jacobian.Invert();
                //}
                //catch (MException e)
                //{
                //    uvNewPoint = null;
                //    stNewPoint = null;
                //    return false;
                //}

                //var shift = jacobian * func(u, v, s, t);

                var jacobian = new Matrix(4, 4);
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 4; j++)
                        jacobian[i, j] = derivatives[j].PointsArray[i];
                jacobian[3, 0] = jacobian[3, 1] = jacobian[3, 2] = 0;
                jacobian[3, 3] = 1;
                //jacobian = Matrix.Transpose(jacobian) * (jacobian * Matrix.Transpose(jacobian)).Invert();
                var res = func(u, v, s, t);
                var mat = new Matrix(4, 1);
                mat[0, 0] = res.X;
                mat[1, 0] = res.Y;
                mat[2, 0] = res.Z;
                mat[3, 0] = 0;
                var shiftv = jacobian.SolveWith(mat);//jacobian * mat;
                var shift = new Vector4(shiftv[0, 0], shiftv[1, 0], shiftv[2, 0], shiftv[3, 0]);
                uvNewPoint = new Vector4(u - shift.X, v - shift.Y, 0);
                stNewPoint = new Vector4(s - shift.Z, t - shift.W, 0);
                u = uvNewPoint.X;
                v = uvNewPoint.Y;
                s = stNewPoint.X;
                t = stNewPoint.Y;
                if (u < 0 || u > 1 || v < 0 || v > 1 || s < 0 || s > 1 || t < 0 || t > 1)
                    return false;
                if (Math.Abs(shift.X) + Math.Abs(shift.Y) + Math.Abs(shift.Z) + Math.Abs(shift.W) < NewtonEpsilon)
                    return true;

            }
            uvNewPoint = null;
            stNewPoint = null;
            return false;
        }
    }
}
