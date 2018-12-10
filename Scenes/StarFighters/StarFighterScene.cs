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

namespace Framework2D.Scenes
{
    class StarFighterScene : Scene, IGameBehavior
    {
        string mainPlayerID = "";
        string secondPlayer = "";
        double angle = 0d;
        public StarFighterScene(string nextScene) : base(nextScene)
        {
        }
        public static new Dictionary<string, IGameBehavior> gameObjects = new Dictionary<string, IGameBehavior>();
        public bool gameActive = true;
        public Delay gameWaitTiming = new Delay(100);
        public Dictionary<string, int> scores = new Dictionary<string, int>();

        public override void Init()
        {
            base.Init();
            BackColor = Color.Black;
            GameWindow.AudioPlayer.Item1.PlayTrack("background");

            for (int i = 0; i < 15; i++)
            {
                var len = MathAssist.RandInt(15, 80);
                var size = new SizeF(len, len);
                var location = new Vector2F(MathAssist.RandInt(0, GameWindow.DisplaySize.Width), MathAssist.RandInt(0, GameWindow.DisplaySize.Height));
                var angle = MathAssist.RandInt(0, 359);
                var velocity = MathAssist.RandInt(1, 3);
                   var asteroid = new Asteroid(Color.Gray, size, location, angle, velocity);
                   asteroid.Init();
                   gameObjects.Add("asteroid" + i, asteroid);
            }

            for (int i = 0; i < 200; i++)
            {
                var radius = MathAssist.RandInt(1, 3);
                var color = Color.FromArgb(MathAssist.RandInt(50, 200), MathAssist.RandInt(50, 200), MathAssist.RandInt(50, 200));
                var rect = new Rectangle(MathAssist.RandInt(0, Screen.PrimaryScreen.Bounds.Width), MathAssist.RandInt(0, Screen.PrimaryScreen.Bounds.Height), radius, radius);
                gameObjects.Add("star" + i, new Star(color, rect, MathAssist.RandInt(1, 2), MathAssist.RandInt(1, 2)));
            }

            var f1 = new StarFighter(Color.Red, new SizeF(10f, 10f), new Vector2F(200, 200), 0);
            mainPlayerID = f1.ID;
            scores.Add(mainPlayerID, 0);
            var f2 = new StarFighter(Color.Blue, new SizeF(10f, 10f), new Vector2F(250, 250), 180);
            secondPlayer = f2.ID;
            scores.Add(secondPlayer, 0);
            gameObjects.Add(f1.ID, f1);
            gameObjects.Add(f2.ID, f2);
           //  GameWindow.View.FollowedObject = (IViewFollowed)f1;
            //  RenderManager.RenderList.Add("f1", new Graphic.Render(RenderType.Default, true, f.Location, null, f.Angle,new Vector2F(0,0), f.Size, null, new RectangleF(0,0,0,0), "", null, Color.Red, 1,DrawRoutine.DrawTriangle));
            gameObjects.Add("view", GameWindow.View);

            foreach (var obj in gameObjects)
                obj.Value.Init();
        }
        public override void Update(GameTime gameTime)
        {
            //  GameWindow.myText = new PointF(GameInput.GetMouseLocation().X - GameWindow.View.Location.X, GameInput.GetMouseLocation().Y - GameWindow.View.Location.Y).ToString();
            GameWindow.GameMessages[1] = ("objCount:" + gameObjects.Count.ToString());
            DestroyManager.DestroyFromList(ref gameObjects);
            DestroyManager.DestroyOutOfBounds(ref gameObjects, new Utilities.BoundF(0, 0, Screen.PrimaryScreen.Bounds.Size.Width, Screen.PrimaryScreen.Bounds.Size.Height));

            BoundF bound = new BoundF(
                               0,// GameWindow.View.Size.Width ,
                               0,// GameWindow.View.Size.Height / 4,
                                GameWindow.DisplaySize.Width,// - GameWindow.View.Size.Width / 4,
                                GameWindow.DisplaySize.Height// - GameWindow.View.Size.Height / 4
                             );

            if (gameObjects.ContainsKey(mainPlayerID))
            {
                var fighter = mainPlayerID.GetByKey<string, IGameBehavior>(gameObjects) as StarFighter;
              
                    if (GameInput.isKeyDown(Keys.Down))
                        // if (GetAsyncKeyState(Keys.Down))
                        fighter.Deaccelerate();
                    if (GameInput.isKeyDown(Keys.Up))
                        // if (GetAsyncKeyState(Keys.Up))
                        fighter.Accelerate();
                    if (GameInput.isKeyDown(Keys.Right))
                        // if (GetAsyncKeyState(Keys.Right))
                        fighter.TurnRight();
                    if (GameInput.isKeyDown(Keys.Left))
                        //  if (GetAsyncKeyState(Keys.Left))
                        fighter.TurnLeft();

                if (gameActive)
                {
                    if (GameInput.isKeyDown(Keys.Space))
                        // if (GetAsyncKeyState(Keys.Space))
                        fighter.Shoot();
                }

                fighter.Location = ((IGame2DProperties)fighter)
                      .KeepInBounds(bound
                     ).Location;
            }


            if (gameObjects.ContainsKey(secondPlayer))
            {

                var fighter2 = secondPlayer.GetByKey<string, IGameBehavior>(gameObjects) as StarFighter;
          
                    if (GameInput.isKeyDown(Keys.W))
                        fighter2.Accelerate();
                    if (GameInput.isKeyDown(Keys.S))
                        fighter2.Deaccelerate();
                    if (GameInput.isKeyDown(Keys.D))
                        fighter2.TurnRight();
                    if (GameInput.isKeyDown(Keys.A))
                        fighter2.TurnLeft();
                if (gameActive)
                {
                    if (GameInput.isKeyDown(Keys.Q))
                        fighter2.Shoot();
                }

                fighter2.Location = ((IGame2DProperties)fighter2).KeepInBounds(bound).Location;
            }


            foreach (var player in gameObjects.Values.OfType<StarFighter>())
            {
                foreach (var bullet in gameObjects.Values.OfType<Projectile>())
                {
                    var delx = player.Size.Width;
                    var dely = player.Size.Height;
                    if (bullet.InBounds(new BoundF(player.Location.X - delx, player.Location.Y - dely, player.Location.X + delx, player.Location.Y + dely)))
                    {
                        if (!bullet.ID.Contains(player.ID))
                        {
                            var id = bullet.ID.Split(',').Last();
                            bullet.ToBeDestroyed = true;
                            if (player.IsDead.IsNotTrue())
                            {
                                player.IsDead = true;
                                scores[id]++;
                            }
                        }
                    }
                }

            }

            angle += 1;
            if (angle > 360) angle = 0;
            var dx = 500 + 200 * Math.Cos(angle * Math.PI / 180f);
            GameWindow.View.Size = new SizeF((float)dx,(float)dx);

            var keys = gameObjects.Keys.ToArray<string>();
            for( int i = 0; i < keys.Count<string>(); i++)
            {
                var ass = gameObjects[keys[i]];
                if (ass.GetType() == typeof(Asteroid))
                {
                    ((IGame2DProperties)ass).Location = ((IGame2DProperties)ass).KeepInBounds(bound).Location;
                }
             
            }


            foreach (var obj in gameObjects)
                obj.Value.Update(gameTime);

            if (((StarFighter)gameObjects[mainPlayerID]).IsDead || ((StarFighter)gameObjects[secondPlayer]).IsDead)
            {
                gameActive = false;
                gameWaitTiming.WaitThenDoOnce(() =>
                {
                    foreach (var fighter in gameObjects.Values.OfType<StarFighter>())
                    {
                        var v = new Vector2F(MathAssist.RandInt(0, (GameWindow.DisplaySize.Width - 50)), MathAssist.RandInt(0, GameWindow.DisplaySize.Height));
                        fighter.Location = v;
                        fighter.IsDead = false;
                      
                        fighter.StopMoving();
                    }
                    gameActive = true;
                    gameWaitTiming.Reset();

                });
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
            //  base.Draw(g);
            g.Clear(Color.Black);
            foreach (var obj in gameObjects.Values.OfType<IViewable>())
               obj.Draw(g);

            var offset = 0f;

            foreach (var score in scores.Values)
            {
                g.DrawString(score.ToString(), new Font("Arial", 30), new SolidBrush(Color.White), new PointF(50f+offset, 50f));
                offset += 400f;
            }

        }

    }
}
