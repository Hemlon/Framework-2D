using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D
{
   public class GameTime
    {
        private float timeInterval = 0.000001f;
        public float TotalTime = 0f;
        public float TotalFrames = 0;
        public float FrameDuration;
        
        public GameTime(int Hertz)
        {
            FrameDuration = Hertz;
        }

        public void NextFrame()
        {
            TotalFrames += timeInterval;
            TotalTime = TotalFrames * FrameDuration;
        }      

    }
}
