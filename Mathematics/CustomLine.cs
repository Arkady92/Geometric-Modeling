using System;
using System.Drawing;

namespace Mathematics
{
    public class CustomLine
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public CustomLine(Point startPoint, Point endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public void Draw(Bitmap bitmap, Graphics graphics, Color color, int thickness)
        {
            SymmetricBresenham(bitmap, graphics, color, thickness);
        }
        
        private void SymmetricBresenham(Bitmap bitmap, Graphics graphics, Color color, int thickness)
        {
            int upPixels = (thickness - 1) / 2;
            int downPixels = thickness - 1 - upPixels;
            int x1 = StartPoint.X, x2 = EndPoint.X, y1 = StartPoint.Y, y2 = EndPoint.Y;
            int incrX, incrY, dx, dy;
            int width = Parameters.WorldPanelWidth/2;
            int height = Parameters.WorldPanelHeight/2;
            FindDrawingStep(x1, x2, y1, y2, out incrX, out incrY, out dx, out dy);
            if (x1 >= -width && x1 < width && y1 >= -height && y1 < height)
            {
                Color c1 = CombineColor(color, bitmap.GetPixel(x1 + width, y1 + height));
                graphics.FillRectangle(new SolidBrush(c1), new Rectangle(new Point(x1, y1), new Size(1, 1)));
            }
            if (x2 >= -width && x2 < width && y2 >= -height && y2 < height)
            {
                Color c2 = CombineColor(color, bitmap.GetPixel(x2 + width, y2 + height));
                graphics.FillRectangle(new SolidBrush(c2), new Rectangle(new Point(x2, y2), new Size(1, 1)));
            }
            if (dx > dy)
                DrawLine(bitmap, graphics, color, x1, y1, x2, y2, dy, dx, incrX, incrY, upPixels, downPixels, isHorizontal: true);
            else
                DrawLine(bitmap, graphics, color, y1, x1, y2, x2, dx, dy, incrY, incrX, upPixels, downPixels, isHorizontal: false);
        }

        private static void DrawLine(Bitmap bitmap, Graphics graphics, Color color, int x1, int y1, int x2, int y2
        , int dy, int dx, int incrX, int incrY, int upPixels, int downPixels, bool isHorizontal)
        {
            int xf = x1;
            int yf = y1;
            int xb = x2;
            int yb = y2;
            var incrE = 2 * dy;
            var incrNe = 2 * (dy - dx);
            var d = 2 * dy - dx;
            while (xf != xb && xf - 1 != xb && xf + 1 != xb)
            {
                xf += incrX;
                xb -= incrX;
                if (d < 0) //Choose E and W
                    d += incrE;
                else //Choose NE and SW
                {
                    d += incrNe;
                    yf += incrY;
                    yb -= incrY;
                }
                if (isHorizontal)
                    DrawLineSegment(bitmap, graphics, xf, yf, xb, yb, upPixels, downPixels, color, true);
                else
                    DrawLineSegment(bitmap, graphics, yf, xf, yb, xb, upPixels, downPixels, color, false);
            }
        }

        private static void DrawLineSegment(Bitmap bitmap, Graphics graphics, int x1, int y1, int x2, int y2
        , int upPixels, int downPixels, Color color, bool isLineHorizontal)
        {
            int horizontalMultiplier = isLineHorizontal ? 1 : 0;
            int verticalMultiplier = 1 - horizontalMultiplier;
            Color c1 = color, c2 = color;
            int width = Parameters.WorldPanelWidth / 2;
            int height = Parameters.WorldPanelHeight / 2;
            if (x1 >= -width && x1 < width && y1 >= -height && y1 < height)
                c1 = CombineColor(color, bitmap.GetPixel(x1 + width, y1 + height));
            if (x2 >= -width && x2 < width && y2 >= -height && y2 < height)
                c2 = CombineColor(color, bitmap.GetPixel(x2 + width, y2 + height));
            for (int i = 0; i <= upPixels; i++)
            {
                if (x1 + i * horizontalMultiplier >= -width && x1 + i * horizontalMultiplier < width
                && y1 + i * verticalMultiplier >= -height && y1 + i * verticalMultiplier < height)
                    graphics.FillRectangle(new SolidBrush(c1), new Rectangle(new Point(
                    x1 + i * horizontalMultiplier, y1 + i * verticalMultiplier), new Size(1, 1)));
                if (x2 + i * horizontalMultiplier >= -width && x2 + i * horizontalMultiplier < width
                && y2 + i * verticalMultiplier >= -height && y2 + i * verticalMultiplier < height)
                    graphics.FillRectangle(new SolidBrush(c2), new Rectangle(new Point(
                    x2 + i * horizontalMultiplier, y2 + i * verticalMultiplier), new Size(1, 1)));
            }
            for (int i = 0; i <= downPixels; i++)
            {
                if (x1 - i * horizontalMultiplier >= -width && x1 - i * horizontalMultiplier < width
                && y1 - i * verticalMultiplier >= -height && y1 - i * verticalMultiplier < height)
                    graphics.FillRectangle(new SolidBrush(c1), new Rectangle(new Point(
                    x1 - i * horizontalMultiplier, y1 - i * verticalMultiplier), new Size(1, 1)));
                if (x2 - i * horizontalMultiplier >= -width && x2 - i * horizontalMultiplier < width
                && y2 - i * verticalMultiplier >= -height && y2 - i * verticalMultiplier < height)
                    graphics.FillRectangle(new SolidBrush(c2), new Rectangle(new Point(
                    x2 - i * horizontalMultiplier, y2 - i * verticalMultiplier), new Size(1, 1)));
            }
        }

        private static Color CombineColor(Color color1, Color color2)
        {
            var a = color1.A + color2.A;
            if (a > 255) a = 255;
            var r = color1.R + color2.R;
            if (r > 255) r = 255;
            var g = color1.G + color2.G;
            if (g > 255) g = 255;
            var b = color1.B + color2.B;
            if (b > 255) b = 255;
            return Color.FromArgb(a, r, g, b);
        }

        private void FindDrawingStep(int x1, int x2, int y1, int y2
        , out int incrX, out int incrY, out int dx, out int dy)
        {
            // Find the direction of drawing in x coordinate
            incrX = x1 < x2 ? 1 : -1;
            dx = Math.Abs(x1 - x2);
            // Find the direction of drawing in y coordinate
            incrY = y1 < y2 ? 1 : -1;
            dy = Math.Abs(y1 - y2);
        }
    }
}
