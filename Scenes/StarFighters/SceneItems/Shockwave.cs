using Framework2D.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D.Scenes.StarFighters.SceneItems
{
    [Serializable]
    class Shockwave: IGameBehavior, IGame2DProperties, IMovable, IViewable
    {

        public bool ToBeDestroyed
        {
            get;

            set;
        }
        public float Velocity
        {
            get;

            set;
        }
        public double Angle
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
        public Color Color
        {
            get;

            set;
        }
        public float Alpha
        {
            get;

            set;
        }
        public float radius;
       Delay waveTiming = new Delay(20);
        public Delay fadeTiming = new Delay(2);
        public float RaduisMax = 0;
        public string ID = "";
        private float v2d = 0;
 
        public Shockwave(Color color, Vector2F location, float velocity,  int delayPerFrame, float radiusMax, float velocity2D, double angle)
        {
            Color = color;
            waveTiming = new Delay(delayPerFrame);
            RaduisMax = radiusMax;
            Location = location;
            Velocity = velocity;
            ID = MathAssist.Rand(1, 0).ToString();
            v2d = velocity2D;
            Angle = angle;
            Init();
        }
        public void Init()
        {
            this.radius = 0;
            this.Size = new SizeF(0, 0);
            this.IsVisible = true;
            this.Alpha = 1f;
            this.ToBeDestroyed = false;
        }
        public void Update(GameTime gameTime)
        {

            waveTiming.PeriodicallyDo(() => { this.radius += this.Velocity; });
            Location.X += this.v2d * (float)Math.Cos(Angle * Math.PI / 180d);
            Location.Y += this.v2d * (float)Math.Sin(Angle * Math.PI / 180d);

            if (Alpha < 0)
            {
                this.ToBeDestroyed = true;
                Alpha = 0;
            }

            fadeTiming.PeriodicallyDo(() => { Alpha -= 0.1f; });


        }
        public void Draw(Graphics g)
        {
            Color c = Color.FromArgb((Alpha * 255f).ToType<int>(), Color);
            g.DrawEllipse(new Pen(new SolidBrush(c), 2),this.Location.X - this.radius/2, this.Location.Y -  this.radius / 2, this.radius * 2, this.radius * 2);
        }


    }
}
