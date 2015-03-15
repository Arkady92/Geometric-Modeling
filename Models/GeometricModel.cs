using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using Mathematics;

namespace Models
{
    public abstract class GeometricModel
    {
        public ModelType Type;

        protected Matrix CurrentOperationsMatrix;

        protected double TranslationFactor;

        protected double RotationFactor;

        protected double ScaleFactor;
        
        protected double MaximumScaleFactor;

        private double _actualScale;

        public void Translate(double x, double y, double z)
        {
            CurrentOperationsMatrix = OperationsMatrices.Translation(x * TranslationFactor, y * TranslationFactor,
                z * TranslationFactor) * CurrentOperationsMatrix;
        }

        public void Rotate(double x, double y, double z)
        {
            if (Math.Abs(x) > Double.Epsilon)
                CurrentOperationsMatrix = OperationsMatrices.RotationX(x * RotationFactor) * CurrentOperationsMatrix;
            if (Math.Abs(y) > Double.Epsilon)
                CurrentOperationsMatrix = OperationsMatrices.RotationY(y * RotationFactor) * CurrentOperationsMatrix;
            if (Math.Abs(z) > Double.Epsilon)
                CurrentOperationsMatrix = OperationsMatrices.RotationZ(z * RotationFactor) * CurrentOperationsMatrix;
        }

        public void Scale(double s)
        {
            if (s > 0 && _actualScale > 1 / MaximumScaleFactor)
            {
                CurrentOperationsMatrix = OperationsMatrices.Scale(1 / ScaleFactor)*CurrentOperationsMatrix;
                _actualScale /= ScaleFactor;
            }
            if (s < 0 && _actualScale < MaximumScaleFactor)
            {
                CurrentOperationsMatrix = OperationsMatrices.Scale(ScaleFactor) * CurrentOperationsMatrix;
                _actualScale *= ScaleFactor;
            }
        }

        protected GeometricModel(ModelType type)
        {
            Type = type;
            CurrentOperationsMatrix = OperationsMatrices.Identity();
            _actualScale = 1;
        }

        public abstract void Draw(Graphics graphics, Matrix currentProjectionMatrix = null);

        public abstract void UpdateModel();
    }
}
