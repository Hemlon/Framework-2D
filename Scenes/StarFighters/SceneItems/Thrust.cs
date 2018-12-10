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
    class Thrust : IGameBehavior, IGame2DProperties, IViewable, IMovable
    {
        public double Angle
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
        public float Velocity
        {
            get;

            set;
        }
        public float Alpha
        {
            get;

            set;
        }
        Delay delayFade = new Delay(1);
        public string ID = "";

        public Thrust(Vector2F location, Color color, double angle, float velocity, int delay)
        {
            this.Velocity = velocity;
            this.Location = location;
            this.Color = color;
            this.Angle = angle;
            this.Size = new Size(3, 3);
            delayFade = new Framework2D.Delay(delay);
            this.ID = "Thrust" + MathAssist.Rand(1, 0).ToString();
        }
        public void Draw(Graphics g)
        {
            var tt = Alpha * 255f;
                Color = Color.FromArgb((tt).ToType<int>(), Color.Orange);
          
               g.FillRectangle(new SolidBrush(Color), new RectangleF(new PointF(Location.X, Location.Y), Size));
        }

        public void Init()
        {
            Alpha = 1f;
        }

        public void Update(GameTime gameTime)
        {
            //delayFade.Update();
            //if (delayFade.IsCompleted)
            //{
            Action fade = () =>
            {
                Alpha -= 0.1f;
                if (Alpha < 0)
                    ToBeDestroyed = true;
            };

            delayFade.PeriodicallyDo(fade);

            Location.X += (float)(this.Velocity * Math.Cos(this.Angle * Math.PI / 180f));
            Location.Y += (float)( this.Velocity * Math.Sin(this.Angle * Math.PI / 180f));
           // Location = new Vector2F((float)x, (float)y);             
        }
    }
}
