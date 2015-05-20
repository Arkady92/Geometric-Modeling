using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mathematics;
using Models;
using Point = Models.Point;

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
                        SaveTorus(item as Torus, stringBuilder);
                        break;
                    case ModelType.Point:
                        SavePoint(item as Point, stringBuilder);
                        break;
                    case ModelType.Ellipsoid:
                        SaveEllipsoid(item as Ellipsoid, stringBuilder);

                        break;
                    case ModelType.BezierCurve:
                        SaveCurve(item as BezierCurve, stringBuilder);
                        break;
                    case ModelType.BezierCurveC2:
                        SaveCurve(item as BezierCurveC2, stringBuilder);
                        break;
                    case ModelType.InterpolationCurve:
                        SaveCurve(item as InterpolationCurve, stringBuilder);
                        break;
                    case ModelType.BezierSurface:
                        SaveSurface(item as BezierSurface, stringBuilder);
                        break;
                    case ModelType.BezierSurfaceC2:
                        SaveSurface(item as BezierSurfaceC2, stringBuilder);
                        break;
                }
            }
            stringBuilder.AppendLine("Selected");
            foreach (var item in ObjectsList.SelectedItems.Cast<GeometricModel>())
                stringBuilder.AppendLine(item.Id.ToString(CultureInfo.InvariantCulture));

            var fileDialog = new SaveFileDialog
            {
                FileName = Parameters.DefaultFilePath,
                Filter = @"Model files (*.mg1)|*.mg1"
            };

            if (fileDialog.ShowDialog() != DialogResult.OK) return;
            using (var streamWriter = new StreamWriter(fileDialog.FileName))
                streamWriter.Write(stringBuilder.ToString());
        }

        private void SaveSurface(BezierSurface bezierSurface, StringBuilder stringBuilder)
        {
            if (bezierSurface == null) return;
            switch (bezierSurface.Type)
            {
                case ModelType.BezierSurface:
                    stringBuilder.AppendLine("BezierSurfaceC0");
                    break;
                case ModelType.BezierSurfaceC2:
                    stringBuilder.AppendLine("BezierSurfaceC2");
                    break;
            }
            stringBuilder.AppendLine("Id=" + bezierSurface.Id);
            stringBuilder.AppendLine("Name=" + bezierSurface.CustomName);
            stringBuilder.AppendLine("Width=" + bezierSurface.Width);
            stringBuilder.AppendLine("Height=" + bezierSurface.Height);
            stringBuilder.AppendLine("PatchesXCount=" + bezierSurface.PatchesLengthCount);
            stringBuilder.AppendLine("PatchesYCount=" + bezierSurface.PatchesBreadthCount);
            stringBuilder.AppendLine("Cylindrical=" + bezierSurface.IsCylindrical);
            stringBuilder.AppendLine("X=" + bezierSurface.GetBasePosition().X);
            stringBuilder.AppendLine("Y=" + bezierSurface.GetBasePosition().Y);
            stringBuilder.AppendLine("Z=" + bezierSurface.GetBasePosition().Z);
            stringBuilder.AppendLine("TMtx=\n" + bezierSurface.OperationMatrix);
            stringBuilder.AppendLine("Color=" + bezierSurface.Color.Name);
            foreach (var child in bezierSurface.GetChildren().Cast<Point>())
                stringBuilder.AppendLine("CP=" + child.Id);
            stringBuilder.AppendLine();
        }

        private void SaveCurve(BezierCurve bezierCurve, StringBuilder stringBuilder)
        {
            if (bezierCurve == null) return;
            switch (bezierCurve.Type)
            {
                case ModelType.BezierCurve:
                    stringBuilder.AppendLine("BezierCurveC0");
                    break;
                case ModelType.BezierCurveC2:
                    stringBuilder.AppendLine("BezierCurveC2");
                    break;
                case ModelType.InterpolationCurve:
                    stringBuilder.AppendLine("InterpolationCurve");
                    break;
            }
            stringBuilder.AppendLine("Id=" + bezierCurve.Id);
            stringBuilder.AppendLine("Name=" + bezierCurve.CustomName);
            stringBuilder.AppendLine("X=" + bezierCurve.GetBasePosition().X);
            stringBuilder.AppendLine("Y=" + bezierCurve.GetBasePosition().Y);
            stringBuilder.AppendLine("Z=" + bezierCurve.GetBasePosition().Z);
            stringBuilder.AppendLine("TMtx=\n" + bezierCurve.OperationMatrix);
            stringBuilder.AppendLine("Color=" + bezierCurve.Color.Name);
            foreach (var child in bezierCurve.GetChildren().Cast<Point>())
            {
                stringBuilder.AppendLine("CP=" + child.Id);
            }
            stringBuilder.AppendLine();
        }

        private void SaveEllipsoid(Ellipsoid ellipsoid, StringBuilder stringBuilder)
        {
            if (ellipsoid == null) return;
            stringBuilder.AppendLine("Ellipsoid");
            stringBuilder.AppendLine("Id=" + ellipsoid.Id);
            stringBuilder.AppendLine("Name=" + ellipsoid.CustomName);
            stringBuilder.AppendLine("X=" + ellipsoid.GetBasePosition().X);
            stringBuilder.AppendLine("Y=" + ellipsoid.GetBasePosition().Y);
            stringBuilder.AppendLine("Z=" + ellipsoid.GetBasePosition().Z);
            stringBuilder.AppendLine("TMtx=\n" + ellipsoid.OperationMatrix);
            stringBuilder.AppendLine("MMtx=\n" + ellipsoid.ModelMatrix);
            stringBuilder.AppendLine("Color=" + ellipsoid.Color.Name);
            stringBuilder.AppendLine();
        }

        private void SavePoint(Point point, StringBuilder stringBuilder)
        {
            if (point == null) return;
            stringBuilder.AppendLine("Point");
            stringBuilder.AppendLine("Id=" + point.Id);
            stringBuilder.AppendLine("Name=" + point.CustomName);
            stringBuilder.AppendLine("X=" + point.GetBasePosition().X);
            stringBuilder.AppendLine("Y=" + point.GetBasePosition().Y);
            stringBuilder.AppendLine("Z=" + point.GetBasePosition().Z);
            stringBuilder.AppendLine("TMtx=\n" + point.OperationMatrix);
            stringBuilder.AppendLine("Color=" + point.Color.Name);
            stringBuilder.AppendLine();
        }

        private void SaveTorus(Torus torus, StringBuilder stringBuilder)
        {
            if (torus == null) return;
            stringBuilder.AppendLine("Torus");
            stringBuilder.AppendLine("Id=" + torus.Id);
            stringBuilder.AppendLine("Name=" + torus.CustomName);
            stringBuilder.AppendLine("R=" + torus.BigRadius);
            stringBuilder.AppendLine("r=" + torus.SmallRadius);
            stringBuilder.AppendLine("X=" + torus.GetBasePosition().X);
            stringBuilder.AppendLine("Y=" + torus.GetBasePosition().Y);
            stringBuilder.AppendLine("Z=" + torus.GetBasePosition().Z);
            stringBuilder.AppendLine("TMtx=\n" + torus.OperationMatrix);
            stringBuilder.AppendLine("Color=" + torus.Color.Name);
            stringBuilder.AppendLine();
        }

        private void LoadScene()
        {
            ClearButton_Click(null, null);

            var fileDialog = new OpenFileDialog
            {
                FileName = Parameters.DefaultFilePath,
                Filter = @"Model files (*.mg1)|*.mg1"
            };

            if (fileDialog.ShowDialog() != DialogResult.OK) return;
            var streamReader = new StreamReader(fileDialog.FileName);
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                switch (line)
                {
                    case "Torus":
                        LoadTorus(streamReader);
                        break;
                    case "Point":
                        LoadPoint(streamReader);
                        break;
                    case "Ellipsoid":
                        LoadElipsoid(streamReader);
                        break;
                    case "BezierCurveC0":
                        LoadCurve(streamReader, ModelType.BezierCurve);
                        break;
                    case "BezierCurveC2":
                        LoadCurve(streamReader, ModelType.BezierCurveC2);
                        break;
                    case "InterpolationCurve":
                        LoadCurve(streamReader, ModelType.InterpolationCurve);
                        break;
                    case "BezierSurfaceC0":
                        LoadSurface(streamReader, ModelType.BezierSurface);
                        break;
                    case "BezierSurfaceC2":
                        LoadSurface(streamReader, ModelType.BezierSurfaceC2);
                        break;
                    case "Selected":
                        SelectObjects(streamReader);
                        break;

                }
            }
            streamReader.Close();
            UpdateTextBoxes();
            DrawWorld();
        }

        private void SelectObjects(StreamReader streamReader)
        {
            var potentialModels = ObjectsList.Items.OfType<GeometricModel>().ToList();
            while (true)
            {
                var line = streamReader.ReadLine();
                if (string.IsNullOrEmpty(line)) break;
                var id = ReadInt(line);
                ObjectsList.SelectedItems.Add(potentialModels.First(item => item.Id == id));
            }
        }

        private void LoadSurface(StreamReader streamReader, ModelType modelType)
        {
            var id = ReadInt(streamReader.ReadLine());
            var name = ReadString(streamReader.ReadLine());
            var width = ReadDouble(streamReader.ReadLine());
            var height = ReadDouble(streamReader.ReadLine());
            var patchesLengthCount = ReadInt(streamReader.ReadLine());
            var patchesBreadthCount = ReadInt(streamReader.ReadLine());
            var isCylindrical = ReadBool(streamReader.ReadLine());
            var x = ReadDouble(streamReader.ReadLine());
            var y = ReadDouble(streamReader.ReadLine());
            var z = ReadDouble(streamReader.ReadLine());
            var matrix = ReadMatrix(streamReader);
            var colorName = ReadString(streamReader.ReadLine());
            var color = Color.FromName(colorName);
            var points = ReadPoints(streamReader);
            GeometricModel surface = null;
            switch (modelType)
            {
                case ModelType.BezierSurface:
                    surface = new BezierSurface(points, new Vector4(x, y, z), width, height, patchesLengthCount, patchesBreadthCount,
                        isCylindrical)
                    {
                        Id = id,
                        CustomName = name,
                        Color = color,
                        OperationMatrix = matrix,
                        ChainEnabled = false
                    };
                    break;
                case ModelType.BezierSurfaceC2:
                    surface = new BezierSurfaceC2(points, new Vector4(x, y, z), width, height, patchesLengthCount, patchesBreadthCount,
                        isCylindrical)
                    {
                        Id = id,
                        CustomName = name,
                        Color = color,
                        OperationMatrix = matrix,
                        ChainEnabled = false
                    };
                    break;
            }
            if (surface == null) return;
            _models.Add(surface);
            ObjectsList.Items.Add(surface);
        }


        private void LoadCurve(StreamReader streamReader, ModelType modelType)
        {
            var id = ReadInt(streamReader.ReadLine());
            var name = ReadString(streamReader.ReadLine());
            var x = ReadDouble(streamReader.ReadLine());
            var y = ReadDouble(streamReader.ReadLine());
            var z = ReadDouble(streamReader.ReadLine());
            var matrix = ReadMatrix(streamReader);
            var colorName = ReadString(streamReader.ReadLine());
            var color = Color.FromName(colorName);
            var points = ReadPoints(streamReader);
            GeometricModel curve = null;
            switch (modelType)
            {
                case ModelType.BezierCurve:
                    curve = new BezierCurve(points, new Vector4(x, y, z))
                    {
                        Id = id,
                        CustomName = name,
                        Color = color,
                        OperationMatrix = matrix
                    };
                    break;
                case ModelType.BezierCurveC2:
                    curve = new BezierCurveC2(points, new Vector4(x, y, z))
                    {
                        Id = id,
                        CustomName = name,
                        Color = color,
                        OperationMatrix = matrix
                    };
                    break;
                case ModelType.InterpolationCurve:
                    curve = new InterpolationCurve(points, new Vector4(x, y, z), matrix)
                    {
                        Id = id,
                        CustomName = name,
                        Color = color
                    };
                    break;
            }
            if (curve == null) return;
            _models.Add(curve);
            ObjectsList.Items.Add(curve);
        }

        private void LoadElipsoid(StreamReader streamReader)
        {
            var id = ReadInt(streamReader.ReadLine());
            var name = ReadString(streamReader.ReadLine());
            var x = ReadDouble(streamReader.ReadLine());
            var y = ReadDouble(streamReader.ReadLine());
            var z = ReadDouble(streamReader.ReadLine());
            var tMatrix = ReadMatrix(streamReader);
            var mMatrix = ReadMatrix(streamReader);
            var colorName = ReadString(streamReader.ReadLine());
            var color = Color.FromName(colorName);
            var ellipsoid = new Ellipsoid(new Vector4(x, y, z))
            {
                Id = id,
                CustomName = name,
                Color = color,
                OperationMatrix = tMatrix,
                ModelMatrix = mMatrix
            };
            _models.Add(ellipsoid);
            ObjectsList.Items.Add(ellipsoid);
            _forceStaticGraphics = true;
            _enableAnimations = true;
            streamReader.ReadLine();
        }

        private void LoadPoint(StreamReader streamReader)
        {
            var id = ReadInt(streamReader.ReadLine());
            var name = ReadString(streamReader.ReadLine());
            var x = ReadDouble(streamReader.ReadLine());
            var y = ReadDouble(streamReader.ReadLine());
            var z = ReadDouble(streamReader.ReadLine());
            var matrix = ReadMatrix(streamReader);
            var colorName = ReadString(streamReader.ReadLine());
            var color = Color.FromName(colorName);
            var point = new Point(new Vector4(x, y, z))
            {
                Id = id,
                CustomName = name,
                Color = color,
                OperationMatrix = matrix
            };
            _models.Add(point);
            ObjectsList.Items.Add(point);
            streamReader.ReadLine();
        }

        private void LoadTorus(StreamReader streamReader)
        {
            var id = ReadInt(streamReader.ReadLine());
            var name = ReadString(streamReader.ReadLine());
            var bigR = ReadDouble(streamReader.ReadLine());
            var smallR = ReadDouble(streamReader.ReadLine());
            var x = ReadDouble(streamReader.ReadLine());
            var y = ReadDouble(streamReader.ReadLine());
            var z = ReadDouble(streamReader.ReadLine());
            var matrix = ReadMatrix(streamReader);
            var colorName = ReadString(streamReader.ReadLine());
            var color = Color.FromName(colorName);
            var torus = new Torus(new Vector4(x, y, z), bigR, smallR)
            {
                Id = id,
                CustomName = name,
                Color = color,
                OperationMatrix = matrix
            };
            _models.Add(torus);
            ObjectsList.Items.Add(torus);
            streamReader.ReadLine();
        }

        private IEnumerable<Point> ReadPoints(StreamReader streamReader)
        {
            var potentialPoints = ObjectsList.Items.OfType<Point>().ToList();
            var points = new List<Point>();
            while (true)
            {
                var line = streamReader.ReadLine();
                if (string.IsNullOrEmpty(line)) break;
                var id = ReadInt(line);
                points.Add(potentialPoints.First(item => item.Id == id));
            }
            return points;
        }

        private Matrix ReadMatrix(StreamReader streamReader)
        {
            streamReader.ReadLine();
            var data = streamReader.ReadLine() + "\r\n";
            data = data + streamReader.ReadLine() + "\r\n";
            data = data + streamReader.ReadLine() + "\r\n";
            data = data + streamReader.ReadLine() + "\r\n";
            return Matrix.Parse(data.Replace('.', ','));
        }

        private double ReadDouble(string line)
        {
            return double.Parse(line.Substring(line.IndexOf("=", System.StringComparison.Ordinal) + 1).Replace('.', ','));
        }

        private string ReadString(string line)
        {
            return line.Substring(line.IndexOf("=", System.StringComparison.Ordinal) + 1);
        }

        private int ReadInt(string line)
        {
            return int.Parse(line.Substring(line.IndexOf("=", System.StringComparison.Ordinal) + 1));
        }

        private bool ReadBool(string line)
        {
            return bool.Parse(line.Substring(line.IndexOf("=", System.StringComparison.Ordinal) + 1));
        }
        #endregion
    }
}
