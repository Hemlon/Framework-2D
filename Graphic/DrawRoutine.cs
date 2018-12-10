using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D.Graphic
{
   public static class DrawRoutine
    {
        public static void DrawTriangle(Graphics g, Render r)
        {
            var angles = new double[3];
            angles[0] = r.Angle;
            angles[1] = r.Angle - 120d;
            angles[2] = r.Angle + 120d;
            float[] p = new float[6];

            for (int i = 0; i < 3; i++)
            {
                p[2 * i] = r.Size.Width * (float)Math.Cos(angles[i] * Math.PI / 180f);
                p[2 * i + 1] = r.Size.Width * (float)Math.Sin(angles[i] * Math.PI / 180f);
            }

            PointF[] pts = new PointF[3];
            pts[0] = new PointF(r.Translation.X + 1.5f * p[0], r.Translation.Y + 1.5f * p[1]);
            pts[1] = new PointF(r.Translation.X + p[2], r.Translation.Y + p[3]);
            pts[2] = new PointF(r.Translation.X + p[4], r.Translation.Y + p[5]);
            g.FillPolygon(new SolidBrush(r.BackColor.Value), pts);
        }
    }
    
}
