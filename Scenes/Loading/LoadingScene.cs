using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework2D;
using Framework2D.Scenes.LoadingScene.SceneItems;
using Framework2D.Interface;

namespace Framework2D
{
    public class LoadingScene : Scene, IGameBehavior
    {
        public Content Content
        {
            get;
            set;
        }

        public PointF Location
        {
            get;

            set;
        }

        private ParallelLoopResult loading;

        private void assetLoading()
        {
            if (Content != null || Content.Assets.Count() > 0)
                loading = Parallel.ForEach(Content.Assets.Keys, (key) => { Content.Assets[key].LoadAsset(); });
        }

        public LoadingScene(string nextScene, Content content) :base(nextScene)
        {
            this.Content = content;
        }

        public override void Init()
        {
            ProgressBar progress = new ProgressBar(100, new PointF(100, 100), new SizeF(250, 50), Color.Blue, Color.Red);
            if (gameObjects.ContainsKey("progress").IsNotTrue())
                        gameObjects.Add("progress", progress);
            gameObjects["progress"].Init();
            loading = new ParallelLoopResult();
            assetLoading();
            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            var g = gameObjects["progress"].ToType<ProgressBar>();
            if (loading.IsCompleted)// && g.IsCompleted)
            {
                g.Reset();
                RunAgain = false;
                GameWindow.CurrentScene = NextScene;
            }

            base.Update(gameTime);
        }

    }
}
