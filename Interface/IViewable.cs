using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D.Interface
{
   public interface IViewable
    {
        Color Color { get; set; }
        bool IsVisible { get; set; }
        float Alpha { get; set; }
        void Draw(Graphics g);
    }
}
