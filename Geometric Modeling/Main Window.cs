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
        private List<GeometricModel> _models;
        private Dictionary<Operation, Button> _operationsButtons;
        private Operation _currentOperation;
        private bool _operationInProgress;
        private int _mousePositionX;
        private int _mousePositionY;
        private Matrix _currentMatrix;

        public MainWindow()
        {
            InitializeComponent();
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            GridResolutionXBox.Text = Parameters.GridResolutionX.ToString(CultureInfo.InvariantCulture);
            GridResolutionYBox.Text = Parameters.GridResolutionY.ToString(CultureInfo.InvariantCulture);
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
            _currentOperation = Operation.None;
            _currentMatrix = OperationsMatrices.Projection(5) * OperationsMatrices.Identity();
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

        private void DrawWorld()
        {
            var graphics = WorldPanel.CreateGraphics();
            graphics.Clear(Color.White);
            if (_models == null) return;
            graphics.TranslateTransform((float)(WorldPanel.Size.Width * 0.5), (float)(WorldPanel.Size.Height * 0.5));
            foreach (var geometricModel in _models)
            {
                geometricModel.Draw(graphics, _currentMatrix);
            }
        }

        private void UpdateMeshes()
        {
            if (_models == null) return;
            foreach (var geometricModel in _models)
            {
                geometricModel.UpdateMesh();
            }
        }

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
            var delta = e.Y - _mousePositionY;
            const double positiveScale = 1.1;
            const double negativeScale = 0.91;
            const double factor = 0.005;

            switch (_currentOperation)
            {
                case Operation.TranslationX:
                    _currentMatrix = OperationsMatrices.Translation(delta * factor, 0, 0) * _currentMatrix;
                    DrawWorld();
                    break;
                case Operation.TranslationY:
                    _currentMatrix = OperationsMatrices.Translation(0, delta * factor, 0) * _currentMatrix;
                    DrawWorld();
                    break;
                case Operation.TranslationZ:
                    _currentMatrix = OperationsMatrices.Translation(0, 0, delta * factor) * _currentMatrix;
                    DrawWorld();
                    break;
                case Operation.RotationX:
                    _currentMatrix = OperationsMatrices.RotationX(delta * factor) * _currentMatrix;
                    DrawWorld();
                    break;
                case Operation.RotationY:
                    _currentMatrix = OperationsMatrices.RotationY(delta * factor) * _currentMatrix;
                    DrawWorld();
                    break;
                case Operation.RotationZ:
                    _currentMatrix = OperationsMatrices.RotationZ(delta * factor) * _currentMatrix;
                    DrawWorld();
                    break;
                case Operation.Scale:
                    if (delta < 0)
                        _currentMatrix = OperationsMatrices.Scale(positiveScale) * _currentMatrix;
                    if (delta > 0)
                        _currentMatrix = OperationsMatrices.Scale(negativeScale) * _currentMatrix;
                    DrawWorld();
                    break;
            }
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

        private void ObjectsList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var item = ObjectsList.SelectedItem;
            if (item != null)
            {
                _models.Remove(item as GeometricModel);
                ObjectsList.Items.Remove(ObjectsList.SelectedItem);
            }
            DrawWorld();
        }
        #endregion

        #region Contents change events
        private void GridResolutionXBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            int result;
            if (textBox == null) return;
            if (int.TryParse(textBox.Text, out result))
            {
                Parameters.GridResolutionX = result;
                UpdateMeshes();
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
            if (int.TryParse(textBox.Text, out result))
            {
                Parameters.GridResolutionY = result;
                UpdateMeshes();
                DrawWorld();
            }
            else if (textBox.Text != string.Empty)
                textBox.Text = Parameters.GridResolutionY.ToString(CultureInfo.InvariantCulture);
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            DrawWorld();
        }
        #endregion
    }
}
