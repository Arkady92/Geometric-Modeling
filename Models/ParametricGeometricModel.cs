using System.Collections.Generic;
using Mathematics;

namespace Models
{
    public abstract class ParametricGeometricModel : GeometricModel
    {
        protected List<Vector4> Vertices;

        protected List<Edge> Edges;

        protected ParametricGeometricModel(ModelType modelType)
            : base(modelType)
        {
            Vertices = new List<Vector4>();
            Edges = new List<Edge>();
        }

        protected abstract void CreateEdges();

        protected abstract void CreateVertices();

        public override void UpdateModel()
        {
            Vertices.Clear();
            Edges.Clear();
            CreateVertices();
            CreateEdges();
        }
    }
}
