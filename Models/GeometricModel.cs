using System;
using System.Drawing;
using Mathematics;

namespace Models
{
    public abstract class GeometricModel
    {
        public ModelType Type;

        public Matrix CurrentOperationMatrix { get; protected set; }

        protected double TranslationFactor;

        protected double RotationFactor;

        protected double ScaleFactor;
        
        protected double MaximumScaleFactor;

        private double _actualScale;

        public void Translate(double x, double y, double z)
        {
            CurrentOperationMatrix = OperationsMatrices.Translation(x * TranslationFactor, y * TranslationFactor,
                z * TranslationFactor) * CurrentOperationMatrix;
        }

        public void Rotate(double x, double y, double z)
        {
            if (Math.Abs(x) > Double.Epsilon)
                CurrentOperationMatrix = OperationsMatrices.RotationX(x * RotationFactor) * CurrentOperationMatrix;
            if (Math.Abs(y) > Double.Epsilon)
                CurrentOperationMatrix = OperationsMatrices.RotationY(y * RotationFactor) * CurrentOperationMatrix;
            if (Math.Abs(z) > Double.Epsilon)
                CurrentOperationMatrix = OperationsMatrices.RotationZ(z * RotationFactor) * CurrentOperationMatrix;
        }

        public void Scale(double s)
        {
            if (s > 0 && _actualScale > 1 / MaximumScaleFactor)
            {
                CurrentOperationMatrix = OperationsMatrices.Scale(1 / ScaleFactor)*CurrentOperationMatrix;
                _actualScale /= ScaleFactor;
            }
            if (s < 0 && _actualScale < MaximumScaleFactor)
            {
                CurrentOperationMatrix = OperationsMatrices.Scale(ScaleFactor) * CurrentOperationMatrix;
                _actualScale *= ScaleFactor;
            }
        }

        protected GeometricModel(ModelType type)
        {
            Type = type;
            CurrentOperationMatrix = OperationsMatrices.Identity();
            _actualScale = 1;
        }

        public abstract void Draw(Graphics graphics, Matrix currentProjectionMatrix);

        public abstract void UpdateModel();
    }
}
