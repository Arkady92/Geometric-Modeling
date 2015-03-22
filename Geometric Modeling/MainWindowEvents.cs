using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Models;
using Point = Models.Point;

namespace Geometric_Modeling
{
    public partial class MainWindow
    {
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
            UpdateTextBoxes();
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
            var geometricObject = new Torus(Models.Cursor.GetPosition());
            _models.Add(geometricObject);
            ObjectsList.Items.Add(geometricObject);
            _enableAnimations = false;
            DrawWorld();
        }

        private void EllipsoidButton_Click(object sender, EventArgs e)
        {
            var geometricObject = new Ellipsoid(Models.Cursor.GetPosition());
            _models.Add(geometricObject);
            ObjectsList.Items.Add(geometricObject);
            _forceStaticGraphics = true;
            _enableAnimations = true;
            DrawWorld();
        }

        private void PointButton_Click(object sender, EventArgs e)
        {
            var geometricObject = new Point(Models.Cursor.GetPosition());
            _models.Add(geometricObject);
            ObjectsList.Items.Add(geometricObject);
            DrawWorld();
        }

        private void BezierCurveButton_Click(object sender, EventArgs e)
        {
            var count = ObjectsList.SelectedItems.Count;
            if (count >= 2)
            {
                var points = new List<Point>();
                int idx = 0;
                for (var i = count - 1; i >= 0; i--)
                {
                    if (!(ObjectsList.SelectedItems[i] is Point)) continue;
                    points.Add(ObjectsList.SelectedItems[i] as Point);
                    _models.Remove(points[idx++]);
                    ObjectsList.Items.Remove(ObjectsList.SelectedItems[i]);
                }
                points.Reverse();
                if (points.Count >= 2)
                {
                    var bezierCurve = new BezierCurve(points, Models.Cursor.GetPosition());
                    _models.Add(bezierCurve);
                    ObjectsList.Items.Add(bezierCurve);
                    ObjectsList.SelectedItem = bezierCurve;
                    DrawWorld();
                    return;
                }
            }
            var geometricObject = new BezierCurve(Models.Cursor.GetPosition());
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
            Models.Cursor.SetPosition(item.GetCurrentPosition());
            Models.Cursor.Instance.UpdateModel();
            DrawWorld();
            UpdateTextBoxes();
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
            foreach (var control in _modelsParameters[ModelType.Cursor])
            {
                control.Visible = true;
            }
        }
        #endregion

        #region key evenets

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            const double epsilon = 0.001;
            const double moveFactor = 0.05;
            switch (e.KeyCode)
            {
                case Keys.W:
                    Models.Cursor.YPosition -= moveFactor;
                    break;
                case Keys.S:
                    Models.Cursor.YPosition += moveFactor;
                    break;
                case Keys.A:
                    Models.Cursor.XPosition -= moveFactor;
                    break;
                case Keys.D:
                    Models.Cursor.XPosition += moveFactor;
                    break;
                case Keys.Q:
                    Models.Cursor.ZPosition -= moveFactor;
                    break;
                case Keys.Z:
                    Models.Cursor.ZPosition += moveFactor;
                    break;
                case Keys.Space:
                    return;
                default:
                    return;
            }
            if (Math.Abs(Models.Cursor.XPosition) < epsilon)
                Models.Cursor.XPosition = 0;
            if (Math.Abs(Models.Cursor.YPosition) < epsilon)
                Models.Cursor.YPosition = 0;
            if (Math.Abs(Models.Cursor.ZPosition) < epsilon)
                Models.Cursor.ZPosition = 0;
            Models.Cursor.Instance.UpdateModel();
            DrawWorld();
            UpdateTextBoxes();
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
            }
        }

        #endregion
    }
}
