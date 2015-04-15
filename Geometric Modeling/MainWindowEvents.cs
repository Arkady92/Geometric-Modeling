﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Mathematics;
using Models;
using Point = Models.Point;

namespace Geometric_Modeling
{
    public partial class MainWindow
    {
        private bool _lockChain;
        #region Mouse events

        private void WorldPanel_MouseDown(object sender, MouseEventArgs e)
        {
            _mousePositionX = e.X;
            _mousePositionY = e.Y;

            // Check if selection is enabled
            if (Models.Cursor.ModelHandled) return;
            if (_currentOperation != Operation.Selection)
            {
                _operationInProgress = true;
                return;
            }
            foreach (var geometricModel in _hiddenModels)
            {
                if (FindAppropriateObjectOnScene(geometricModel))
                    return;
            }

            foreach (var geometricModel in ObjectsList.Items)
            {
                if (FindAppropriateObjectOnScene(geometricModel))
                    return;
            }
        }

        private bool FindAppropriateObjectOnScene(object geometricModel)
        {
            if (geometricModel is Models.Cursor) return false;
            var model = geometricModel as GeometricModel;
            if (model == null) return false;
            var position = model.GetCurrentPosition();
            var screenPosition = (_currentProjectionMatrix * position) * Parameters.WorldPanelSizeFactor;
            if (!(Vector4.Distance2(
                screenPosition.X, screenPosition.Y,
                _mousePositionX - Parameters.WorldPanelWidth / 2,
                _mousePositionY - Parameters.WorldPanelHeight / 2)
                  < Parameters.MouseInaccuracy)) return false;

            Models.Cursor.Instance.TranslateToPosition(position);
            Models.Cursor.Instance.UpdateModel();
            DrawWorld();
            UpdateTextBoxes();
            ObjectsList.SelectedItems.Clear();
            ObjectsList.SelectedItem = geometricModel;
            ObjectsList_MouseClick(model, null);
            return true;
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
            var geometricObject = new Torus(Models.Cursor.GetCurrentPosition());
            _models.Add(geometricObject);
            _hiddenModels.AddRange(geometricObject.GetChildren());
            ObjectsList.Items.Add(geometricObject);
            _enableAnimations = false;
            DrawWorld();
        }

        private void EllipsoidButton_Click(object sender, EventArgs e)
        {
            var geometricObject = new Ellipsoid(new Vector4(0, 0, 0));
            _models.Add(geometricObject);
            ObjectsList.Items.Add(geometricObject);
            _forceStaticGraphics = true;
            _enableAnimations = true;
            DrawWorld();
        }

        private void PointButton_Click(object sender, EventArgs e)
        {
            var geometricObject = new Point(Models.Cursor.GetCurrentPosition());
            //geometricObject.TranslateToPosition(Models.Cursor.GetCurrentPosition());
            _models.Add(geometricObject);
            ObjectsList.Items.Add(geometricObject);
            if (ObjectsList.SelectedItem is BezierCurveC2)
            {
                ObjectsList.SelectedItems.Add(geometricObject);
                BezierCurveC2Button_Click(null, null);
            }
            else if (ObjectsList.SelectedItem is BezierCurve)
            {
                ObjectsList.SelectedItems.Add(geometricObject);
                BezierCurveButton_Click(null, null);
            }
            DrawWorld();
        }

        private void BezierCurveButton_Click(object sender, EventArgs e)
        {
            bool polygonChainEnabled;
            var points = CollectPoints(out polygonChainEnabled);
            if (points != null && points.Count > 0)
            {
                var bezierCurve = new BezierCurve(points.Distinct(), Models.Cursor.GetCurrentPosition());
                //bezierCurve.TranslateToPosition(Models.Cursor.GetCurrentPosition());
                if (bezierCurve.ChainEnabled != polygonChainEnabled)
                    bezierCurve.TogglePolygonialChain();
                _models.Add(bezierCurve);
                ObjectsList.Items.Add(bezierCurve);
                ObjectsList.SelectedItems.Clear();
                ObjectsList.SelectedItem = bezierCurve;
                DrawWorld();
                return;
            }
            var geometricObject = new BezierCurve(new Vector4(0, 0, 0));
            geometricObject.TranslateToPosition(Models.Cursor.GetCurrentPosition());
            foreach (var child in geometricObject.GetChildren())
                ObjectsList.Items.Add(child);
            _models.Add(geometricObject);
            ObjectsList.Items.Add(geometricObject);
            DrawWorld();
        }

        private void BezierCurveC2Button_Click(object sender, EventArgs e)
        {
            bool polygonChainEnabled;
            var points = CollectPoints(out polygonChainEnabled);
            if (points != null && points.Count > 0)
            {
                var bezierCurve = new BezierCurveC2(points.Distinct(), Models.Cursor.GetCurrentPosition(), _hiddenModels);
                //bezierCurve.TranslateToPosition(Models.Cursor.GetCurrentPosition());
                if(bezierCurve.ChainEnabled != polygonChainEnabled)
                    bezierCurve.TogglePolygonialChain();
                _models.Add(bezierCurve);
                ObjectsList.Items.Add(bezierCurve);
                ObjectsList.SelectedItems.Clear();
                ObjectsList.SelectedItem = bezierCurve;
                DrawWorld();
                return;
            }
            var geometricObject = new BezierCurveC2(new Vector4(0, 0, 0), _hiddenModels);
            geometricObject.TranslateToPosition(Models.Cursor.GetCurrentPosition());
            foreach (var child in geometricObject.GetChildren())
                ObjectsList.Items.Add(child);
            _models.Add(geometricObject);
            ObjectsList.Items.Add(geometricObject);
            DrawWorld();
        }

        private List<Point> CollectPoints(out bool polygonChainEnabled)
        {
            polygonChainEnabled = true;
            var count = ObjectsList.SelectedItems.Count;
            if (count > 0)
            {
                var curves = new List<BezierCurve>();
                var tmpPoints = new List<Point>();
                var points = new List<Point>();
                foreach (var element in ObjectsList.SelectedItems)
                {
                    if (element is Point)
                    {
                        tmpPoints.Add(element as Point);
                        _models.Remove(element as Point);
                    }
                    if (element is BezierCurve)
                    {
                        curves.Add(element as BezierCurve);
                        _models.Remove(element as BezierCurve);
                    }
                }
                if (curves.Count > 0)
                {
                    polygonChainEnabled = curves[0].ChainEnabled;
                    foreach (var curve in curves)
                    {
                        points.AddRange(curve.GetChildren().OfType<Point>().Select(child => child));
                        ObjectsList.Items.Remove(curve);
                        curve.RemoveModel();
                    }
                }
                points.AddRange(tmpPoints);
                return points;
            }
            return null;
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

        private void SelectionButton_Click(object sender, EventArgs e)
        {
            SwitchOperation(Operation.Selection);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            _enableStereoscopy = false;
            _forceStaticGraphics = false;
            _enableAnimations = false;
            DisableAllSettings();
            ObjectsList.Items.Clear();
            _models.Clear();
            Models.Cursor.Instance.TranslateToPosition(new Vector4(0, 0, 0));
            Models.Cursor.Instance.UpdateModel();
            _models.Add(Models.Cursor.Instance);
            UpdateTextBoxes();
            DrawWorld();
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
            RemoveSelectedObject();
        }

        private void ObjectsList_MouseClick(object sender, MouseEventArgs e)
        {
            if (Models.Cursor.ModelHandled) return;
            var item = ObjectsList.SelectedItem as GeometricModel;
            if (item == null)
            {
                item = sender as GeometricModel;
                if (item == null) return;
            }
            DisableAllSettings();
            foreach (var parameterBox in _modelsParameters[(item).Type])
            {
                parameterBox.Visible = true;
            }
            _enableAnimations = !(item is ParametricGeometricModel);
            _forceStaticGraphics = !(item is ParametricGeometricModel);

            Models.Cursor.Instance.TranslateToPosition(item.GetCurrentPosition());
            //Models.Cursor.XPosition = position.X;
            //Models.Cursor.YPosition = position.Y;
            //Models.Cursor.ZPosition = position.Z;
            Models.Cursor.Instance.UpdateModel();
            _lockChain = true;
            if (ObjectsList.SelectedItem is BezierCurve)
            {
                PolygonalChainCheckBox.Checked = (ObjectsList.SelectedItem as BezierCurve).ChainEnabled;
                ControlPointsRadioButton.Checked = (ObjectsList.SelectedItem as BezierCurve).ControlPointsEnabled;
                DeBoorsPointsRadioButton.Checked = !(ObjectsList.SelectedItem as BezierCurve).ControlPointsEnabled;
            }
            _lockChain = false;
            DrawWorld();
            UpdateTextBoxes();
        }

        private void ObjectsListLabel_Click(object sender, EventArgs e)
        {
            ObjectsList.ClearSelected();
            DisableAllSettings();
        }

        private void RemoveSelectedObject()
        {
            var item = ObjectsList.SelectedItem;
            if (item != null)
            {
                RemovedObject(item as GeometricModel);
            }
            DisableAllSettings();
            DrawWorld();
        }

        private void RemovedObject(GeometricModel model)
        {
            if (model is ParametricGeometricModel)
            {
                var parametricModel = model as ParametricGeometricModel;
                if (parametricModel.ReturnChildrenOnRemove)
                {
                    _models.AddRange(parametricModel.GetChildren());
                    foreach (var child in parametricModel.GetChildren())
                    {
                        if (!ObjectsList.Items.Contains(child))
                            ObjectsList.Items.Add(child);
                    }
                }
                parametricModel.RemoveModel();
            }
            _models.Remove(model);
            ObjectsList.Items.Remove(model);
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

        private void ControlPointsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null || !radioButton.Checked) return;
            var item = ObjectsList.SelectedItem as BezierCurve;
            if (!_lockChain && item != null)
                item.SetControlPointsEnablability(true);
            DrawWorld();
        }

        private void DeBoorsPointsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null || !radioButton.Checked) return;
            var item = ObjectsList.SelectedItem as BezierCurve;
            if (!_lockChain && item != null)
                item.SetControlPointsEnablability(false);
            DrawWorld();
        }

        private void PolygonalChainCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null) return;
            var item = ObjectsList.SelectedItem as BezierCurve;
            if (!_lockChain && item != null)
                item.TogglePolygonialChain();
            DrawWorld();
        }


        private void SaveButton_Click(object sender, System.EventArgs e)
        {
            SaveScene();
        }

        private void LoadButton_Click(object sender, System.EventArgs e)
        {
            LoadScene();
        }

        #endregion

        #region key evenets

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    Models.Cursor.YPosition = Models.Cursor.YPosition + Parameters.CursorMoveValue;
                    break;
                case Keys.S:
                    Models.Cursor.YPosition = Models.Cursor.YPosition - Parameters.CursorMoveValue;
                    break;
                case Keys.A:
                    Models.Cursor.XPosition = Models.Cursor.XPosition - Parameters.CursorMoveValue;
                    break;
                case Keys.D:
                    Models.Cursor.XPosition = Models.Cursor.XPosition + Parameters.CursorMoveValue;
                    break;
                case Keys.Q:
                    Models.Cursor.ZPosition = Models.Cursor.ZPosition - Parameters.CursorMoveValue;
                    break;
                case Keys.Z:
                    Models.Cursor.ZPosition = Models.Cursor.ZPosition + Parameters.CursorMoveValue;
                    break;
                case Keys.Space:
                    e.Handled = true;
                    if (!Models.Cursor.ModelHandled)
                    {
                        var currentItem = ObjectsList.SelectedItem as GeometricModel;
                        if (currentItem != null && TryHandleModel(currentItem)) break;
                        bool result = false;
                        foreach (var geometricModel in ObjectsList.Items)
                        {
                            if (geometricModel is Models.Cursor) continue;
                            if (TryHandleModel(geometricModel as GeometricModel))
                            {
                                result = true;
                                break;
                            }
                        }
                        if (!result)
                        {
                            foreach (var geometricModel in _hiddenModels)
                            {
                                if (geometricModel is Models.Cursor) continue;
                                if (TryHandleModel(geometricModel))
                                    break;
                            }
                        }
                    }
                    else
                        Models.Cursor.RemoveHandledModel();
                    break;
                case Keys.Delete:
                    if (!Models.Cursor.ModelHandled)
                    {
                        var currentItem = ObjectsList.SelectedItem as ParametricGeometricModel;
                        if (currentItem != null)
                        {
                            currentItem.RemoveModel();
                            if (!_models.Contains(currentItem))
                                _models.Add(currentItem);
                        }
                        else
                        {
                            foreach (var geometricModel in _hiddenModels)
                            {
                                if (FindAppropriateObjectOnScene(geometricModel))
                                {
                                    currentItem = geometricModel as ParametricGeometricModel;
                                    if (currentItem != null)
                                    {
                                        currentItem.RemoveModel();
                                        if (!_models.Contains(currentItem))
                                            _models.Add(currentItem);
                                        if (!ObjectsList.Items.Contains(currentItem))
                                            ObjectsList.Items.Add(currentItem);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    break;
                default:
                    return;
            }
            Models.Cursor.Instance.UpdateModel();
            DrawWorld();
            UpdateTextBoxes();
        }

        private bool TryHandleModel(GeometricModel model)
        {
            if (Vector4.Distance3(model.GetCurrentPosition(), Models.Cursor.GetCurrentPosition()) <
                Models.Cursor.CursorSize)
            {
                Models.Cursor.AddHandledModel(model);
                ObjectsList.SelectedItems.Clear();
                ObjectsList.SelectedItem = model;
                return true;
            }
            return false;
        }

        private void ObjectsList_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
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
            }
        }

        #endregion
    }
}
