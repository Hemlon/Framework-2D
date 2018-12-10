using Framework2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D
{
   public class Content : IContent<IContent<string>>
    {
        public IDictionary<string, IContent<string>> Assets
        {
            get;
            set;
        }
        public Content()
        {
            Assets = new Dictionary<string, IContent<string>>();
            Assets.Add("Audio", new AudioContent());
            Assets.Add("Graphics", new GraphicContent());
        }

        public void LoadAsset()
        {
            Assets["Audio"].LoadAsset();
            Assets["Graphics"].LoadAsset();
        }
    }
}
