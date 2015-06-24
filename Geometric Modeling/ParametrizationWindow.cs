using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Mathematics;

namespace Geometric_Modeling
{
    public partial class ParametrizationWindow : Form
    {
        private Bitmap _backBufferUV;
        private Bitmap _backBufferST;


        public ParametrizationWindow()
        {
            InitializeComponent();
            _backBufferUV = new Bitmap(UVParametrizationPictureBox.Width, UVParametrizationPictureBox.Height);
            _backBufferST = new Bitmap(STParametrizationPictureBox.Width, STParametrizationPictureBox.Height);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            SetDoubleBuffered(UVParametrizationPictureBox);
            SetDoubleBuffered(STParametrizationPictureBox);
            DrawUVParametrization(null);
            DrawSTParametrization(null);
        }

        public static void SetDoubleBuffered(Control c)
        {
            System.Reflection.PropertyInfo aProp = typeof(Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }

        public void DrawUVParametrization(List<Vector4> points)
        {
            DrawParametrization(UVParametrizationPictureBox, _backBufferUV, points);
        }
        public void DrawSTParametrization(List<Vector4> points)
        {
            DrawParametrization(STParametrizationPictureBox, _backBufferST, points);
        }

        public void DrawParametrization(PictureBox pictureBox, Bitmap backBuffer, List<Vector4> points)
        {
            var graphics = Graphics.FromImage(backBuffer);
            graphics.Clear(Color.Black);
            var size = (float)pictureBox.Width;
            graphics.DrawLine(Pens.Red, 0.0f, 0.5f * size, size, 0.5f * size);
            graphics.DrawLine(Pens.Red, 0.5f * size, 0.0f, 0.5f * size, size);
            pictureBox.Image = backBuffer;
            if (points == null) return;
            var pen = Pens.GreenYellow;
            var sizeFactor = size;

            for (int i = 0; i < points.Count - 1; i++)
            {
                graphics.DrawLine(pen, (float)points[i].X * sizeFactor, (float)(1 - points[i].Y) * sizeFactor,
                    (float)points[i + 1].X * sizeFactor, (float)(1 - points[i + 1].Y) * sizeFactor);
            }
            pictureBox.Image = backBuffer;
            pictureBox.Invalidate();
            pictureBox.Refresh();
        }
    }
}
