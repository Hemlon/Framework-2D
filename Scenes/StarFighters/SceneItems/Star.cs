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
    public class Star: IGameBehavior, IGame2DProperties, IKillable, IViewable
    {
        private float pulsateAngle;
        private float pulsateDr;
        private float pulsateMax;
        private float pulsateFreq;
        Delay TwinkleTiming = new Delay(1);
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
        public bool IsVisible
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

        public Star(Color color, RectangleF rect, float pulsateRadius, int freq)
        {
            this.Color = color;
            this.Size = rect.Size;
            this.Location = new Vector2F(rect.Location.X, rect.Location.Y);
            this.pulsateMax = pulsateRadius;
            this.pulsateDr = 0;
            this.pulsateAngle = new Random().Next(0, 360) ;
            this.pulsateFreq = freq;
            TwinkleTiming = new Delay(freq);
        }

        public void Update(GameTime g)
        {

            Action twinkle = () =>
            {
                pulsateAngle += pulsateFreq;
                if (pulsateAngle > 360) pulsateAngle = 0;
                pulsateDr = pulsateMax * (float)Math.Sin(pulsateAngle.ToType<double>() * Math.PI / 180d);
            };

            TwinkleTiming.PeriodicallyDo(twinkle);
        }

        public void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(this.Color), new RectangleF(this.Location.X, this.Location.Y, this.Size.Width + this.pulsateDr, this.Size.Height + this.pulsateDr));
        }

        public void Init()
        {
           
        }

    }
}
