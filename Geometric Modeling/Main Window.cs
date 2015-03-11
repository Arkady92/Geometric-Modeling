using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Mathematics;
using Models;

namespace Geometric_Modeling
{
    public partial class MainWindow : Form
    {
        #region Members
        private List<GeometricModel> _models;
        private Dictionary<Operation, Button> _operationsButtons;
        private Dictionary<ModelType, List<Control>> _modelsParameters;
        private Operation _currentOperation;
        private bool _operationInProgress;
        private int _mousePositionX;
        private int _mousePositionY;
        //private Matrix _currentOperationsMatrix;
        private Matrix _currentProjectionMatrix;
        private const double MaximumScale = 40;
        private const double MinimumScale = 0.025;
        private double _actualScale;
        private Bitmap _backBuffer;
        private bool _forceStaticGraphics;
        private bool _enableAnimations;

        #endregion

        #region Initialization
        public MainWindow()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            SetDoubleBuffered(WorldPanel);
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            _backBuffer = new Bitmap(WorldPanel.Width, WorldPanel.Height);
            _forceStaticGraphics = false;
            _enableAnimations = false;
            GridResolutionXBox.Text = Parameters.GridResolutionX.ToString(CultureInfo.InvariantCulture);
            GridResolutionYBox.Text = Parameters.GridResolutionY.ToString(CultureInfo.InvariantCulture);
            IlluminanceBox.Text = Parameters.Illuminance.ToString(CultureInfo.InvariantCulture);
            XAxisFactorBox.Text = Parameters.XAxisFactor.ToString(CultureInfo.InvariantCulture);
            YAxisFactorBox.Text = Parameters.YAxisFactor.ToString(CultureInfo.InvariantCulture);
            ZAxisFactorBox.Text = Parameters.ZAxisFactor.ToString(CultureInfo.InvariantCulture);
            PixelMaxSizeBox.Text = Parameters.PixelMaxSize.ToString(CultureInfo.InvariantCulture);
            Parameters.WorldPanelWidth = WorldPanel.Width;
            Parameters.WorldPanelHeight = WorldPanel.Height;
            _models = new List<GeometricModel>();
            _operationsButtons = new Dictionary<Operation, Button>
            {
                {Operation.TranslationX, TranslationXButton},
                {Operation.TranslationY, TranslationYButton},
                {Operation.TranslationZ, TranslationZButton},
                {Operation.RotationX, RotationXButton},
                {Operation.RotationY, RotationYButton},
                {Operation.RotationZ, RotationZButton},
                {Operation.Scale, ScaleButton}
            };
            _modelsParameters = new Dictionary<ModelType, List<Control>>
            {
                {ModelType.Torus, new List<Control> {GridResolutionXBox, GridResolutionYBox, GridResolutionXLabel, 
                    GridResolutionYLabel}},
                {ModelType.Ellipsoid, new List<Control> {IlluminanceBox, XAxisFactorBox, YAxisFactorBox, ZAxisFactorBox, PixelMaxSizeBox, 
                    IlluminanceLabel, XAxisFactorLabel, YAxisFactorLabel, ZAxisFactorLabel, PixelMaxSizeLabel}}
            };
            _currentOperation = Operation.None;
            //_currentOperationsMatrix = OperationsMatrices.Identity();
            _currentProjectionMatrix = OperationsMatrices.Projection(50);
            _actualScale = 1;
        }

        public static void SetDoubleBuffered(Control c)
        {
            System.Reflection.PropertyInfo aProp = typeof(Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }
        #endregion

        #region Cooperation with models
        private void DrawWorld()
        {
            Graphics graphics;
            if (Parameters.GridResolutionX * Parameters.GridResolutionY < 5000 && !_forceStaticGraphics)
                graphics = Graphics.FromImage(_backBuffer);
            else
                graphics = WorldPanel.CreateGraphics();
            graphics.Clear(Color.White);
            if (_models == null) return;
            graphics.TranslateTransform((float)(WorldPanel.Size.Width * 0.5), (float)(WorldPanel.Size.Height * 0.5));
            foreach (var geometricModel in _models)
            {
                if (geometricModel is ImplicitGeometricModel && _enableAnimations)
                {
                    var pixelSize = Parameters.PixelMaxSize;
                    while (pixelSize > 1)
                    {
                        graphics.Clear(Color.White);
                        (geometricModel as ImplicitGeometricModel).Draw(graphics, _currentProjectionMatrix,
                            pixelSize);
                        if (Parameters.GridResolutionX * Parameters.GridResolutionY < 5000 && !_forceStaticGraphics)
                            WorldPanel.Image = _backBuffer;
                        pixelSize--;
                        System.Threading.Thread.Sleep(50);
                    }
                }
            }
            foreach (var geometricModel in _models)
                geometricModel.Draw(graphics, _currentProjectionMatrix);

            if (Parameters.GridResolutionX * Parameters.GridResolutionY < 5000 && !_forceStaticGraphics)
                WorldPanel.Image = _backBuffer;
        }

        private void UpdateCurrentModel()
        {
            var model = ObjectsList.SelectedItem as GeometricModel;
            if (model == null) return;
            model.UpdateModel();
        }
        #endregion

        #region Mouse events

        private void WorldPanel_MouseDown(object sender, MouseEventArgs e)
        {
            _operationInProgress = true;
            _mousePositionX = e.X;
            _mousePositionY = e.Y;
        }

        private void WorldPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_operationInProgress) return;
            var deltaX = e.X - _mousePositionX;
            var deltaY = e.Y - _mousePositionY;
            const double positiveScale = 1.11;
            const double negativeScale = 0.91;
            const double factor = 0.03;
            var matrix = OperationsMatrices.Identity();
            switch (_currentOperation)
            {
                case Operation.TranslationX:
                    matrix = OperationsMatrices.Translation(deltaX * factor, 0, 0);
                    break;
                case Operation.TranslationY:
                    matrix = OperationsMatrices.Translation(0, deltaY * factor, 0);
                    break;
                case Operation.TranslationZ:
                    matrix = OperationsMatrices.Translation(0, 0, deltaY * factor);
                    break;
                case Operation.RotationX:
                    matrix = OperationsMatrices.RotationX(deltaY * factor);
                    break;
                case Operation.RotationY:
                    matrix = OperationsMatrices.RotationY(deltaX * factor);
                    break;
                case Operation.RotationZ:
                    matrix = OperationsMatrices.RotationZ(deltaX * factor);
                    break;
                case Operation.Scale:
                    if (deltaY < 0 && _actualScale < MaximumScale)
                    {
                        matrix = OperationsMatrices.Scale(positiveScale);
                        _actualScale *= positiveScale;
                    }
                    if (deltaY > 0 && _actualScale > MinimumScale)
                    {
                        matrix = OperationsMatrices.Scale(negativeScale);
                        _actualScale *= negativeScale;
                    }
                    break;
                default:
                    _mousePositionX = e.X;
                    _mousePositionY = e.Y;
                    return;
            }
            _mousePositionX = e.X;
            _mousePositionY = e.Y;
            ApplyOperationsChange(matrix);
        }

        private void ApplyOperationsChange(Matrix matrix)
        {
            if (ObjectsList.SelectedItem == null)
                foreach (var geometricModel in _models)
                {
                    _enableAnimations = true;
                    geometricModel.CurrentOperationsMatrix = matrix * geometricModel.CurrentOperationsMatrix;
                }
            else
            {
                var model = ObjectsList.SelectedItem as GeometricModel;
                if (model != null)
                {
                    if (model is ImplicitGeometricModel)
                        _enableAnimations = true;
                    else
                        _enableAnimations = false;
                    model.CurrentOperationsMatrix = matrix * model.CurrentOperationsMatrix;
                }
            }
            DrawWorld();
        }

        private void WorldPanel_MouseUp(object sender, MouseEventArgs e)
        {
            _operationInProgress = false;
        }
        #endregion

        #region Buttons click events
        private void TorusButton_Click(object sender, EventArgs e)
        {
            var geometricObject = new Torus();
            _models.Add(geometricObject);
            ObjectsList.Items.Add(geometricObject);
            _enableAnimations = false;
            DrawWorld();
        }

        private void EllipsoidButton_Click(object sender, EventArgs e)
        {
            var geometricObject = new Ellipsoid();
            _models.Add(geometricObject);
            ObjectsList.Items.Add(geometricObject);
            _forceStaticGraphics = true;
            _enableAnimations = true;
            DrawWorld();
        }

        private void TranslationXButton_Click(object sender, EventArgs e)
        {
            SwitchOperation(Operation.TranslationX);
        }

        private void TranslationYButton_Click(object sender, EventArgs e)
        {
            SwitchOperation(Operation.TranslationY);
        }

        private void TranslationZButton_Click(object sender, EventArgs e)
        {
            SwitchOperation(Operation.TranslationZ);
        }

        private void RotationXButton_Click(object sender, EventArgs e)
        {
            SwitchOperation(Operation.RotationX);
        }

        private void RotationYButton_Click(object sender, EventArgs e)
        {
            SwitchOperation(Operation.RotationY);
        }

        private void RotationZButton_Click(object sender, EventArgs e)
        {
            SwitchOperation(Operation.RotationZ);
        }

        private void ScaleButton_Click(object sender, EventArgs e)
        {
            SwitchOperation(Operation.Scale);
        }

        void SwitchOperation(Operation operation)
        {
            if (_currentOperation == operation)
            {
                _operationsButtons[_currentOperation].BackColor = SystemColors.Control;
                _currentOperation = Operation.None;
            }
            else
            {
                if (_currentOperation != Operation.None)
                    _operationsButtons[_currentOperation].BackColor = SystemColors.Control;
                _currentOperation = operation;
                _operationsButtons[_currentOperation].BackColor = Color.GreenYellow;
            }
        }

        private void ObjectsList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var item = ObjectsList.SelectedItem;
            if (item != null)
            {
                _models.Remove(item as GeometricModel);
                ObjectsList.Items.Remove(ObjectsList.SelectedItem);
            }
            DisableAllSettings();
            DrawWorld();
        }

        private void ObjectsList_MouseClick(object sender, MouseEventArgs e)
        {
            var item = ObjectsList.SelectedItem;
            if (item == null) return;
            DisableAllSettings();
            foreach (var parameterBox in _modelsParameters[((GeometricModel)item).Type])
            {
                parameterBox.Enabled = true;
            }
        }

        private void ObjectsListLabel_Click(object sender, EventArgs e)
        {
            ObjectsList.ClearSelected();
            DisableAllSettings();
        }

        private void DisableAllSettings()
        {
            foreach (var modelsParameter in _modelsParameters.Values)
            {
                foreach (var parameterBox in modelsParameter)
                {
                    parameterBox.Enabled = false;
                }
            }
        }
        #endregion

        #region Contents change events
        private void GridResolutionXBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            int result;
            if (textBox == null) return;
            if (int.TryParse(textBox.Text, out result) && result >= 0 && result < 1000)
            {
                Parameters.GridResolutionX = result;
                UpdateCurrentModel();
                DrawWorld();
            }
            else if (textBox.Text != string.Empty)
                textBox.Text = Parameters.GridResolutionX.ToString(CultureInfo.InvariantCulture);
        }

        private void GridResolutionYBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            int result;
            if (textBox == null) return;
            if (int.TryParse(textBox.Text, out result) && result >= 0 && result < 1000)
            {
                Parameters.GridResolutionY = result;
                UpdateCurrentModel();
                DrawWorld();
            }
            else if (textBox.Text != string.Empty)
                textBox.Text = Parameters.GridResolutionY.ToString(CultureInfo.InvariantCulture);
        }
        private void IlluminanceBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            double result;
            if (textBox == null) return;
            if (double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out result) && result >= 0 && result <= 1)
            {
                Parameters.Illuminance = result;
                UpdateCurrentModel();
                DrawWorld();
            }
            else if (textBox.Text != string.Empty && textBox.Text[0] != '.' && textBox.Text[textBox.TextLength - 1] != '.')
                textBox.Text = Parameters.Illuminance.ToString(CultureInfo.InvariantCulture);
        }

        private void XAxisFactorBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            double result;
            if (textBox == null) return;
            if (double.TryParse(textBox.Text, out result) && result >= 0 && result < 1000)
            {
                if (Math.Abs(result) < Double.Epsilon) return;
                Parameters.XAxisFactor = result;
                UpdateCurrentModel();
                DrawWorld();
            }
            else if (textBox.Text != string.Empty)
                textBox.Text = Parameters.XAxisFactor.ToString(CultureInfo.InvariantCulture);
        }

        private void YAxisFactorBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            double result;
            if (textBox == null) return;
            if (double.TryParse(textBox.Text, out result) && result >= 0 && result < 1000)
            {
                if (Math.Abs(result) < Double.Epsilon) return;
                Parameters.YAxisFactor = result;
                UpdateCurrentModel();
                DrawWorld();
            }
            else if (textBox.Text != string.Empty)
                textBox.Text = Parameters.YAxisFactor.ToString(CultureInfo.InvariantCulture);
        }

        private void ZAxisFactorBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            double result;
            if (textBox == null) return;
            if (double.TryParse(textBox.Text, out result) && result >= 0 && result < 1000)
            {
                if (Math.Abs(result) < Double.Epsilon) return;
                Parameters.ZAxisFactor = result;
                UpdateCurrentModel();
                DrawWorld();
            }
            else if (textBox.Text != string.Empty)
                textBox.Text = Parameters.ZAxisFactor.ToString(CultureInfo.InvariantCulture);
        }

        private void PixelMaxSizeBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            int result;
            if (textBox == null) return;
            if (int.TryParse(textBox.Text, out result) && result >= 0 && result < 100)
            {
                if (result == 0) return;
                Parameters.PixelMaxSize = result;
                UpdateCurrentModel();
                DrawWorld();
            }
            else if (textBox.Text != string.Empty)
                textBox.Text = Parameters.PixelMaxSize.ToString(CultureInfo.InvariantCulture);
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            DrawWorld();
        }
        #endregion
    }
}
