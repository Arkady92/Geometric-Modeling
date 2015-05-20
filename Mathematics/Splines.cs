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

        public static double[] GetKnotsDoubledEnds(int count)
        {
            var step = 1.0 / (count - 3);
            var result = new double[count];
            result[0] = 0;
            result[1] = 0;
            for (int i = 2; i < count - 2; i++)
                result[i] = (i - 1)*step;
            result[count - 2] = 1;
            result[count - 1] = 1;
            return result;
        }
    }
}
