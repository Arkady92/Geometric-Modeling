namespace Models
{
    public abstract class ImplicitGeometricModel : GeometricModel
    {
        protected ImplicitGeometricModel(ModelType modelType)
            : base(modelType)
        {
            TranslationFactor = 0.5;
            RotationFactor = 0.01;
            ScaleFactor = 1.1;
            MaximumScaleFactor = 10;
        }

        public abstract void Draw(System.Drawing.Graphics graphics, Mathematics.Matrix currentProjectionMatrix,
            int pixelSize);
    }
}
