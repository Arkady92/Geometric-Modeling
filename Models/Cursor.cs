using System;
using System.Drawing;
using Mathematics;

namespace Models
{
    public class Cursor : ParametricGeometricModel
    {
        private static Cursor _instance;

        private static Vector4 _position = new Vector4(0, 0, 0);
        private static Vector4 _screenPosition = new Vector4(0, 0, 0);

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
                _position.X = Math.Round(value, 2);
                if (ModelHandled)
                    _handledModel.TranslateToPosition(GetCurrentPosition());
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
                _position.Y = Math.Round(value, 2);
                if (ModelHandled)
                    _handledModel.TranslateToPosition(GetCurrentPosition());
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
                _position.Z = Math.Round(value, 2);
                if (ModelHandled)
                    _handledModel.TranslateToPosition(GetCurrentPosition());
            }
        }

        public static double ScreenXPosition { get { return _screenPosition.X; } set { _screenPosition.X = value; } }
        public static double ScreenYPosition { get { return _screenPosition.Y; } set { _screenPosition.Y = value; } }

        public const double CursorSize = 0.025;

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
            return Instance.OperationMatrix * _position;
        }

        public static void AddHandledModel(GeometricModel model)
        {
            _handledModel = model;
            ModelHandled = true;
            Instance.Color = Color.Yellow;
        }
        public static void RemoveHandledModel()
        {
            _handledModel = null;
            ModelHandled = false;
            Instance.Color = Color.White;
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
            var position = currentProjectionMatrix * OperationMatrix * _position;
            _screenPosition.X = (int)(position.X * Parameters.WorldPanelSizeFactor);
            _screenPosition.Y = -(int)(position.Y * Parameters.WorldPanelSizeFactor);
        }

        public override void DrawStereoscopy(Graphics graphics, Matrix leftMatrix, Matrix rightMatrix, bool additiveColorBlending = false)
        {
            base.DrawStereoscopy(graphics, leftMatrix, rightMatrix, additiveColorBlending);
            var lPos = leftMatrix * OperationMatrix * _position;
            var rPos = rightMatrix * OperationMatrix * _position;
            _screenPosition.X = (int)((lPos.X + rPos.X) * 0.5 * Parameters.WorldPanelSizeFactor);
            _screenPosition.Y = -(int)((lPos.Y + rPos.Y) * 0.5 * Parameters.WorldPanelSizeFactor);
        }
    }
}
