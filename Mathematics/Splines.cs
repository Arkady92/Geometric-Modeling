using System;

namespace Mathematics
{
    public static class Splines
    {
        public static double CalculateZeroSplineValues(double[] knots, int i, double t)
        {
            var result = 0.0;
            if (i <= 0 || i >= knots.Length) return result;
            if (knots[i - 1] <= t && knots[i] > t)
                result = 1;
            return result;
        }

        public static double CalculateNSplineValues(double[] knots, int i, int n, double t)
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


        //public static Matrix GenerateInterpolationSplineLinearCostMatrix(double[] knots, int degree, double[] taus)
        //{
        //    var bandSize = degree + 1;
        //    var matrix = new Matrix(taus.Length, bandSize);
        //    for (int i = 0; i < taus.Length; i++)
        //        for (int j = 0; j < bandSize; j++)
        //        {
        //            var k = i + j - 1;
        //            if (k < 0 || k >= taus.Length) continue;
        //            matrix[i, j] = CalculateNSplineValues(knots, k + 1, degree, taus[i]);
        //        }
        //    return matrix;
        //}

        //public static List<Vector4> SolveSystemOfEquationsInLinearCost(double[] knots, int degree, double[] taus, List<Vector4> points)
        //{
        //    var result = new List<Vector4>();

        //    return result;
        //}
    }
}
