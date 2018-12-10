using Framework2D.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Framework2D.Scenes.StarFighters.SceneItems
{
    [Serializable]
    public class Projectile : IGameBehavior, IGame2DProperties, IViewable, IMovable
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

        public string ID = "";

        public float Velocity
        {
            get;

            set;
        }

        public bool ToBeDestroyed
        {
            get;

            set;
        }

        public float Alpha
        {
            get;

            set;
        }

        public Projectile(Vector2F location, float Angle)
        {
            Init();
            this.Location = location; this.Angle = Angle;         
        }

        public void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color), new RectangleF(Location.X, Location.Y, Size.Width, Size.Height));
        }

        public void Init()
        {
            Size = new Size(5, 5);
            Velocity = 6f;
            this.ID = "Projectile" + MathAssist.Rand(1, 0).ToString();
            Color = Color.Yellow;
            IsVisible = true;
        }

        public void Update(GameTime gameTime)
        {

            this.Location.X += this.Velocity * (float)Math.Cos(-this.Angle * Math.PI / 180d);
            this.Location.Y += this.Velocity * (float)Math.Sin(this.Angle * Math.PI / 180d);
        }
    
    }
}
