using System.Drawing;
using System.Linq;
using Mathematics;
using Matrix = Mathematics.Matrix;

namespace Models
{
    public class Point : ParametricGeometricModel
    {
        public double X { get { return Dimentions[0]; } set { Dimentions[0] = value; } }
        public double Y { get { return Dimentions[1]; } set { Dimentions[1] = value; } }
        public double Z { get { return Dimentions[2]; } set { Dimentions[2] = value; } }

        public double[] Dimentions { get; set; }

        private const double CubeSize = 0.01;

        private static int _increment = 1;

        public Point(double x, double y, double z) : base(ModelType.Point)
        {
            Dimentions = new double[3];
            X = x;
            Y = y;
            Z = z;
            CreateVertices();
            CreateEdges();
        }

        public Point(Vector4 vector, ParametricGeometricModel parent) : base(ModelType.Point)
        {
            Dimentions = new double[3];
            X = vector.X;
            Y = vector.Y;
            Z = vector.Z;
            AddParent(parent);
            CreateVertices();
            CreateEdges();
        }

        protected override void DrawModel(Graphics graphics, Matrix currentProjectionMatrix, Color color)
        {
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current*parent.CurrentOperationMatrix);
            currentMatrix = currentProjectionMatrix * CurrentOperationMatrix * currentMatrix;

            var pen = new Pen(color);
            var vertices = Vertices.Select(vertex => currentMatrix * vertex).ToList();
            float factor = (Parameters.WorldPanelWidth < Parameters.WorldPanelHeight) ?
                Parameters.WorldPanelWidth * 0.25f : Parameters.WorldPanelHeight * 0.25f;
            foreach (var edge in Edges)
            {
                graphics.DrawLine(pen, (float)vertices[edge.StartVertex].X * factor,
                    (float)vertices[edge.StartVertex].Y * factor,
                    (float)vertices[edge.EndVertex].X * factor,
                    (float)vertices[edge.EndVertex].Y * factor);
            }
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
            return "Point <" + _increment++ + ">";
        }
    }
}
