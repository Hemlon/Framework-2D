using Framework2D.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Framework2D.Utilities;

namespace Framework2D.Scenes.StarFighters.SceneItems
{
    [Serializable]
    class Asteroid : IGameBehavior, IViewable, IMovable, IGame2DProperties
    {
        public float Alpha
        {
            get;

            set;
        }

        public Color Color
        {
            get;

            set;
        }

        public bool IsVisible
        {
            get;

            set;
        }

        public Vector2F Location
        {
            get;

            set;
        }

        public SizeF Size
        {
            get;

            set;
        }

        public bool ToBeDestroyed
        {
            get;

            set;
        }

       public double Angle
        {
            get;

            set;
        }

       public float Velocity
        {
            get;

            set;
        }
        Point[] pts;
        public void Draw(Graphics g)
        {
            var points = new Point[pts.Count<Point>()];
            for (int i = 0; i < pts.Count<Point>(); i++)
            { 
                points[i] = new Point((int)Location.X + (int)pts[i].X,(int)Location.Y + (int)pts[i].Y);
            }
            g.FillPolygon(new SolidBrush(Color), points);
          //  g.FillEllipse(new SolidBrush(Color), new RectangleF(Location.ToPointF(), Size));
        }

        public Asteroid(Color color, SizeF size,  Vector2F location , double angle , float velocity)
        {
          //  Init();
            Color = color;
            Location = location;
            Angle = angle;
            Velocity = velocity;
            Size = size;
        }

        public void Init()
        {
            Alpha = 1f;
            ToBeDestroyed = false;
            IsVisible = true;
            var count = MathAssist.RandInt(10, 20);
            pts = new Point[count];
            for (int i = 0; i < count; i++)
            {
                var delx = Location.X + Size.Width * Math.Cos(i * 360 / count * Math.PI / 180f);
                var dely = Location.Y + Size.Height * Math.Sin(i * 360 / count * Math.PI / 180f);
                pts[i] = new Point((int)delx - MathAssist.RandInt(1, 30), (int)dely + MathAssist.RandInt(1, 20));
            }
        }

        public void Update(GameTime gameTime)
        {
            Location = Translation.Move2D(Location, this.Velocity, Angle);
        }
    }
}
