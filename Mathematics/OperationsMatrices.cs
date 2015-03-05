﻿using System;

namespace Mathematics
{
    public static class OperationsMatrices
    {
        public static Matrix Identity()
        {
            return Matrix.IdentityMatrix(4, 4);
        }

        public static Matrix RotationX(double alpha)
        {
            var matrix = new Matrix(4, 4);
            matrix[0, 0] = 1;
            matrix[1, 1] = Math.Cos(alpha);
            matrix[1, 2] = -Math.Sin(alpha);
            matrix[2, 1] = Math.Sin(alpha);
            matrix[2, 2] = Math.Cos(alpha);
            matrix[3, 3] = 1;
            return matrix;
        }

        public static Matrix RotationY(double alpha)
        {
            var matrix = new Matrix(4, 4);
            matrix[0, 0] = Math.Cos(alpha);
            matrix[0, 2] = Math.Sin(alpha);
            matrix[1, 1] = 1;
            matrix[2, 0] = -Math.Sin(alpha);
            matrix[2, 2] = Math.Cos(alpha);
            matrix[3, 3] = 1;
            return matrix;
        }

        public static Matrix RotationZ(double alpha)
        {
            var matrix = new Matrix(4, 4);
            matrix[0, 0] = Math.Cos(alpha);
            matrix[0, 1] = -Math.Sin(alpha);
            matrix[1, 0] = Math.Sin(alpha);
            matrix[1, 1] = Math.Cos(alpha);
            matrix[2, 2] = 1;
            matrix[3, 3] = 1;
            return matrix;
        }

        public static Matrix Translation(double x, double y, double z)
        {
            var matrix = new Matrix(4, 4);
            matrix[0, 0] = 1;
            matrix[0, 3] = x;
            matrix[1, 1] = 1;
            matrix[1, 3] = y;
            matrix[2, 2] = 1;
            matrix[2, 3] = z;
            matrix[3, 3] = 1;
            return matrix;   
        }

        public static Matrix Scale(double s)
        {
            var matrix = new Matrix(4, 4);
            matrix[0, 0] = s;
            matrix[1, 1] = s;
            matrix[2, 2] = s;
            matrix[3, 3] = 1;
            return matrix;
        }

        public static Matrix Projection(double r)
        {
            var matrix = new Matrix(4, 4);
            matrix[0, 0] = 1;
            matrix[1, 1] = 1;
            matrix[3, 2] = 1 / r;
            matrix[3, 3] = 1;
            return matrix;
        }

        public static Matrix Projection(double f, double n)
        {
            var matrix = new Matrix(4, 4);
            matrix[0, 0] = 1;
            matrix[1, 1] = 1;
            matrix[2, 2] = -(f + n)/(f - n);
            matrix[2, 3] = -2*f*n/(f - n);
            matrix[3, 2] = -1;
            return matrix;
        }
    }
}