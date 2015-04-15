using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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

        private void SaveScene()
        {
            var stringBuilder = new StringBuilder();
            foreach (var item in ObjectsList.Items.Cast<GeometricModel>())
            {
                switch (item.Type)
                {
                    case ModelType.Torus:
                        var torus = item as Torus;
                        stringBuilder.AppendLine("Torus");
                        if (torus != null)
                        {
                            stringBuilder.AppendLine("Name=" + torus);
                            stringBuilder.AppendLine("R=" + torus.BigRadius);
                            stringBuilder.AppendLine("r=" + torus.SmallRadius);
                            stringBuilder.AppendLine("X=" + torus.GetBasePosition().X);
                            stringBuilder.AppendLine("Y=" + torus.GetBasePosition().Y);
                            stringBuilder.AppendLine("Z=" + torus.GetBasePosition().Z);
                            stringBuilder.AppendLine("TMtx=\n" + torus.CurrentOperationMatrix);
                            stringBuilder.AppendLine("Color=" + torus.DefaultColor.Name);
                            stringBuilder.AppendLine();
                        }
                        break;
                    case ModelType.Point:
                        var point = item as Models.Point;
                        stringBuilder.AppendLine("Point");
                        if (point != null)
                        {
                            stringBuilder.AppendLine("Name=" + point);
                            stringBuilder.AppendLine("X=" + point.GetBasePosition().X);
                            stringBuilder.AppendLine("Y=" + point.GetBasePosition().Y);
                            stringBuilder.AppendLine("Z=" + point.GetBasePosition().Z);
                            stringBuilder.AppendLine("TMtx=\n" + point.CurrentOperationMatrix);
                            stringBuilder.AppendLine("Color=" + point.DefaultColor.Name);
                            stringBuilder.AppendLine();
                        }
                        break;
                    case ModelType.Ellipsoid:
                        var ellipsoid = item as Ellipsoid;
                        stringBuilder.AppendLine("Ellipsoid");
                        if (ellipsoid != null)
                        {
                            stringBuilder.AppendLine("Name=" + ellipsoid);
                            stringBuilder.AppendLine("X=" + ellipsoid.GetBasePosition().X);
                            stringBuilder.AppendLine("Y=" + ellipsoid.GetBasePosition().Y);
                            stringBuilder.AppendLine("Z=" + ellipsoid.GetBasePosition().Z);
                            stringBuilder.AppendLine("TMtx=\n" + ellipsoid.CurrentOperationMatrix);
                            stringBuilder.AppendLine("MMtx=\n" + ellipsoid.ModelMatrix);
                            stringBuilder.AppendLine("Color=" + Parameters.DefaultModelColor.Name);
                            stringBuilder.AppendLine();
                        }
                        break;
                    case ModelType.BezierCurve:
                        var bezierCurve = item as BezierCurve;
                        stringBuilder.AppendLine("BezierCurveC0");
                        if (bezierCurve != null)
                        {
                            stringBuilder.AppendLine("Name=" + bezierCurve);
                            stringBuilder.AppendLine("X=" + bezierCurve.GetBasePosition().X);
                            stringBuilder.AppendLine("Y=" + bezierCurve.GetBasePosition().Y);
                            stringBuilder.AppendLine("Z=" + bezierCurve.GetBasePosition().Z);
                            stringBuilder.AppendLine("TMtx=\n" + bezierCurve.CurrentOperationMatrix);
                            stringBuilder.AppendLine("Color=" + bezierCurve.DefaultColor.Name);
                            foreach (var child in bezierCurve.GetChildren().Cast<Models.Point>())
                            {
                                stringBuilder.AppendLine("CP");
                                stringBuilder.AppendLine("X=" + child.X);
                                stringBuilder.AppendLine("Y=" + child.Y);
                                stringBuilder.AppendLine("Z=" + child.Z);                                
                            }
                            stringBuilder.AppendLine();
                        }
                        break;
                    case ModelType.BezierCurveC2:
                        var bezierCurveC2 = item as BezierCurveC2;
                        stringBuilder.AppendLine("BezierCurveC2");
                        if (bezierCurveC2 != null)
                        {
                            stringBuilder.AppendLine("Name=" + bezierCurveC2);
                            stringBuilder.AppendLine("X=" + bezierCurveC2.GetBasePosition().X);
                            stringBuilder.AppendLine("Y=" + bezierCurveC2.GetBasePosition().Y);
                            stringBuilder.AppendLine("Z=" + bezierCurveC2.GetBasePosition().Z);
                            stringBuilder.AppendLine("TMtx=\n" + bezierCurveC2.CurrentOperationMatrix);
                            stringBuilder.AppendLine("Color=" + bezierCurveC2.DefaultColor.Name);
                            foreach (var child in bezierCurveC2.GetChildren().Cast<Models.Point>())
                            {
                                stringBuilder.AppendLine("CP");
                                stringBuilder.AppendLine("X=" + child.X);
                                stringBuilder.AppendLine("Y=" + child.Y);
                                stringBuilder.AppendLine("Z=" + child.Z);
                            }
                            stringBuilder.AppendLine();
                        }
                        break;
                }
            }
            stringBuilder.AppendLine("Selected");
            foreach (var item in ObjectsList.SelectedItems.Cast<GeometricModel>())
            {
                stringBuilder.AppendLine(item.ToString());
            }
            var file = new StreamWriter("Scene.mg1");
            file.Write(stringBuilder.ToString());
            file.Close();
        }

        private void LoadScene()
        {

        }
        #endregion
    }
}
