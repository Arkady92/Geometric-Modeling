using System.Security.Policy;

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

        public Vector4()
        {
            PointsArray = new double[4];
            Z = 1;
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
                result.X += matrix[i, 0] * vector.PointsArray[i];
                result.Y += matrix[i, 1] * vector.PointsArray[i];
                result.Z += matrix[i, 2] * vector.PointsArray[i];
                result.W += matrix[i, 3] * vector.PointsArray[i];
            }
            result.Normalize();
            return result;
        }

        private void Normalize()
        {
            for (int i = 0; i < 4; i++)
            {
                PointsArray[i] /= W;
            }
        }
    }
}
