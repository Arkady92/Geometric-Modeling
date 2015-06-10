using System;
using System.Globalization;
using System.Windows.Forms;
using Mathematics;
using Models;

namespace Geometric_Modeling
{
    public partial class MainWindow
    {
        #region Contents change events

        private void SurfaceGridResolutionXBox_TextChanged(object sender, EventArgs e)
        {
            if (!_enableWorldDrawing) return;
            var textBox = sender as TextBox;
            int result;
            if (textBox == null) return;
            if (int.TryParse(textBox.Text, out result) && result > 1 && result < 1000)
            {
                Parameters.SurfaceGridResolutionX = result;
                UpdateCurrentModel();
                DrawWorld();
            }
            else if (textBox.Text != string.Empty && textBox.Text[0] != '0' && textBox.Text[0] != '1')
                textBox.Text = Parameters.SurfaceGridResolutionX.ToString(CultureInfo.InvariantCulture);
        }

        private void SurfaceGridResolutionYBox_TextChanged(object sender, EventArgs e)
        {
            if (!_enableWorldDrawing) return;
            var textBox = sender as TextBox;
            int result;
            if (textBox == null) return;
            if (int.TryParse(textBox.Text, out result) && result > 1 && result < 1000)
            {
                Parameters.SurfaceGridResolutionY = result;
                UpdateCurrentModel();
                DrawWorld();
            }
            else if (textBox.Text != string.Empty && textBox.Text[0] != '0' && textBox.Text[0] != '1')
                textBox.Text = Parameters.SurfaceGridResolutionY.ToString(CultureInfo.InvariantCulture);
        }
        
        private void GridResolutionXBox_TextChanged(object sender, EventArgs e)
        {
            if(!_enableWorldDrawing) return;
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
            if (!_enableWorldDrawing) return;
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
            if (!_enableWorldDrawing) return;
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
            if (!_enableWorldDrawing) return;
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
            if (!_enableWorldDrawing) return;
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
            if (!_enableWorldDrawing) return;
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
            if (!_enableWorldDrawing) return;
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

        private void CursorXBox_TextChanged(object sender, EventArgs e)
        {
            if (!_enableWorldDrawing) return;
            var textBox = sender as TextBox;
            double result;
            if (textBox == null) return;
            if (double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out result)
                && result >= -10 && result <= 10)
            {
                var position = Models.Cursor.GetCurrentPosition();
                position.X = result;
                Models.Cursor.Instance.TranslateToPosition(position);
                Models.Cursor.Instance.UpdateModel();
                DrawWorld();
            }
            else if (textBox.Text != string.Empty && textBox.Text[0] != '.' && textBox.Text[textBox.TextLength - 1] != '.'
                && textBox.Text != @"-")
                textBox.Text = Math.Round(Models.Cursor.GetCurrentPosition().X, 2).ToString(CultureInfo.InvariantCulture);
        }

        private void CursorYBox_TextChanged(object sender, EventArgs e)
        {
            if (!_enableWorldDrawing) return;
            var textBox = sender as TextBox;
            double result;
            if (textBox == null) return;
            if (double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out result)
                && result >= -10 && result <= 10)
            {
                var position = Models.Cursor.GetCurrentPosition();
                position.Y = result;
                Models.Cursor.Instance.TranslateToPosition(position);
                Models.Cursor.Instance.UpdateModel();
                DrawWorld();
            }
            else if (textBox.Text != string.Empty && textBox.Text[0] != '.' && textBox.Text[textBox.TextLength - 1] != '.'
                && textBox.Text != @"-")
                textBox.Text = Math.Round(Models.Cursor.GetCurrentPosition().Y, 2).ToString(CultureInfo.InvariantCulture);
        }

        private void CursorZBox_TextChanged(object sender, EventArgs e)
        {
            if (!_enableWorldDrawing) return;
            var textBox = sender as TextBox;
            double result;
            if (textBox == null) return;
            if (double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out result)
                && result >= -10 && result <= 10)
            {
                var position = Models.Cursor.GetCurrentPosition();
                position.Z = result;
                Models.Cursor.Instance.TranslateToPosition(position);
                Models.Cursor.Instance.UpdateModel();
                DrawWorld();
            }
            else if (textBox.Text != string.Empty && textBox.Text[0] != '.' && textBox.Text[textBox.TextLength - 1] != '.'
                && textBox.Text != @"-")
                textBox.Text = Math.Round(Models.Cursor.GetCurrentPosition().Z,2).ToString(CultureInfo.InvariantCulture);
        }

        private void CursorScreenXBox_TextChanged(object sender, EventArgs e)
        {
            if (!_enableWorldDrawing) return;
            var textBox = sender as TextBox;
            int result;
            if (textBox == null) return;
            if (int.TryParse(textBox.Text, out result) && result >= -Parameters.WorldPanelWidth / 2 && result <= Parameters.WorldPanelWidth / 2)
            {
                Models.Cursor.ScreenXPosition = result;
                Models.Cursor.Instance.UpdateModel();
                DrawWorld();
            }
            else if (textBox.Text != string.Empty && textBox.Text != @"-")
                textBox.Text = Models.Cursor.ScreenXPosition.ToString(CultureInfo.InvariantCulture);
        }

        private void CursorScreenYBox_TextChanged(object sender, EventArgs e)
        {
            if (!_enableWorldDrawing) return;
            var textBox = sender as TextBox;
            int result;
            if (textBox == null) return;
            if (int.TryParse(textBox.Text, out result) && result >= -Parameters.WorldPanelHeight / 2 && result <= Parameters.WorldPanelHeight / 2)
            {
                Models.Cursor.ScreenYPosition = result;
                Models.Cursor.Instance.UpdateModel();
                DrawWorld();
            }
            else if (textBox.Text != string.Empty && textBox.Text != @"-")
                textBox.Text = Models.Cursor.ScreenYPosition.ToString(CultureInfo.InvariantCulture);
        }

        private void StereoscopyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null || _lockModification) return;
            _enableStereoscopy = checkBox.Checked;
            DrawWorld();
        }

        private void AdditiveColorBlendingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null || _lockModification) return;
            _enableAdditiveColorBlending = checkBox.Checked;
            DrawWorld();
        }

        private void ControlPointsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null || !radioButton.Checked) return;
            var item = ObjectsList.SelectedItem as BezierCurve;
            if (!_lockModification && item != null)
                item.SetControlPointsEnablability(true);
            DrawWorld();
        }

        private void DeBoorsPointsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null || !radioButton.Checked) return;
            var item = ObjectsList.SelectedItem as BezierCurve;
            if (!_lockModification && item != null)
                item.SetControlPointsEnablability(false);
            DrawWorld();
        }

        private void PolygonalChainCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null) return;
            var cItem = ObjectsList.SelectedItem as BezierCurve;
            if (!_lockModification && cItem != null)
                cItem.TogglePolygonialChain();
            var sItem = ObjectsList.SelectedItem as BezierSurface;
            if (!_lockModification && sItem != null)
                sItem.TogglePolygonialChain();
            var gItem = ObjectsList.SelectedItem as GapFiller;
            if (!_lockModification && gItem != null)
                gItem.TogglePolygonialChain();
            DrawWorld();
        }

        private void NormalParametrizationRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null || !radioButton.Checked) return;
            var item = ObjectsList.SelectedItem as InterpolationCurve;
            if (!_lockModification && item != null)
            {
                item.ChordParametrizationEnabled = false;
                item.UpdateModel();
                DrawWorld();
            }
        }

        private void ChordParametrizationRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null || !radioButton.Checked) return;
            var item = ObjectsList.SelectedItem as InterpolationCurve;
            if (!_lockModification && item != null)
            {
                item.ChordParametrizationEnabled = true;
                item.UpdateModel();
                DrawWorld();
            }
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            Parameters.WorldPanelWidth = WorldPanel.Width;
            Parameters.WorldPanelHeight = WorldPanel.Height;
            Parameters.WorldPanelSizeFactor = (Parameters.WorldPanelWidth < Parameters.WorldPanelHeight) ?
                Parameters.WorldPanelWidth * 0.25f : Parameters.WorldPanelHeight * 0.25f;
            DrawWorld();
        }
        #endregion
    }
}
