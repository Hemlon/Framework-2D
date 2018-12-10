using Framework2D.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Net.Sockets;

namespace Framework2D.Scenes
{
    class StartServerScene : Scene, IGameBehavior
    {
        public StartServerScene(string nextScene) : base(nextScene)
        {
        }

        ServerSide server = new ServerSide();


        public override void Init()
        {
            base.Init();

            var StartServer = new Thread(new ThreadStart(server.StartServer));
            StartServer.Start();          
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (ServerSide.ConnectClients.ConnectedClients.Count > 0)
            {
                GameWindow.CurrentScene = NextScene;
            }
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            
        }

    }
}
