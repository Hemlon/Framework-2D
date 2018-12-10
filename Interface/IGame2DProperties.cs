using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D.Interface
{
   public interface IGame2DProperties
    {
        Vector2F Location { get; set; }
        SizeF Size { get; set; }
    }
}
