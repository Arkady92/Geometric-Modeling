using System.Drawing;
using System.Windows.Forms;
using Mathematics;
using Models;

namespace Geometric_Modeling
{
    public partial class MainWindow : Form
    {
        #region Cooperation with models
        private void DrawWorld()
        {
            if (!_enableWorldDrawing) return;
            Graphics graphics;
            if (Parameters.GridResolutionX * Parameters.GridResolutionY < 5000 && !_forceStaticGraphics)
                graphics = Graphics.FromImage(_backBuffer);
            else
                graphics = WorldPanel.CreateGraphics();
            graphics.Clear(Color.Black);
            graphics.TranslateTransform((float)(WorldPanel.Size.Width * 0.5), (float)(WorldPanel.Size.Height * 0.5));

            if (_models == null) return;
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
                    (geometricModel as ParametricGeometricModel).DrawStereoscopy(graphics,
                        _currentStereoscopyLeftMatrix,
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
    }
}
