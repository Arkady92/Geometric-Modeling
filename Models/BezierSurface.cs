using Mathematics;

namespace Models
{
    public class BezierSurface : ParametricGeometricModel
    {
        private readonly double _width;
        private readonly double _height;
        private readonly int _patchesLengthCount;
        private readonly int _patchesBreadthCount;
        private readonly bool _isCylindrical;

        public bool ChainEnabled { get; set; }

        public BezierSurface(Vector4 position, double width, double height, int patchesLengthCount, int patchesBreadthCount, 
            bool isCylindrical)
            : base(ModelType.BezierSurface, position)
        {
            _width = width;
            _height = height;
            _patchesLengthCount = patchesLengthCount;
            _patchesBreadthCount = patchesBreadthCount;
            _isCylindrical = isCylindrical;
        }


        protected override void CreateEdges()
        {
        }

        protected override void CreateVertices()
        {
        }

        public virtual void TogglePolygonialChain()
        {
            ChainEnabled = !ChainEnabled;
        }

    }
}
