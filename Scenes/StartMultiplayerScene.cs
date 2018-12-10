using Framework2D.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Framework2D.Scenes
{
    class StartMultiplayerScene : Scene,IGameBehavior
    {
        public bool isConnected = false;

        public ClientSide TCPClient = new Multiplayer.ClientSide();
        public StartMultiplayerScene(string nextScene) : base(nextScene)
        {
             NextScene = nextScene;
        }

        public override void Init()
        {
            RunAgain = true;

            TCPClient.InputIPForm();
            

        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);

         //   if (TCPClient  || TCPClient.isWait.IsNotTrue())
                GameWindow.CurrentScene = NextScene;

        }

    }
}
