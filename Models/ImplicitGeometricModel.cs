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

        public abstract void Draw(System.Drawing.Graphics graphic, int pixelSize, Matrix currentProjectionMatrix = null);
    }
}
