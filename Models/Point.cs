using System.Globalization;
using System.Xml.Serialization;
using Mathematics;

namespace Models
{
    public class Point : ParametricGeometricModel
    {
        public double X
        {
            get { return SpacePosition.X; }
            set
            {
                SpacePosition.X = value;
                RecreateStructure();
                PropagateTransformation();
            }
        }
        public double Y
        {
            get { return SpacePosition.Y; }
            set
            {
                SpacePosition.Y = value;
                RecreateStructure();
                PropagateTransformation();
            }
        }
        public double Z
        {
            get { return SpacePosition.Z; }
            set
            {
                SpacePosition.Z = value;
                RecreateStructure();
                PropagateTransformation();
            }
        }

        private const double CubeSize = 0.003;

        private static int _increment = 1;

        protected override void RecreateStructure(int number = 0)
        {
            Vertices.Clear();
            Edges.Clear();
            CreateVertices();
            CreateEdges();
        }

        public Point(Vector4 position)
            : base(ModelType.Point, position)
        {
            CreateVertices();
            CreateEdges();
            CustomName = (_increment++).ToString(CultureInfo.InvariantCulture);
        }

        public Point(Vector4 position, ParametricGeometricModel parent, int vertexIndex)
            : base(ModelType.Point, position)
        {
            AddParent(parent, vertexIndex);
            CreateVertices();
            CreateEdges();
            CustomName = (_increment++).ToString(CultureInfo.InvariantCulture);
        }

        public void AddParent(ParametricGeometricModel parent, int vertexIndex)
        {
            AddParent(parent);
            ParentsIndexes.Add(parent, vertexIndex);
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

        public override void PropagateTransformation(ParametricGeometricModel geometricModel = null)
        {
            base.PropagateTransformation(geometricModel);
            foreach (var parent in Parents)
            {
                if (parent != geometricModel)
                    parent.UpdateVertex(ParentsIndexes[parent]);
            }
        }

        public void ChangePoint(Point point)
        {
            foreach (var parent in Parents)
            {
                parent.ChangeChild(this, point);
            }
        }

        public Vector4 SetCenterPosition(Point point)
        {
            var position = Vector4.Center(GetCurrentPosition(), point.GetCurrentPosition());
            TranslateToPosition(position);
            return position;
        }

        public void CorrectPosition(Vector4 position)
        {
            TranslateToPosition(position);
            foreach (var parent in Parents)
                parent.UpdateVertex(ParentsIndexes[parent]);
        }

        public ParametricGeometricModel GetOnlyParent()
        {
            if (Parents.Count != 1) return null;
            return Parents[0];
        }

        private Point _collaborator;

        public void AddCollaborator(Point point)
        {
            _collaborator = point;
        }

        public override void TranslateToPosition(Vector4 position)
        {
            base.TranslateToPosition(position);
            if (_collaborator != null)
                _collaborator.TranslateToPosition(position);
        }

        public override void Translate(double x, double y, double z)
        {
            base.Translate(x, y, z);
            if (_collaborator != null)
                _collaborator.Translate(x, y, z);
        }
    }
}
