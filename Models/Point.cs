using System.Collections.Generic;
using Mathematics;

namespace Models
{
    public class Point : ParametricGeometricModel
    {
        private readonly Dictionary<ParametricGeometricModel, int> _parentsVerticIndexes;

        public double X { get { return SpacePosition.X; } set { SpacePosition.X = value; } }
        public double Y { get { return SpacePosition.Y; } set { SpacePosition.Y = value; } }
        public double Z { get { return SpacePosition.Z; } set { SpacePosition.Z = value; } }

        public double[] Dimentions { get; set; }

        private const double CubeSize = 0.01;

        private static int _increment = 1;

        public Point(Vector4 position)
            : base(ModelType.Point, position)
        {
            _parentsVerticIndexes = new Dictionary<ParametricGeometricModel, int>();
            CreateVertices();
            CreateEdges();
        }

        public Point(Vector4 position, ParametricGeometricModel parent, int vertexIndex)
            : base(ModelType.Point, position)
        {
            _parentsVerticIndexes = new Dictionary<ParametricGeometricModel, int>();
            AddParent(parent, vertexIndex);
            CreateVertices();
            CreateEdges();
        }

        public void AddParent(ParametricGeometricModel parent, int vertexIndex)
        {
            AddParent(parent);
            _parentsVerticIndexes.Add(parent, vertexIndex);
        }

        protected override void CreateEdges()
        {
            Edges.Add(new Edge(0, 1));
            Edges.Add(new Edge(1, 2));
            Edges.Add(new Edge(2, 3));
            Edges.Add(new Edge(3, 0));
            Edges.Add(new Edge(0, 4));
            Edges.Add(new Edge(1, 5));
            Edges.Add(new Edge(2, 6));
            Edges.Add(new Edge(3, 7));
            Edges.Add(new Edge(4, 5));
            Edges.Add(new Edge(5, 6));
            Edges.Add(new Edge(6, 7));
            Edges.Add(new Edge(7, 4));
        }

        protected override void CreateVertices()
        {
            Vertices.Add(new Vector4(X - CubeSize, Y - CubeSize, Z - CubeSize));
            Vertices.Add(new Vector4(X + CubeSize, Y - CubeSize, Z - CubeSize));
            Vertices.Add(new Vector4(X + CubeSize, Y - CubeSize, Z + CubeSize));
            Vertices.Add(new Vector4(X - CubeSize, Y - CubeSize, Z + CubeSize));
            Vertices.Add(new Vector4(X - CubeSize, Y + CubeSize, Z - CubeSize));
            Vertices.Add(new Vector4(X + CubeSize, Y + CubeSize, Z - CubeSize));
            Vertices.Add(new Vector4(X + CubeSize, Y + CubeSize, Z + CubeSize));
            Vertices.Add(new Vector4(X - CubeSize, Y + CubeSize, Z + CubeSize));
        }

        public override string ToString()
        {
            if (CustomName != null)
                return CustomName;
            return "Point <" + _increment++ + ">";
        }
    }
}
