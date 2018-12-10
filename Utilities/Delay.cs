using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D
{
    [Serializable]
    public class Delay
    {
        private int count = 0;
        private int delayDuration;
        private bool isCompleted;
        public bool IsCompleted
        {
            get
            {
                return isCompleted;
            }

            set
            {
                isCompleted = value;
            }
        }
        public Delay(int delayDuration)
        {
            this.delayDuration = delayDuration;
        }
        public void Set(int delayDuration)
        {
            this.delayDuration = delayDuration;
        }
        public void Update()
        {
            count += 1;
            if (count > delayDuration)
            {
                IsCompleted = true;
            }
        }
        public void Reset()
        {
            count = 0;
            isCompleted = false;
        }

        public void PeriodicallyDo(Action action)
        {
            Update();
            if (IsCompleted)
            {
                action();
                Reset();
            }
        }

        public void WaitThenDoOnce(Action action)
        {
            Update();
            if (IsCompleted)
                action();
        }
    }
}
