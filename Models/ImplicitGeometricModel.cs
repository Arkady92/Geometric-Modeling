namespace Models
{
    public abstract class ImplicitGeometricModel : GeometricModel
    {
        protected ImplicitGeometricModel(ModelType modelType)
            : base(modelType) {}

        public abstract void Draw(System.Drawing.Graphics graphics, Mathematics.Matrix currentProjectionMatrix,
            int pixelSize);
    }
}
