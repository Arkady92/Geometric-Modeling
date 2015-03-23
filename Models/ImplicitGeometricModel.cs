using Mathematics;

namespace Models
{
    public abstract class ImplicitGeometricModel : GeometricModel
    {
        protected ImplicitGeometricModel(ModelType modelType, Vector4 position)
            : base(modelType, position)
        {
            TranslationFactor = 0.5;
            RotationFactor = 0.004;
            ScaleFactor = 1.1;
            MaximumScaleFactor = 10;
        }

        public override void UpdatePositions(Vector4 shift)
        {
            SpacePosition = SpacePosition + shift;
        }

        public abstract void Draw(System.Drawing.Graphics graphic, int pixelSize, Mathematics.Matrix currentProjectionMatrix = null);
    }
}
