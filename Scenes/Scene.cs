using Framework2D.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D
{
    public class Scene : IScene, IGameBehavior
    {
        public static Dictionary<string, Scene> Scenes = new Dictionary<string, Scene>();
        public string NextScene
        {
            get;

            set;
        }
        public bool RunAgain
        {
            get;

            set;
        }
        public Color BackColor { get; set; }
        public Dictionary<string, IGameBehavior> gameObjects
        {
            get;

            set;
        }
        public bool ToBeDestroyed
        {
            get;

            set;
        }
        public Scene(string nextScene)
        {
            RunAgain = false;
            this.NextScene = nextScene;
        
            gameObjects = new Dictionary<string, IGameBehavior>();
        }
        public virtual void Draw(Graphics g)
        {
            g.Clear(BackColor);

            foreach (var obj in gameObjects.Values.OfType<IViewable>())
            {
                if (obj.IsVisible)
                    obj.Draw(g);
            }
            
        }
        public virtual void Init()
        {
            RunAgain = true;

            foreach (var obj in gameObjects)
                obj.Value.Init();
        }
        public virtual void Update(GameTime gameTime)
        {
            foreach (var obj in gameObjects)
                obj.Value.Update(gameTime);
        }
    }
}
