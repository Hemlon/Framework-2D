using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D.Scenes.StarFighters
{
    class TitleScreenScene : Scene, IGameBehavior
    {
        public TitleScreenScene(string nextScene) : base(nextScene)
        {
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            g.DrawString("STAR FIGHTERS", new Font("Arial", 30), new SolidBrush(Color.Blue), new PointF(30, 150));
            g.DrawString("Press Y to Begin", new Font("Arial", 22), new SolidBrush(Color.Red), new PointF(50, 200));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
         //   if (GameInput.isKeyDown(System.Windows.Forms.Keys.Y))
            {
                GameWindow.CurrentScene = NextScene; 
            }
        }


    }
}
