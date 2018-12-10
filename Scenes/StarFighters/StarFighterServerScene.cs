using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework2D.Scenes.StarFighters;
using System.Windows.Forms;
using Framework2D.Interface;
using Framework2D.Utilities;
using System.Runtime.InteropServices;
using Framework2D.Graphic;
using Framework2D.Destruction;
using Framework2D.Scenes.StarFighters.SceneItems;
using Framework2D.Multiplayer;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Collections.Concurrent;

namespace Framework2D.Scenes
{
    class StarFighterServerScene : Scene, IGameBehavior
    {
        public ConcurrentDictionary<string, ClientMember> PlayingClients = new ConcurrentDictionary<string, ClientMember>();
        public bool delClient = false;

        public StarFighterServerScene(string nextScene) : base(nextScene)
        {
           
        }

        public KeyValuePair<string,StarFighter> fighter = new KeyValuePair<string, StarFighter>();
        bool newPlayer = false;

        public static new ConcurrentDictionary<string, IGameBehavior> gameObjects = new ConcurrentDictionary<string, IGameBehavior>();
        public bool gameActive = true;
        public Delay gameWaitTiming = new Delay(100);

        public Task[] tasks = new Task[1];
      //  public ConcurrentDictionary<string, int> scores = new Dictionary<string, int>();


        public void ConnectNewPlayingClient()
        {
                   try
                   {
                        while (true)
                       {
                           Console.WriteLine("Checking for playing clients");
                           try
                           {
                               tasks[0] = Task.Run(() =>
                               {
                                   foreach (var client in ServerSide.ConnectClients.ConnectedClients.Values)
                                   {
                                      if (client.Connected)
                                       {
                                           try
                                           {
                                               NetworkStream ns = client.GetStream();
                                               //byte[] b = new byte[sizeof(short)];
                                               //ns.Read(b, 0, b.Length);
                                               //var message = (GameAction)BitConverter.ToInt16(b, 0);
                                               var message = ServerSide.RecvCommand(ns);

                                               if (message == GameAction.Ready)
                                               {
                                                   var key = ServerSide.RecvShortString(ns);
                                                   client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                                                   Console.WriteLine("client address: " + client.Client.RemoteEndPoint.ToString() + "name:" + key + " playing");
                                                   PlayingClients.TryAdd(key, new ClientMember(client, key,client.GetStream()));
                                                   Console.WriteLine("initing " + key + " background transfer");
                                                   InitClient(key);
                                                   break;
                                               }
                                           }
                                           catch { Console.WriteLine("problem with user playing"); }
                                       }
                                   }
                               });
                           }
                           catch { Console.WriteLine("error"); }

                           Thread.Sleep(5000);
                       }
                   }
                   catch { Console.WriteLine("Problem with while"); }
               }

            
        public override void Init()
        {
            base.Init();
            BackColor = Color.Black;
            var by = new char[5];

            //for (int i = 0; i < 15; i++)
            //{
            //    var len = MathAssist.RandInt(15, 80);
            //    var size = new SizeF(len, len);
            //    var location = new Vector2F(MathAssist.RandInt(0, GameWindow.DisplaySize.Width), MathAssist.RandInt(0, GameWindow.DisplaySize.Height));
            //    var angle = MathAssist.RandInt(0, 359);
            //    var velocity = MathAssist.RandInt(1, 3);
            //    var asteroid = new Asteroid(Color.Gray, size, location, angle, velocity);
            //    asteroid.Init();
            //    gameObjects.Add("Asteroid" + i, asteroid);
            //}

            for (int i = 0; i < 200; i++)
            { 
                var radius = MathAssist.RandInt(1, 3);
                var color = Color.FromArgb(MathAssist.RandInt(50, 200), MathAssist.RandInt(50, 200), MathAssist.RandInt(50, 200));
                var rect = new Rectangle(MathAssist.RandInt(0, Screen.PrimaryScreen.Bounds.Width), MathAssist.RandInt(0, Screen.PrimaryScreen.Bounds.Height), radius, radius);
               var star = new Star(color, rect, MathAssist.RandInt(1, 2), MathAssist.RandInt(1, 2));
                gameObjects.TryAdd("star" + i, star);
            }

         //   new Thread(new ThreadStart( ConnectNewPlayingClient)).Start(); 
           // ConnectNewPlayingClient();
            foreach (var obj in gameObjects)
                obj.Value.Init();

        }

        public void InitClient(string key)
        {
            var fighterObj = new StarFighter(Color.Red, new SizeF(10f, 10f), new Vector2F(200, 200), 0);
            fighterObj.Init();
            fighterObj.ID = key;
            fighter = new KeyValuePair<string, StarFighter>(key, fighterObj);
            Console.WriteLine("sending " + key + " objects");

            try
            {
                foreach (var obj in gameObjects)
                {
                    string typeName = obj.Value.GetType().Name.ToString();
                    Console.WriteLine("sending to " + key + " " + " of type" + typeName + " " + obj.ToString());

                    var item = ((IGame2DProperties)obj.Value);
                    var dat = obj.Key + "," + item.Location.X + "," + item.Location.Y;
                    var type = (GameAction)Enum.Parse(typeof(GameAction), typeName);
                    ServerSide.SendData(type, dat, PlayingClients[key].Ns);
                }
                ServerSide.SendCommand(GameAction.Done, PlayingClients[key].Ns);
            }
            catch
            {
                
            }

            PlayingClients[key].status = "loaded";

        }

        public override void Update(GameTime gameTime)
        {

            foreach (var client in ServerSide.ConnectClients.ConnectedClients.Values)
                                   {
                                       if (client.Connected)
                                       {
                                           try
                                           {
                                               NetworkStream ns = client.GetStream();      
                                               var message = ServerSide.RecvCommand(ns);

                                               if (message == GameAction.Ready)
                                               {
                                                   var key = ServerSide.RecvShortString(ns);

                                                   if (!PlayingClients.ContainsKey(key))
                                                   {
                                                       client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                                                       Console.WriteLine("client address: " + client.Client.RemoteEndPoint.ToString() + "name:" + key + " playing");
                                                       PlayingClients.TryAdd(key, new ClientMember(client, key, client.GetStream())); //, sw, sr));
                                                       Console.WriteLine("initing " + key + " background transfer");
                                                       InitClient(key);


                                                       var pt = Parallel.ForEach<ClientMember>(PlayingClients.Values, (cl) =>
                                                       {
                                                           if (fighter.Value != null)
                                                           {
                                                               var datString = key + "," + fighter.Value.Angle + "," + fighter.Value.Location.X + "," + fighter.Value.Location.Y + ",";
                                                               gameObjects.TryAdd(key, fighter.Value);
                                                               ServerSide.SendData(GameAction.NewPlayer, datString, cl.Ns);
                                                           }
                                                       });

                                                       while (!pt.IsCompleted) { }

                                                       break;
                                                   }
                                               }
                                               //   ns.Close();
                                           }
                                           catch { Console.WriteLine("problem"); }
                                       }
                                   }



         
            

            var p = Parallel.ForEach<ClientMember>(PlayingClients.Values, (client) =>
            {
                if (client.status == "loaded")
                {
                    try
                    {
                        client.Ns = client.Client.GetStream();
                    }
                    catch { }
                
                    if (client.Ns.DataAvailable)
                    {
                        try
                        {
                            var message = ServerSide.RecvCommand(client.Ns);
                            var key = ServerSide.RecvShortString(client.Ns);

                            if (message == GameAction.Accelerate)
                            {
                                ((StarFighter)gameObjects[key]).Accelerate();
                            }
                            if (message == GameAction.Deaccelerate)
                            {
                                ((StarFighter)gameObjects[key]).Deaccelerate();
                            }
                            if (message == GameAction.TurnLeft)
                            {
                                ((StarFighter)gameObjects[key]).TurnLeft();
                            }
                            if (message == GameAction.TurnRight)
                            {
                                ((StarFighter)gameObjects[key]).TurnRight();
                            }

                            client.ns.Read(new byte[ClientSide.Client.ReceiveBufferSize], 0, ClientSide.Client.ReceiveBufferSize);
                        }
                        catch { }
                    }

                    var fighters = gameObjects.Keys.OfType<StarFighter>();

                    foreach (var fighter in fighters)
                    {
                        var datString = fighter.ID + "," + fighter.Angle + "," + fighter.Location.X + "," + fighter.Location.Y + ",";
                        ServerSide.SendData(GameAction.Update, datString, client.Ns);
                    }
                }

            });

            while (!p.IsCompleted)
            {

            }
            newPlayer = false;

            foreach (var obj in gameObjects)
                obj.Value.Update(gameTime);


            //foreach (var player in gameObjects.Values.OfType<StarFighter>())
            //{
            //    foreach (var bullet in gameObjects.Values.OfType<Projectile>())
            //    {
            //        var delx = player.Size.Width;
            //        var dely = player.Size.Height;
            //        if (bullet.InBounds(new BoundF(player.Location.X - delx, player.Location.Y - dely, player.Location.X + delx, player.Location.Y + dely)))
            //        {
            //            if (!bullet.ID.Contains(player.ID))
            //            {
            //                var id = bullet.ID.Split(',').Last();
            //                bullet.ToBeDestroyed = true;
            //                if (player.IsDead.IsNotTrue())
            //                {
            //                    player.IsDead = true;
            //                    scores[id]++;
            //                }
            //            }
            //        }
            //    }

            //}


            //var keys = gameObjects.Keys.ToArray<string>();
            //for (int i = 0; i < keys.Count<string>(); i++)
            //{
            //    var ass = gameObjects[keys[i]];
            //    if (ass.GetType() == typeof(Asteroid))
            //    {
            //        //  ((IGame2DProperties)ass).Location = ((IGame2DProperties)ass).KeepInBounds(bound).Location;
            //    }

            //}


       


            //var fighters = gameObjects.Values.OfType<StarFighter>();
            //foreach (var client in PlayingClients)
            //{
            //    if (client.Value.status == "loaded")
            //    {
            //        foreach (var fighter in fighters)
            //        {
            //            SendString("upfi", client.Value.key, client.Value.key + "," + fighter.Angle + "," + fighter.Location.X + "," + fighter.Location.Y);
            //            // SendObject("upfi", client.Value.key, client.Value.key + "," + fighter.Angle + "," + fighter.Location.X + "," + fighter.Location.Y);
            //        }
            //    }
            //}

           // if (delClient)
            {
                var lst = new List<string>();
                foreach (var client in PlayingClients)
                    if (!client.Value.Client.Connected)
                    {
                        lst.Add(client.Key);
                        client.Value.status = "disconnected";
                        delClient = true;
                    }

                if (delClient)
                {

                    foreach (var key in lst)
                    {
                        var c = new ClientMember(null,"", null);
                        var f = default(IGameBehavior);
                        PlayingClients.TryRemove(key, out c);
                        if (gameObjects.ContainsKey(key))
                            gameObjects.TryRemove(key, out f);
                    }
                    //    delClient = false;

                    //  tasks[1] = Task.Run(() =>{
                    for (int i = 0; i < ServerSide.ConnectClients.ConnectedClients.Count; i++)
                    {
                        try
                        {
                            if (!ServerSide.ConnectClients.ConnectedClients[i].Connected)
                            {
                                // disconnect clients
                                var key = ServerSide.ConnectClients.ConnectedClients[i].Client.RemoteEndPoint.ToString();
                                var t = new TcpClient();
                                ServerSide.ConnectClients.ConnectedClients.TryRemove(i, out t);//.TryTake(out ServerSide.ConnectClients.ConnectedClients.ElementAt(i) );
                                i--;
                                Console.WriteLine("client " + key + " disconnected");

                            }
                        }
                        catch { }

                    }
                    delClient = false;
                }
                  //  Console.WriteLine("ConnectReadyCheck Close..");
               // });
            }

            if (GameInput.isKeyDown(Keys.Escape))
            {
                RunAgain = false;
                GameWindow.AudioPlayer.Item1.StopTrack("background");
                GameWindow.CurrentScene = NextScene;
                GameWindow.View.Init();
                gameObjects.Clear();
            }

        }

        public override void Draw(Graphics g)
        {
            //g.Clear(Color.Black);
            //foreach (var obj in gameObjects.Values.OfType<IViewable>())
            //    obj.Draw(g);
        }



        public void SendObject(string type, string key, dynamic obj)
        {
            try
            {
                SendCommand(type, key);
                //PlayingClients[key].Ns.Read(b, 0, 2);
                //var message = Encoding.ASCII.GetString(b);
                SendObject(key, obj);
            }
            catch
            {
                Console.WriteLine("could not send object " + obj.ToString());
            }
        }
        public void SendString(string command, string key, string str)
        {
            SendCommand(command, key);
            SendCommand(str, key);
        }
        private void SendObject(string key, dynamic obj)
        {
            if (!Object.Equals(obj, null))
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, obj);
                var tt = ms.ToArray();
                var size = BitConverter.GetBytes(tt.Length);
                PlayingClients[key].Ns.Write(size, 0, 4);
                PlayingClients[key].Ns.Write(tt, 0, tt.Length);
                PlayingClients[key].Ns.Flush();
            }
        }
        private void SendCommand(string type, string key)
        {
            try
            {
                var msg = type.ToBytes();
                PlayingClients[key].Ns.Write(BitConverter.GetBytes(msg.Length), 0, BitConverter.GetBytes(msg.Length).Length);
                PlayingClients[key].Ns.Write(msg, 0, msg.Length);
                PlayingClients[key].Ns.Flush();
            }
            catch
            {
                Console.WriteLine("unabled to send to player");
            }
        }
    }
}
