using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework2D;

namespace Framework2D
{
    public interface IScene
    {
        bool RunAgain { get; set; }
        string NextScene { get; set; }
        Dictionary<string, IGameBehavior> gameObjects { get; set; }
        Color BackColor { get;set;}
    }
}
