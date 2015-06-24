using Mathematics;
using System.Collections.Generic;
using System.Globalization;

namespace Models
{
    public class IntersectionCurve : ParametricGeometricModel
    {
        private static int _increment = 1;

        private List<Vector4> _uvPoints;
        private List<Vector4> _stPoints;

        public IntersectionCurve(List<Vector4> points, List<Vector4> uvPoints, List<Vector4> stPoints, 
            Vector4 position) : base(ModelType.IntersectionCurve, position, false)
        {
            _uvPoints = uvPoints;
            _stPoints = stPoints;
            foreach (Vector4 point in points)
            {
                Vertices.Add(point);
            }
            CreateEdges();
            CustomName = (_increment++).ToString(CultureInfo.InvariantCulture);
        }

        protected override void CreateEdges()
        {
            for (int i = 0; i < Vertices.Count - 1; i++)
                Edges.Add(new Edge(i, i + 1));
        }

        protected override void CreateVertices() {}

        public List<Vector4> GetUVPoints()
        {
            return _uvPoints;
        }

        public List<Vector4> GetSTPoints()
        {
            return _stPoints;
        }
    }
}
