using System;

namespace Mathematics
{
    public class Vector4
    {
        public double X
        {
            get { return PointsArray[0]; }
            set { PointsArray[0] = value; }
        }

        public double Y
        {
            get { return PointsArray[1]; }
            set { PointsArray[1] = value; }
        }

        public double Z
        {
            get { return PointsArray[2]; }
            set { PointsArray[2] = value; }
        }

        public double W
        {
            get { return PointsArray[3]; }
            set { PointsArray[3] = value; }
        }

        public double[] PointsArray;

        public void NormalizeSecond()
        {
            var norm = Math.Sqrt(X * X + Y * Y + Z * Z);
            if(Math.Abs(norm) < Double.Epsilon) return;
            X /= norm;
            Z /= norm;
            Y /= norm;
        }

        public void NormalizeW()
        {
            for (int i = 0; i < 4; i++)
            {
                PointsArray[i] /= W;
            }
        }

        public Vector4()
        {
            PointsArray = new double[4];
        }

        public Vector4(double x, double y, double z, double w = 1)
        {
            PointsArray = new double[4];
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static Vector4 operator *(Matrix matrix, Vector4 vector)
        {
            var result = new Vector4();
            for (int i = 0; i < 4; i++)
            {
                result.X += matrix[0, i] * vector.PointsArray[i];
                result.Y += matrix[1, i] * vector.PointsArray[i];
                result.Z += matrix[2, i] * vector.PointsArray[i];
                result.W += matrix[3, i] * vector.PointsArray[i];
            }
            /*for (int i = 0; i < 4; i++)
            {
                result.PointsArray[i] /= result.W;
            }*/
            return result;
        }

        public static Vector4 operator *(Vector4 vector, Matrix matrix)
        {
            var result = new Vector4();
            for (int i = 0; i < 4; i++)
            {
                result.X += matrix[i, 0] * vector.PointsArray[i];
                result.Y += matrix[i, 1] * vector.PointsArray[i];
                result.Z += matrix[i, 2] * vector.PointsArray[i];
                result.W += matrix[i, 3] * vector.PointsArray[i];
            }
            /*for (int i = 0; i < 4; i++)
            {
                result.PointsArray[i] /= result.W;
            }*/
            return result;
        }

        public static double operator *(Vector4 vector, Vector4 vector2)
        {
            double result = 0;
            for (int i = 0; i < 4; i++)
                result += vector.PointsArray[i] * vector2.PointsArray[i];
            return result;
        }

        public static Vector4 operator *(Vector4 vector, double value)
        {
            return new Vector4(vector.X * value, vector.Y * value, vector.Z * value, vector.W * value);
        }

        public void NormalizeFirst()
        {
            var sum = Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
            if(Math.Abs(sum) < Double.Epsilon) return;
            X /= sum;
            Z /= sum;
            Y /= sum;
        }
    }
}
