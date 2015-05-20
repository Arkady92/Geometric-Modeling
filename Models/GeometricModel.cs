using System;
using System.Drawing;
using Mathematics;

namespace Models
{
    [Serializable]
    public abstract class GeometricModel
    {
        public bool IsRemovableFromScene = true;

        public bool IsRemovableFromModel = true;

        public static int Increment;

        public Color Color;

        public int Id { get; set; }

        public ModelType Type;

        public string CustomName { get; set;}

        protected Vector4 SpacePosition;

        public Matrix OperationMatrix { get; set; }

        protected double TranslationFactor;

        protected double RotationFactor;

        protected double ScaleFactor;
        
        protected double MaximumScaleFactor;

        private double _actualScale;

        public void ResetOperationMatrix()
        {
            OperationMatrix = OperationsMatrices.Identity();
        }

        public virtual Vector4 GetCurrentPosition()
        {
            return OperationMatrix * SpacePosition;
        }

        public Vector4 GetBasePosition()
        {
            return SpacePosition;
        }

        public virtual void Translate(double x, double y, double z)
        {
            OperationMatrix = OperationsMatrices.Translation(x * TranslationFactor, y * TranslationFactor,
                z * TranslationFactor) * OperationMatrix;
        }

        public virtual void TranslateToPosition(Vector4 position)
        {
            var shift = GetCurrentPosition();
            shift = position - shift;
            OperationMatrix = OperationsMatrices.Translation(shift.X, shift.Y, shift.Z) * OperationMatrix;
        }

        public virtual void Rotate(double x, double y, double z)
        {
            if (Math.Abs(x) > Double.Epsilon)
                OperationMatrix = OperationsMatrices.RotationX(x * RotationFactor) * OperationMatrix;
            if (Math.Abs(y) > Double.Epsilon)
                OperationMatrix = OperationsMatrices.RotationY(y * RotationFactor) * OperationMatrix;
            if (Math.Abs(z) > Double.Epsilon)
                OperationMatrix = OperationsMatrices.RotationZ(z * RotationFactor) * OperationMatrix;
        }

        public virtual void Scale(double s)
        {
            if (s > 0 && _actualScale > 1 / MaximumScaleFactor)
            {
                OperationMatrix = OperationsMatrices.Scale(1 / ScaleFactor)*OperationMatrix;
                _actualScale /= ScaleFactor;
            }
            if (s < 0 && _actualScale < MaximumScaleFactor)
            {
                OperationMatrix = OperationsMatrices.Scale(ScaleFactor) * OperationMatrix;
                _actualScale *= ScaleFactor;
            }
        }

        protected GeometricModel(ModelType type, Vector4 position)
        {
            Id = Increment++;
            Type = type;
            SpacePosition = position;
            OperationMatrix = OperationsMatrices.Identity();
            _actualScale = 1;
        }

        public abstract void Draw(Graphics graphics, Matrix currentProjectionMatrix);

        public abstract void UpdateModel();

        public override string ToString()
        {
            return Type + " <" + CustomName + ">";
        }

    }
}
