using Framework2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Framework2D
{
    public class AudioContent : IContent<string>
    {
        public IDictionary<string, string> Assets
        {
            get;
            set;
        }

        public AudioContent()
        {
            Assets = new Dictionary<string, string>();
        }
        public void LoadAsset()
        {
            foreach (var media in Assets)
            {
                 GameWindow.AudioPlayer.Item1.LoadMediaFile(media.Value, media.Key);
              //  GameWindow.GameAudio.Player.Item1.LoadMediaFile(media.Value, media.Key);
            }
        }
    }
}
