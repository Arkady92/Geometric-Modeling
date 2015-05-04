using Mathematics;

namespace Models
{
    public class BezierPatch : ParametricGeometricModel
    {
        private readonly double _width;
        private readonly double _height;
        private readonly bool _isCylindric;

        public BezierPatch(Vector4 position, double width, double height, bool isCylindric)
            : base(ModelType.BezierPatch, position)
        {
            _width = width;
            _height = height;
            _isCylindric = isCylindric;
        }


        protected override void CreateEdges()
        {
        }

        protected override void CreateVertices()
        {
        }
    }
}
