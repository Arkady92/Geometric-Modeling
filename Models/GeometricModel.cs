using System.Collections.Generic;
using System.Drawing;
using Mathematics;

namespace Models
{
    public abstract class GeometricModel
    {
        public ModelType Type;

        protected List<Vector4> Vertices;

        protected List<Edge> Edges; 

        protected GeometricModel(ModelType type)
        {
            Type = type;
            Vertices = new List<Vector4>();
            Edges = new List<Edge>();
        }

        public abstract void Draw(Graphics graphics, Mathematics.Matrix currentMatrix);

        public abstract void UpdateMesh();
    }
}
