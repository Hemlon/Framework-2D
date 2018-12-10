using Framework2D.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Framework2D.Scenes
{
    class ComplexNumberRotation : Scene, IGameBehavior
    {
        Complex[] newZs;
        Complex[] zs;
        double angle = 0;
        bool isPaused = false;
        Delay colorChange = new Delay(5);
        float Width = GameWindow.View.Size.Width;
        float Height = GameWindow.View.Size.Height;
        double speed = 0.1;
        Delay animateTick;     
        List<PointF> pts = new List<PointF>();
        private bool isDrawLines = true;

        public ComplexNumberRotation(string nextScene) : base(nextScene)
        {
            GameWindow.GameMessages[0] = "Complex Number Demonstration";

            double l = 20d;
            double x = 20;
            double y = 20;

            animateTick = new Delay(5 + (int)(speed * 10));

            zs = new Complex[] { new Complex(x, y), new Complex(x + l, y + l), new Complex(x, y + l), new Complex(x + l, y) };
              zs = new Complex[36];
              for (int i = 0; i < zs.Length; i++)
            {
               //  zs[i] = new Complex(x + l * Math.Cos((i * 10d).ToRadians()), y + l*Math.Sin((i * 10d).ToRadians()));
            }
            zs = new Complex[] { new Complex(60, 20) , new Complex(50,40), new Complex(80,45) , new Complex(85,25)};
            
            newZs = new Complex[zs.Length];
            zs.ToArray<Complex>().CopyTo(newZs, 0);
            isPaused = true;

            var t = new Complex(Math.Sqrt(2), 0) - Math.Sqrt(2) * Complex.cis(0) - 1 / 2 * Complex.cis(-Math.PI / 2) - Math.Sqrt(3) / 2 * Complex.cis(0) - 2 * Math.Sqrt(2) * Complex.cis(-Math.PI / 2);
            
            t.ShowPolar();
        }

        public override void Draw(Graphics t)
        {
            base.Draw(t);
            t.Clear(Color.SkyBlue);
            var mypen = new Pen(new SolidBrush(Color.Black), 1);
            var mypenAxis = new Pen(new SolidBrush(Color.Red), 3);
            for (int i = 0; i < Height / 10; i++)
            {
                t.DrawLine(mypen, new PointF(0, 10 * i), new PointF(Width, 10 * i));
                t.DrawLine(mypen, new PointF(10 * i, 0), new PointF(10 * i, Height));
            }
            t.DrawLine(mypenAxis, new PointF(0, Height / 2), new PointF(Width, Height / 2));
            t.DrawLine(mypenAxis, new PointF(Width / 2, 0), new PointF(Width / 2, Height));

            var r = new Random();
            Brush b = new SolidBrush(Color.Red);
            for (int i = 0; i < newZs.Length; i++)
            {
             //   col.PeriodicallyDo(() => {
                    b = new SolidBrush(Color.FromArgb(255, r.Next(0, 200), r.Next(0, 200), r.Next(0, 200)));
            //  });
                t.FillEllipse(b, new RectangleF(Width / 2 + (float)newZs[i].Real - 5, Height / 2 - (float)newZs[i].Imz - 5, 10, 10));

            }
            if (pts.Count > 2 && isDrawLines)
            t.DrawPolygon(new Pen(new SolidBrush(Color.Black), 1), pts.ToArray());
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            if (isPaused == false)
            {
                animateTick.PeriodicallyDo(() =>
                {
                    angle = angle.ChangeBy<double>(speed, (e) => e < 2 * Math.PI, 0);
                    for (int i = 0; i < zs.Length; i++)
                    {
                        var z = (new Complex(1 * Math.Cos(angle), 1 * Math.Sin(angle)));
                        z = new Complex(-11d / 20d, 37d / 20d);
                        newZs[i] = zs[i] * (z)/z.GetMag();
                        if (isDrawLines)
                            pts.Add(new PointF(Width / 2 + (float)newZs[i].Real, Height / 2 - (float)newZs[i].Imz));                 
                    }
                }   
                );
            }

            base.Update(gameTime);
            if (GameInput.GetMouseButton(System.Windows.Forms.MouseButtons.Left))
            {
                GameInput.ResetMouseButton(System.Windows.Forms.MouseButtons.Left);
                isPaused = !isPaused;
            }
            if (GameInput.isKeyDown(Keys.Down))
            {
                speed += 0.001;
                animateTick.Set((int)(speed * 1000));
            }
            if (GameInput.isKeyDown(Keys.Up))
            {
                speed -= 0.001;
                animateTick.Set((int)(speed * 1000));
            }
            if (GameInput.isKeyDown(Keys.L))
            {
                GameInput.ResetKeyDown(Keys.L);
                isDrawLines = !isDrawLines;
            }
        }
    }
}
