using Framework2D.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D
{
    public interface IGameBehavior: IKillable
    {
       // string ID { get; set; }
        void Init();
        void Update(GameTime gameTime);
    }
}
