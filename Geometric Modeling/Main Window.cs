using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Mathematics;
using Models;
using Point = Models.Point;

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
        private Matrix _currentProjectionMatrix;
        private Matrix _currentStereoscopyLeftMatrix;
        private Matrix _currentStereoscopyRightMatrix;
        private Bitmap _backBuffer;
        private bool _forceStaticGraphics;
        private bool _enableAnimations;
        private bool _enableStereoscopy;
        private const double ProjectionR = 11;
        private const double ProjectionE = 1;
        private bool _enableAdditiveColorBlending;

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
                    GridResolutionYLabel, StereoscopyCheckBox, AdditiveColorBlendingCheckBox}},
                {ModelType.Ellipsoid, new List<Control> {IlluminanceBox, XAxisFactorBox, YAxisFactorBox, ZAxisFactorBox, PixelMaxSizeBox, 
                    IlluminanceLabel, XAxisFactorLabel, YAxisFactorLabel, ZAxisFactorLabel, PixelMaxSizeLabel}},
                    {ModelType.Point, new List<Control>{StereoscopyCheckBox, AdditiveColorBlendingCheckBox}},
                    {ModelType.BezierCurve, new List<Control>{StereoscopyCheckBox, AdditiveColorBlendingCheckBox}}
            };
            DisableAllSettings();
            _currentOperation = Operation.None;
            _currentProjectionMatrix = OperationsMatrices.Projection(200);
            _currentStereoscopyLeftMatrix = OperationsMatrices.StereoscopyLeft(ProjectionR, ProjectionE);
            _currentStereoscopyRightMatrix = OperationsMatrices.StereoscopyRight(ProjectionR, ProjectionE);
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
            graphics.Clear(Color.Black);
            if (_models == null) return;
            graphics.TranslateTransform((float)(WorldPanel.Size.Width * 0.5), (float)(WorldPanel.Size.Height * 0.5));
            foreach (var geometricModel in _models)
            {
                if (geometricModel is ImplicitGeometricModel && _enableAnimations)
                {
                    var pixelSize = Parameters.PixelMaxSize;
                    while (pixelSize > 1)
                    {
                        graphics.Clear(Color.Black);
                        (geometricModel as ImplicitGeometricModel).Draw(graphics, pixelSize);
                        if (Parameters.GridResolutionX * Parameters.GridResolutionY < 5000 && !_forceStaticGraphics)
                            WorldPanel.Image = _backBuffer;
                        pixelSize--;
                    }
                }
            }
            foreach (var geometricModel in _models)
            {
                if (geometricModel is ParametricGeometricModel && _enableStereoscopy)
                    (geometricModel as ParametricGeometricModel).DrawStereoscopy(graphics, _currentStereoscopyLeftMatrix,
                        _currentStereoscopyRightMatrix, _enableAdditiveColorBlending);
                else
                    geometricModel.Draw(graphics, _currentProjectionMatrix);
            }

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
            //_enableAnimations = false;
            _mousePositionX = e.X;
            _mousePositionY = e.Y;
        }

        private void WorldPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_operationInProgress) return;
            var deltaX = e.X - _mousePositionX;
            var deltaY = e.Y - _mousePositionY;
            switch (_currentOperation)
            {
                case Operation.TranslationX:
                    if (ObjectsList.SelectedItem == null)
                        foreach (var geometricModel in _models)
                            geometricModel.Translate(deltaX, 0, 0);
                    else
                    {
                        var model = ObjectsList.SelectedItem as GeometricModel;
                        if (model != null) model.Translate(deltaX, 0, 0);
                    }
                    DrawWorld();
                    break;
                case Operation.TranslationY:
                    if (ObjectsList.SelectedItem == null)
                        foreach (var geometricModel in _models)
                            geometricModel.Translate(0, deltaY, 0);
                    else
                    {
                        var model = ObjectsList.SelectedItem as GeometricModel;
                        if (model != null) model.Translate(0, deltaY, 0);
                    }
                    DrawWorld();
                    break;
                case Operation.TranslationZ:
                    if (ObjectsList.SelectedItem == null)
                        foreach (var geometricModel in _models)
                            geometricModel.Translate(0, 0, deltaY);
                    else
                    {
                        var model = ObjectsList.SelectedItem as GeometricModel;
                        if (model != null) model.Translate(0, 0, deltaY);
                    }
                    DrawWorld();
                    break;
                case Operation.RotationX:
                    if (ObjectsList.SelectedItem == null)
                        foreach (var geometricModel in _models)
                            geometricModel.Rotate(-deltaY, 0, 0);
                    else
                    {
                        var model = ObjectsList.SelectedItem as GeometricModel;
                        if (model != null) model.Rotate(-deltaY, 0, 0);
                    }
                    DrawWorld();
                    break;
                case Operation.RotationY:
                    if (ObjectsList.SelectedItem == null)
                        foreach (var geometricModel in _models)
                            geometricModel.Rotate(0, deltaX, 0);
                    else
                    {
                        var model = ObjectsList.SelectedItem as GeometricModel;
                        if (model != null) model.Rotate(0, deltaX, 0);
                    }
                    DrawWorld();
                    break;
                case Operation.RotationZ:
                    if (ObjectsList.SelectedItem == null)
                        foreach (var geometricModel in _models)
                            geometricModel.Rotate(0, 0, deltaX);
                    else
                    {
                        var model = ObjectsList.SelectedItem as GeometricModel;
                        if (model != null) model.Rotate(0, 0, deltaX);
                    }
                    DrawWorld();
                    break;
                case Operation.Scale:
                    if (ObjectsList.SelectedItem == null)
                        foreach (var geometricModel in _models)
                            geometricModel.Scale(deltaY);
                    else
                    {
                        var model = ObjectsList.SelectedItem as GeometricModel;
                        if (model != null) model.Scale(deltaY);
                    }
                    DrawWorld();
                    break;
            }
            _mousePositionX = e.X;
            _mousePositionY = e.Y;
        }

        private void WorldPanel_MouseUp(object sender, MouseEventArgs e)
        {
            _operationInProgress = false;
            //_enableAnimations = true;
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

        private void PointButton_Click(object sender, EventArgs e)
        {
            var geometricObject = new Point(0, 0, 0);
            _models.Add(geometricObject);
            ObjectsList.Items.Add(geometricObject);
            DrawWorld();
        }

        private void BezierCurveButton_Click(object sender, EventArgs e)
        {
            var geometricObject = new BezierCurve();
            _models.Add(geometricObject);
            ObjectsList.Items.Add(geometricObject);
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
            RemoveObject();
        }

        private void ObjectsList_MouseClick(object sender, MouseEventArgs e)
        {
            var item = ObjectsList.SelectedItem as GeometricModel;
            if (item == null) return;
            DisableAllSettings();
            foreach (var parameterBox in _modelsParameters[(item).Type])
            {
                parameterBox.Visible = true;
            }
            _enableAnimations = !(item is ParametricGeometricModel);
            _forceStaticGraphics = !(item is ParametricGeometricModel);
        }

        private void ObjectsList_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    var item = ObjectsList.SelectedItem as GeometricModel;
                    if (item == null) return;
                    var form = new RenameForm();
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        item.SetCustomName(form.EnteredText);
                        ObjectsList.Items[ObjectsList.SelectedIndex] = ObjectsList.SelectedItem;
                    }
                    break;
                case Keys.Delete:
                    RemoveObject();
                    break;
                case Keys.Space:
                    var count = ObjectsList.SelectedItems.Count;
                    if (count < 2) break;
                    var points = new List<Point>();
                    for (int i = count - 1; i >= 0; i--)
                    {
                        if (!(ObjectsList.SelectedItems[i] is Point)) continue;
                        points.Add(ObjectsList.SelectedItems[i] as Point);
                        _models.Remove(points[-i + count - 1]);
                        ObjectsList.Items.Remove(ObjectsList.SelectedItems[i]);
                    }
                    points.Reverse();
                    if(points.Count < 2) break;
                    var bezierCurve = new BezierCurve(points);
                    _models.Add(bezierCurve);
                    ObjectsList.Items.Add(bezierCurve);
                    ObjectsList.SelectedItem = bezierCurve;
                    DrawWorld();
                    break;
            }
        }

        private void ObjectsListLabel_Click(object sender, EventArgs e)
        {
            ObjectsList.ClearSelected();
            DisableAllSettings();
        }

        private void RemoveObject()
        {
            var item = ObjectsList.SelectedItem;
            if (item != null)
            {
                if (item is ParametricGeometricModel)
                    (item as ParametricGeometricModel).RemoveModel();
                _models.Remove(item as GeometricModel);
                ObjectsList.Items.Remove(ObjectsList.SelectedItem);
            }
            DisableAllSettings();
            DrawWorld();
        }

        private void DisableAllSettings()
        {
            foreach (var modelsParameter in _modelsParameters.Values)
            {
                foreach (var parameterBox in modelsParameter)
                {
                    parameterBox.Visible = false;
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
            if (double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out result) && result >= 0 && result <= 100)
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

        private void StereoscopyChackBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null) return;
            _enableStereoscopy = checkBox.Checked;
            DrawWorld();
        }

        private void AdditiveColorBlendingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null) return;
            _enableAdditiveColorBlending = checkBox.Checked;
            DrawWorld();
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            Parameters.WorldPanelWidth = WorldPanel.Width;
            Parameters.WorldPanelHeight = WorldPanel.Height;
            DrawWorld();
        }
        #endregion
    }
}
