using System.Drawing;
using Mathematics;

namespace Models
{
    public abstract class ImplicitGeometricModel : GeometricModel
    {
        public Matrix ModelMatrix { get; set; }

        protected ImplicitGeometricModel(ModelType modelType, Vector4 position)
            : base(modelType, position)
        {
            TranslationFactor = 0.5;
            RotationFactor = 0.004;
            ScaleFactor = 1.1;
            MaximumScaleFactor = 10;
            Color = Color.Yellow;
        }

        public abstract void Draw(System.Drawing.Graphics graphic, int pixelSize, Matrix currentProjectionMatrix = null);
    }
}
