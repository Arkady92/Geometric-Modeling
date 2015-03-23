using System.Drawing;
using Mathematics;

namespace Models
{
    public class Cursor : ParametricGeometricModel
    {
        private static Cursor _instance;

        private static Vector4 _position = new Vector4(0, 0, 0);
        private static readonly Vector4 ScreenPosition = new Vector4(0, 0, 0);

        private static GeometricModel _handledModel;

        public static bool ModelHandled;

        public static double XPosition
        {
            get
            {
                return _position.X;
            }
            set
            {
                _position.X = value;
                if (ModelHandled)
                    _handledModel.UpdatePositions(_position - _handledModel.GetBasePosition());
            }
        }

        public static double YPosition
        {
            get
            {
                return _position.Y;
            }
            set
            {
                _position.Y = value;
                if (ModelHandled)
                    _handledModel.UpdatePositions(_position - _handledModel.GetBasePosition());
            }
        }

        public static double ZPosition
        {
            get
            {
                return _position.Z;
            }
            set
            {
                _position.Z = value;
                if (ModelHandled)
                    _handledModel.UpdatePositions(_position - _handledModel.GetBasePosition());
            }
        }

        public static double ScreenXPosition { get { return ScreenPosition.X; } set { ScreenPosition.X = value; } }
        public static double ScreenYPosition { get { return ScreenPosition.Y; } set { ScreenPosition.Y = value; } }

        public const double CursorSize = 0.08;

        private Cursor()
            : base(ModelType.Cursor, _position)
        {
            CreateVertices();
            CreateEdges();
        }

        public static Cursor Instance
        {
            get { return _instance ?? (_instance = new Cursor()); }
        }

        public new static Vector4 GetCurrentPosition()
        {
            return Instance.CurrentOperationMatrix*_position;
        }

        public static Vector4 GetPosition()
        {
            return new Vector4(_position.X, _position.Y, _position.Z);
        }

        public static void SetPosition(Vector4 position)
        {
            _position = position;
        }

        public static void AddHandledModel(GeometricModel model)
        {
            _handledModel = model;
            ModelHandled = true;
        }
        public static void RemoveHandledModel()
        {
            _handledModel = null;
            ModelHandled = false;
        }

        protected override void CreateEdges()
        {
            Edges.Add(new Edge(0, 1));
            Edges.Add(new Edge(2, 3));
            Edges.Add(new Edge(4, 5));
        }

        protected override void CreateVertices()
        {
            Vertices.Add(new Vector4(XPosition - CursorSize, YPosition, ZPosition));
            Vertices.Add(new Vector4(XPosition + CursorSize, YPosition, ZPosition));
            Vertices.Add(new Vector4(XPosition, YPosition + CursorSize, ZPosition));
            Vertices.Add(new Vector4(XPosition, YPosition - CursorSize, ZPosition));
            Vertices.Add(new Vector4(XPosition, YPosition, ZPosition - CursorSize));
            Vertices.Add(new Vector4(XPosition, YPosition, ZPosition + CursorSize));
        }

        public override void Draw(Graphics graphics, Matrix currentProjectionMatrix)
        {
            base.Draw(graphics, currentProjectionMatrix);
            var position = currentProjectionMatrix * CurrentOperationMatrix * _position;
            ScreenPosition.X = (int)(position.X * Parameters.WorldPanelSizeFactor);
            ScreenPosition.Y = (int)(position.Y * Parameters.WorldPanelSizeFactor);
        }

        public override void DrawStereoscopy(Graphics graphics, Matrix leftMatrix, Matrix rightMatrix, bool additiveColorBlending = false)
        {
            base.DrawStereoscopy(graphics, leftMatrix, rightMatrix, additiveColorBlending);
            var lPos = leftMatrix * CurrentOperationMatrix * _position;
            var rPos = rightMatrix * CurrentOperationMatrix * _position;
            ScreenPosition.X = (int)((lPos.X + rPos.X) * 0.5 * Parameters.WorldPanelSizeFactor);
            ScreenPosition.Y = (int)((lPos.Y + rPos.Y) * 0.5 * Parameters.WorldPanelSizeFactor);
        }
    }
}
