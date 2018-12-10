using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D.Scenes
{
    class Level1Scene : Scene, IGameBehavior
    {

        private double angle;
        private double r;
        private int speedup = 100;
        Delay delay = new Delay(300);

        public Level1Scene(string nextScene):base(nextScene)
        {
            this.NextScene = nextScene;
        }

        public override void Draw(Graphics g)
        {
            g.Clear(Color.LightGreen);
            g.DrawString("WELCOME TO HEM'S GAME DEMO", new Font("Copperplate Gothic Bold", 20, FontStyle.Bold), Brushes.Red, new PointF((150 + (r * Math.Cos((angle * (Math.PI / 180)))).ToType<float>()), (250 + (r * Math.Sin((angle * (Math.PI / 180)))).ToType<float>())));
        }

        public override void Init()
        {
            r = 50;
            angle = 0;           
            GameWindow.AudioPlayer.Item1.PlayTrack("background");
            RunAgain = true;
        }

        public override void Update(GameTime gameTime)
        {
                angle += 3;
                speedup -= 5;
                if (speedup < 1)
                    speedup = 1;

                 if ((angle > 360))
                {
                    angle = 0;
                }
  

            delay.Update();
            if (delay.IsCompleted)
            {
                RunAgain = false;
                GameWindow.AudioPlayer.Item1.StopTrack("background");               
                GameWindow.CurrentScene = NextScene;
                delay.Reset();
            }

        }

    }
}
