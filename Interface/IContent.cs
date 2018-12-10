using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D
{
   public interface IContent<Tin>
    {
        IDictionary<string, Tin> Assets { get; set; }
        void LoadAsset();
    }
}
