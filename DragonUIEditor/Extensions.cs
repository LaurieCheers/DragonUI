using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Drawing.Imaging;

namespace DragonUI
{
    public static class Extensions
    {
        public static Rectangle ExpandToContain(this Rectangle a, Rectangle b)
        {
            int X = Math.Min(a.X, b.X);
            int Y = Math.Min(a.Y, b.Y);
            return new Rectangle(X, Y, Math.Max(a.Right, b.Right) - X, Math.Max(a.Bottom, b.Bottom) - Y);
        }

        public static Rectangle toRectangle(this JSONArray array)
        {
            if (array == null)
                return new Rectangle(0,0,0,0);
            else
                return new Rectangle(array.getInt(0), array.getInt(1), array.getInt(2), array.getInt(3));
        }
    }
}

namespace DragonUIEditor
{
    public static class Extensions
    {
        public static void DrawImageForceColor(this System.Drawing.Graphics graphics, System.Drawing.Image image, System.Drawing.Rectangle rect, System.Drawing.Color color)
        {
            // Create parallelogram for drawing image.
            System.Drawing.Point ulCorner1 = rect.Location;
            System.Drawing.Point urCorner1 = new System.Drawing.Point(rect.Right, rect.Top);
            System.Drawing.Point llCorner1 = new System.Drawing.Point(rect.Left, rect.Bottom);
            System.Drawing.Point[] destPara1 = { ulCorner1, urCorner1, llCorner1 };

            // Create rectangle for source image.
            System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle(0, 0, image.Width, image.Height);
            System.Drawing.GraphicsUnit units = System.Drawing.GraphicsUnit.Pixel;

            // Create image attributes and set large gamma.
            System.Drawing.Imaging.ImageAttributes imageAttr = new System.Drawing.Imaging.ImageAttributes();
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]{
                new float[]{0,0,0,0,0},
                new float[]{0,0,0,0,0},
                new float[]{0,0,0,0,0},
                new float[]{0,0,0,1,0},
                new float[]{color.R, color.G, color.B, 0, 1}
            });
            imageAttr.SetColorMatrix(colorMatrix);

            // Draw original image to screen.
            graphics.DrawImage(image, destPara1, srcRect, units, imageAttr);
        }
    }
}
