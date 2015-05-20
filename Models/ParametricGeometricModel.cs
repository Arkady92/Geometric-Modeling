using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Mathematics;

namespace Models
{
    public abstract class ParametricGeometricModel : GeometricModel
    {
        protected List<Vector4> Vertices;

        protected List<Edge> Edges;

        protected List<ParametricGeometricModel> Children;

        protected List<ParametricGeometricModel> Parents;

        protected readonly Dictionary<ParametricGeometricModel, int> ParentsIndexes;


        public bool ReturnChildrenOnRemove { get; set; }

        protected ParametricGeometricModel(ModelType modelType, Vector4 position, bool returnChildrenOnRemove = false)
            : base(modelType, position)
        {
            Vertices = new List<Vector4>();
            Edges = new List<Edge>();
            Children = new List<ParametricGeometricModel>();
            Parents = new List<ParametricGeometricModel>();
            ParentsIndexes = new Dictionary<ParametricGeometricModel, int>();
            TranslationFactor = 0.003;
            RotationFactor = 0.01;
            ScaleFactor = 1.02;
            MaximumScaleFactor = 20;
            Color = Color.White;
            ReturnChildrenOnRemove = returnChildrenOnRemove;
        }

        protected abstract void CreateEdges();

        protected abstract void CreateVertices();

        public override Vector4 GetCurrentPosition()
        {
            Matrix parrentsMatrix = OperationsMatrices.Identity();
            parrentsMatrix = Parents.Aggregate(parrentsMatrix, (current, parent) => current * parent.OperationMatrix);
            return OperationMatrix * parrentsMatrix * SpacePosition;
        }

        public Vector4 GetCurrentPositionWithoutMineTransformations(ParametricGeometricModel model)
        {
            Matrix parrentsMatrix = OperationsMatrices.Identity();
            parrentsMatrix = Parents.Where(parent => parent != model).Aggregate(parrentsMatrix, (current, parent) => current * parent.OperationMatrix);
            return OperationMatrix * parrentsMatrix * SpacePosition;
        }

        public override void UpdateModel()
        {
            Vertices.Clear();
            Edges.Clear();
            Children.Clear();
            CreateVertices();
            CreateEdges();
        }

        public virtual List<ParametricGeometricModel> GetChildren()
        {
            return Children;
        }

        public void AddParent(ParametricGeometricModel parent)
        {
            Parents.Add(parent);
        }

        public void AddChild(ParametricGeometricModel child)
        {
            Children.Add(child);
        }

        public void RemoveParent(ParametricGeometricModel parent)
        {
            OperationMatrix = parent.OperationMatrix * OperationMatrix;
            Parents.Remove(parent);
        }
        public void RemoveChild(ParametricGeometricModel child)
        {
            Children.Remove(child);
        }

        public void RemoveModel()
        {
            foreach (var child in Children)
            {
                child.RemoveParent(this);
            }
            foreach (var parent in Parents)
            {
                parent.RemoveChild(this);
                parent.RecreateStructure(ParentsIndexes[parent]);
            }
        }

        protected virtual void RecreateStructure(int number = 0) { }

        public void SetParentIndex(ParametricGeometricModel parent, int index)
        {
            if (ParentsIndexes.ContainsKey(parent))
                ParentsIndexes[parent] = index;
        }

        public virtual void UpdateVertex(int number)
        {
            var position = Children[number].GetCurrentPositionWithoutMineTransformations(this);
            Vertices[number].X = position.X;
            Vertices[number].Y = position.Y;
            Vertices[number].Z = position.Z;
        }

        public virtual void DrawStereoscopy(Graphics graphics, Matrix leftMatrix, Matrix rightMatrix,
            bool additiveColorBlending = false)
        {
            if ((Type != ModelType.BezierCurveC2 && Type != ModelType.BezierCurve) ||
                (Type == ModelType.BezierCurveC2 && !((this as BezierCurveC2).CurveParent is InterpolationCurve) 
                && !(this as BezierCurveC2).ControlPointsEnabled) ||
                (Type == ModelType.BezierCurve && (this as BezierCurve).ControlPointsEnabled))
                foreach (var geometricModel in Children)
                    geometricModel.DrawStereoscopy(graphics, leftMatrix, rightMatrix, additiveColorBlending);
            if (!additiveColorBlending)
            {
                DrawModel(graphics, leftMatrix, Color.Red);
                DrawModel(graphics, rightMatrix, Color.Blue);
                return;
            }
            var bitmapResult = new Bitmap(Parameters.WorldPanelWidth, Parameters.WorldPanelHeight);
            var graphicsResult = Graphics.FromImage(bitmapResult);
            var bitmapFinalResult = new Bitmap(Parameters.WorldPanelWidth, Parameters.WorldPanelHeight);
            var graphicsFinalResult = Graphics.FromImage(bitmapFinalResult);
            graphicsResult.TranslateTransform(Parameters.WorldPanelWidth * 0.5f, Parameters.WorldPanelHeight * 0.5f);
            graphicsFinalResult.TranslateTransform(Parameters.WorldPanelWidth * 0.5f, Parameters.WorldPanelHeight * 0.5f);
            DrawWithAdditiveBlending(bitmapResult, graphicsResult, leftMatrix, Color.Red);
            DrawWithAdditiveBlending(bitmapResult, graphicsResult, rightMatrix, Color.Blue);
            graphics.DrawImage(bitmapResult, new Rectangle(-Parameters.WorldPanelWidth / 2, -Parameters.WorldPanelHeight / 2,
                Parameters.WorldPanelWidth, Parameters.WorldPanelHeight));
        }

        protected virtual void DrawWithAdditiveBlending(Bitmap bitmap, Graphics graphics, Matrix currentProjectionMatrix, Color color)
        {
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.OperationMatrix);
            currentMatrix = currentProjectionMatrix * currentMatrix * OperationMatrix;

            var vertices = Vertices.Select(vertex => currentMatrix * vertex).ToList();
            foreach (var edge in Edges)
            {
                var customLine = new CustomLine(
                    new System.Drawing.Point(
                        (int)(vertices[edge.StartVertex].X * Parameters.WorldPanelSizeFactor),
                        (int)(vertices[edge.StartVertex].Y * Parameters.WorldPanelSizeFactor)),
                    new System.Drawing.Point(
                        (int)(vertices[edge.EndVertex].X * Parameters.WorldPanelSizeFactor),
                        (int)(vertices[edge.EndVertex].Y * Parameters.WorldPanelSizeFactor)));
                customLine.Draw(bitmap, graphics, color, 1);
            }
        }

        public override void Draw(Graphics graphics, Matrix currentProjectionMatrix)
        {
            if ((Type != ModelType.BezierCurveC2 && Type != ModelType.BezierCurve) ||
                (Type == ModelType.BezierCurveC2 && !((this as BezierCurveC2).CurveParent is InterpolationCurve)
                && !(this as BezierCurveC2).ControlPointsEnabled) ||
                (Type == ModelType.BezierCurve && (this as BezierCurve).ControlPointsEnabled))
                foreach (var geometricModel in Children)
                    geometricModel.Draw(graphics, currentProjectionMatrix);
            DrawModel(graphics, currentProjectionMatrix, Color);
        }

        protected virtual void DrawModel(Graphics graphics, Matrix currentProjectionMatrix, Color color)
        {
            Matrix currentMatrix = OperationsMatrices.Identity();
            currentMatrix = Parents.Aggregate(currentMatrix, (current, parent) => current * parent.OperationMatrix);
            currentMatrix = currentProjectionMatrix * currentMatrix * OperationMatrix;

            var pen = new Pen(color);
            var vertices = Vertices.Select(vertex => currentMatrix * vertex).ToList();
            foreach (var edge in Edges)
            {
                graphics.DrawLine(pen, (float)vertices[edge.StartVertex].X * Parameters.WorldPanelSizeFactor,
                    (float)vertices[edge.StartVertex].Y * Parameters.WorldPanelSizeFactor,
                    (float)vertices[edge.EndVertex].X * Parameters.WorldPanelSizeFactor,
                    (float)vertices[edge.EndVertex].Y * Parameters.WorldPanelSizeFactor);
            }
        }

        public virtual void PropagateTransformation(ParametricGeometricModel geometricModel = null)
        {
            foreach (var child in Children)
            {
                child.PropagateTransformation(this);
            }
        }

        public override void Translate(double x, double y, double z)
        {
            base.Translate(x, y, z);
            PropagateTransformation();
        }

        public override void TranslateToPosition(Vector4 position)
        {
            var shift = GetCurrentPosition();
            shift = position - shift;
            OperationMatrix = OperationsMatrices.Translation(shift.X, shift.Y, shift.Z) * OperationMatrix;
            PropagateTransformation();
        }

        public override void Rotate(double x, double y, double z)
        {
            base.Rotate(x, y, z);
            PropagateTransformation();
        }

        public override void Scale(double s)
        {
            base.Scale(s);
            PropagateTransformation();
        }
    }
}
