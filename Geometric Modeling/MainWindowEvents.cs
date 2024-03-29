﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using Mathematics;
using Models;
using Point = Models.Point;

namespace Geometric_Modeling
{
    public partial class MainWindow
    {
        private bool _lockModification;
        private bool _intersectionEnabled;
        private Intersector _intersector;
        private bool _intersectionSelectEnabled;
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
            var size = Math.Max(Parameters.WorldPanelWidth, Parameters.WorldPanelHeight) / 2;
            if (_intersectionEnabled && _intersectionSelectEnabled)
            {
                var intersectionResult = _intersector.PerformIntersection(
                    (_mousePositionX - Parameters.WorldPanelWidth / 2.0) / size,
                    -(_mousePositionY - Parameters.WorldPanelHeight / 2.0) / size);
                if (intersectionResult == null)
                {
                    MessageBox.Show(@"Intersection not detected");
                    return;
                }
                _models.Add(intersectionResult);
                ObjectsList.Items.Add(intersectionResult);
                _parametrizationWindow.DrawUVParametrization(intersectionResult.GetUVPoints());
                _parametrizationWindow.DrawSTParametrization(intersectionResult.GetSTPoints());
                DrawWorld();
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
            if (_multiselectEnabled)
            {
                ObjectsList.SelectedItems.Add(geometricModel);
            }
            else
            {
                ObjectsList.SelectedItems.Clear();
                ObjectsList.SelectedItem = geometricModel;
            }
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
                        foreach (var geometricModel in _models.Where(model => model.IsRemovableFromModel))
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
                        foreach (var geometricModel in _models.Where(model => model.IsRemovableFromModel))
                            geometricModel.Translate(0, -deltaY, 0);
                    else
                    {
                        var model = ObjectsList.SelectedItem as GeometricModel;
                        if (model != null) model.Translate(0, -deltaY, 0);
                    }
                    DrawWorld();
                    break;
                case Operation.TranslationZ:
                    if (ObjectsList.SelectedItem == null)
                        foreach (var geometricModel in _models.Where(model => model.IsRemovableFromModel))
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
                        foreach (var geometricModel in _models.Where(model => model.IsRemovableFromModel))
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
                        foreach (var geometricModel in _models.Where(model => model.IsRemovableFromModel))
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
                        foreach (var geometricModel in _models.Where(model => model.IsRemovableFromModel))
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
                        foreach (var geometricModel in _models.Where(model => model.IsRemovableFromModel))
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

        private void RoughButton_Click(object sender, System.EventArgs e)
        {
            if (pathCreator == null) pathCreator = new PathCreator(_models.OfType<BezierSurface>().ToList(), _models);
            pathCreator.GenerateRoughPathes();
            DrawWorld();
            MessageBox.Show(@"File generated.");
        }

        private void EnvelopeButton_Click(object sender, System.EventArgs e)
        {
            if (pathCreator == null) pathCreator = new PathCreator(_models.OfType<BezierSurface>().ToList(), _models);
            pathCreator.GenerateEnvelopePathes();
            DrawWorld();
            MessageBox.Show(@"File generated.");
        }

        private void FlatButton_Click(object sender, System.EventArgs e)
        {
            if (pathCreator == null) pathCreator = new PathCreator(_models.OfType<BezierSurface>().ToList(), _models);
            pathCreator.GenerateFlatPathes();
            DrawWorld();
            MessageBox.Show(@"File generated.");
        }

        private void AccurateButton_Click(object sender, System.EventArgs e)
        {
            if (pathCreator == null) pathCreator = new PathCreator(_models.OfType<BezierSurface>().ToList(), _models);
            pathCreator.GenerateAccuratePathes();
            DrawWorld();
            MessageBox.Show(@"File generated.");
        }
        private void button1_Click(object sender, System.EventArgs e)
        {
            if (pathCreator == null) pathCreator = new PathCreator(_models.OfType<BezierSurface>().ToList(), _models);
            pathCreator.GenerateSignaturePathes();
            DrawWorld();
            MessageBox.Show(@"File generated.");
        }

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
            else if (ObjectsList.SelectedItem is InterpolationCurve)
            {
                ObjectsList.SelectedItems.Add(geometricObject);
                InterpolationCurveButton_Click(null, null);
            }
            else if (ObjectsList.SelectedItem is BezierCurve)
            {
                ObjectsList.SelectedItems.Add(geometricObject);
                BezierCurveButton_Click(null, null);
            }
            else
                DrawWorld();
        }

        private void BezierCurveButton_Click(object sender, EventArgs e)
        {
            bool polygonChainEnabled;
            bool controlPointsEnabled;
            var points = CollectPoints(out polygonChainEnabled, out controlPointsEnabled);
            if (points != null && points.Count > 0)
            {
                var bezierCurve = new BezierCurve(points.Distinct(), Models.Cursor.GetCurrentPosition(),
                    polygonialChainEnabled: polygonChainEnabled);
                bezierCurve.ControlPointsEnabled = true;
                _lockModification = true;
                PolygonalChainCheckBox.Checked = polygonChainEnabled;
                ControlPointsRadioButton.Checked = true;
                _lockModification = false;
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
            bool controlPointsEnabled;
            var points = CollectPoints(out polygonChainEnabled, out controlPointsEnabled);
            if (points != null && points.Count > 0)
            {
                var bezierCurve = new BezierCurveC2(points.Distinct(), Models.Cursor.GetCurrentPosition(), _hiddenModels,
                    polygonChainEnabled, controlPointsEnabled);
                _lockModification = true;
                PolygonalChainCheckBox.Checked = polygonChainEnabled;
                ControlPointsRadioButton.Checked = controlPointsEnabled;
                _lockModification = false;
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

        private void InterpolationCurveButton_Click(object sender, EventArgs e)
        {
            bool chordParametrizationEnabled;
            var points = CollectPoints(out chordParametrizationEnabled);
            if (points != null && points.Count > 0)
            {
                var curve = new InterpolationCurve(points.Distinct(), Models.Cursor.GetCurrentPosition(), chordParametrizationEnabled);
                _models.Add(curve);
                ObjectsList.Items.Add(curve);
                _lockModification = true;
                ChordParametrizationRadioButton.Checked = chordParametrizationEnabled;
                NormalParametrizationRadioButton.Checked = !chordParametrizationEnabled;
                _lockModification = false;
                ObjectsList.SelectedItems.Clear();
                ObjectsList.SelectedItem = curve;
                DrawWorld();
                return;
            }
            var geometricObject = new InterpolationCurve(new Vector4(0, 0, 0));
            geometricObject.TranslateToPosition(Models.Cursor.GetCurrentPosition());
            foreach (var child in geometricObject.GetChildren())
                ObjectsList.Items.Add(child);
            _models.Add(geometricObject);
            ObjectsList.Items.Add(geometricObject);
            DrawWorld();
        }

        private void BezierSurfaceC0Button_Click(object sender, EventArgs e)
        {
            var form = new SurfaceInitWindow();
            var result = form.ShowDialog();
            if (result != DialogResult.OK) return;
            var geometricObject = new BezierSurface(Vector4.Zero(), form.SurfaceWidth, form.SurfaceHeight,
                form.SurfacePatchesLengthCount, form.SurfacePatchesBreadthCount, form.IsSurfaceCylindrical)
                {Color = _colors[_colorIndex++ % _colors.Count]};
            geometricObject.TranslateToPosition(Models.Cursor.GetCurrentPosition());
            geometricObject.ChainEnabled = PolygonalChainCheckBox.Checked;
            foreach (var child in geometricObject.GetChildren())
                ObjectsList.Items.Add(child);
            _models.Add(geometricObject);
            ObjectsList.Items.Add(geometricObject);
            DrawWorld();
        }

        private void BezierSurfaceC2Button_Click(object sender, EventArgs e)
        {
            var form = new SurfaceInitWindow();
            var result = form.ShowDialog();
            if (result != DialogResult.OK) return;
            var geometricObject = new BezierSurfaceC2(Vector4.Zero(), form.SurfaceWidth, form.SurfaceHeight,
                form.SurfacePatchesLengthCount, form.SurfacePatchesBreadthCount, form.IsSurfaceCylindrical) 
                { Color = _colors[_colorIndex++ % _colors.Count] };
            geometricObject.TranslateToPosition(Models.Cursor.GetCurrentPosition());
            geometricObject.ChainEnabled = PolygonalChainCheckBox.Checked;
            foreach (var child in geometricObject.GetChildren())
                ObjectsList.Items.Add(child);
            _models.Add(geometricObject);
            ObjectsList.Items.Add(geometricObject);
            DrawWorld();

        }

        private void IntersectionButton_Click(object sender, EventArgs e)
        {
            if (_intersectionEnabled)
            {
                _intersectionEnabled = false;
                if (!_parametrizationWindow.IsDisposed)
                    _parametrizationWindow.Dispose();
                return;
            }
            var surfaces = ObjectsList.SelectedItems.OfType<BezierSurface>().ToList();
            if (surfaces.Count != 2)
            {
                MessageBox.Show(@"Wrong surfaces count");
                return;
            }
            _intersector = new Intersector(surfaces[0], surfaces[1]);
            _intersectionEnabled = true;

            if (_parametrizationWindow.IsDisposed)
                _parametrizationWindow = new ParametrizationWindow();
            _parametrizationWindow.Show();
        }

        private void GregorySurfaceButton_Click(object sender, EventArgs e)
        {
            var surfaces = ObjectsList.SelectedItems.OfType<BezierSurface>().ToList();
            if (surfaces.Count == 0)
            {
                var bsurf = new BezierSurface[3];
                bsurf[0] = new BezierSurface(Vector4.Zero(), 0.4, 0.4, 1, 1, false);
                bsurf[0].TranslateToPosition(new Vector4(-0.65, -0.05, 0));
                bsurf[1] = new BezierSurface(Vector4.Zero(), 0.4, 0.4, 1, 1, false);
                bsurf[1].TranslateToPosition(new Vector4(0.4, 0.35, 0));
                bsurf[2] = new BezierSurface(Vector4.Zero(), 0.4, 0.4, 1, 1, false);
                bsurf[2].TranslateToPosition(new Vector4(0.6, -0.35, 0));
                for (int i = 0; i < 3; i++)
                {
                    bsurf[i].ChainEnabled = PolygonalChainCheckBox.Checked;
                    foreach (var child in bsurf[i].GetChildren())
                        ObjectsList.Items.Add(child);
                    _models.Add(bsurf[i]);
                    ObjectsList.Items.Add(bsurf[i]);
                    surfaces.Add(bsurf[i]);
                }
                DrawWorld();
                return;
            }
            if (surfaces.Count != 3)
            {
                MessageBox.Show(@"Not enough surfaces selected");
                return;
            }
            if (
                !(surfaces[0].CollapsedSurfaces.Contains(surfaces[1]) &&
                  surfaces[0].CollapsedSurfaces.Contains(surfaces[2])
                  && surfaces[1].CollapsedSurfaces.Contains(surfaces[2])))
            {
                MessageBox.Show(@"Surfaces are not correctly connected");
                return;
            }
            var filler = new GapFiller(surfaces);
            _models.Add(filler);
            ObjectsList.Items.Add(filler);
            DrawWorld();
        }

        private List<Point> CollectPoints(out bool chordParametrizationEnabled)
        {
            bool dummy;
            return CollectPoints(out dummy, out dummy, out chordParametrizationEnabled);
        }

        private List<Point> CollectPoints(out bool polygonChainEnabled, out bool controlsPointsEnabled)
        {
            bool dummy;
            return CollectPoints(out polygonChainEnabled, out controlsPointsEnabled, out dummy);
        }

        private List<Point> CollectPoints(out bool polygonChainEnabled, out bool controlsPointsEnabled,
            out bool chordParametrizationEnabled)
        {
            polygonChainEnabled = true;
            controlsPointsEnabled = false;
            chordParametrizationEnabled = false;
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
                    controlsPointsEnabled = curves[0].ControlPointsEnabled;
                    if (curves[0] is InterpolationCurve)
                        chordParametrizationEnabled = (curves[0] as InterpolationCurve).ChordParametrizationEnabled;
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
            _enableAdditiveColorBlending = false;
            _lockModification = true;
            StereoscopyCheckBox.Checked = false;
            AdditiveColorBlendingCheckBox.Checked = false;
            ChordParametrizationRadioButton.Checked = true;
            ControlPointsRadioButton.Checked = true;
            _lockModification = false;
            _forceStaticGraphics = false;
            _enableAnimations = false;
            DisableAllSettings();
            ObjectsList.Items.Clear();
            _models.Clear();
            Models.Cursor.RemoveHandledModel();
            Models.Cursor.Instance.ResetOperationMatrix();
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
            _lockModification = true;
            if (ObjectsList.SelectedItem is BezierCurve)
            {
                PolygonalChainCheckBox.Checked = (ObjectsList.SelectedItem as BezierCurve).ChainEnabled;
                ControlPointsRadioButton.Checked = (ObjectsList.SelectedItem as BezierCurve).ControlPointsEnabled;
                DeBoorsPointsRadioButton.Checked = !(ObjectsList.SelectedItem as BezierCurve).ControlPointsEnabled;
            }
            _lockModification = false;
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
            var item = ObjectsList.SelectedItem as GeometricModel;
            if (item != null && item.IsRemovableFromScene)
            {
                if (item.IsRemovableFromModel)
                    RemovedObject(item);
                else
                    ObjectsList.Items.Remove(item);
            }
            DisableAllSettings();
            DrawWorld();
        }

        private void RemovedObject(GeometricModel model)
        {
            if (model is ParametricGeometricModel)
            {
                var parametricModel = model as ParametricGeometricModel;
                if (parametricModel is BezierSurface)
                    foreach (var child in parametricModel.GetChildren())
                    {
                        ObjectsList.Items.Remove(child);
                        _models.Remove(child);
                    }
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

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveScene();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            LoadScene();
        }

        private void CollapseButton_Click(object sender, EventArgs e)
        {
            var points = ObjectsList.SelectedItems.OfType<Point>().ToList();
            if (points.Count != 2)
            {
                MessageBox.Show(@"Not enough points selected");
                return;
            }
            var parent0 = points[0].GetOnlyParent() as BezierSurface;
            var parent1 = points[1].GetOnlyParent() as BezierSurface;
            if (parent0 == null || parent1 == null)
            {
                MessageBox.Show(@"Points are not a part of the surface or are already collapsed");
                return;
            }
            if (parent0 == parent1)
            {
                MessageBox.Show(@"Wrong points selected");
                return;
            }
            if (parent0.Type != parent1.Type)
            {
                MessageBox.Show(@"Surfaces are not of the same type");
                return;
            }
            if (!(points[0].IsCornerPointOfParent(parent0) && points[1].IsCornerPointOfParent(parent1)))
            {
                MessageBox.Show(@"Wrong points selected");
                return;
            }

            parent0.AddCollapsedSurface(parent1);
            parent1.AddCollapsedSurface(parent0);
            var position = points[0].SetCenterPosition(points[1]);
            points[1].ChangePoint(points[0]);
            points[0].CorrectPosition(position);
            parent0.AddCollapsedPoints(points[0]);
            parent1.AddCollapsedPoints(points[0]);

            ObjectsList.Items[ObjectsList.Items.IndexOf(points[1])] = points[0];
            DrawWorld();
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
                case Keys.E:
                    var extractionItem = ObjectsList.SelectedItem as InterpolationCurve;
                    if (extractionItem != null)
                    {
                        var curve = extractionItem.ExtractBezierCurve();
                        var bezierCurve = new BezierCurveC2(curve.GetChildren().OfType<Point>().Select(
                            point => new Point(point.GetCurrentPosition())), curve.GetCurrentPosition(), _hiddenModels,
                            curve.ChainEnabled, curve.ControlPointsEnabled);
                        _lockModification = true;
                        PolygonalChainCheckBox.Checked = curve.ChainEnabled;
                        ControlPointsRadioButton.Checked = curve.ControlPointsEnabled;
                        _lockModification = false;
                        if (!_models.Contains(bezierCurve))
                            _models.Add(bezierCurve);
                        foreach (var child in bezierCurve.GetChildren().OfType<Point>())
                            ObjectsList.Items.Add(child);
                        if (!ObjectsList.Items.Contains(bezierCurve))
                            ObjectsList.Items.Add(bezierCurve);
                        ObjectsList.SelectedItems.Clear();
                        ObjectsList.SelectedItem = bezierCurve;
                    }
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
                        if (currentItem != null && currentItem.IsRemovableFromScene)
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
                                    if (currentItem != null && currentItem.IsRemovableFromScene)
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
                case Keys.ControlKey:
                    _multiselectEnabled = true;
                    break;
                case Keys.ShiftKey:
                    _intersectionSelectEnabled = true;
                    break;
                default:
                    return;
            }
            Models.Cursor.Instance.UpdateModel();
            DrawWorld();
            UpdateTextBoxes();
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    _multiselectEnabled = false;
                    break;
                case Keys.ShiftKey:
                    _intersectionSelectEnabled = false;
                    break;
                default:
                    return;
            }
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
                        item.CustomName = form.EnteredText;
                        ObjectsList.Items[ObjectsList.SelectedIndex] = ObjectsList.SelectedItem;
                    }
                    break;
            }
        }

        #endregion
    }
}
