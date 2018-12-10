using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D
{
    public class GraphicContent : IContent<string>
    {
        public IDictionary<string, string> Assets
        {
            get;
            set;
        }

        public GraphicContent() {
                Assets = new Dictionary<string, string>();
            }

        public void LoadAsset()
        {
            
        }
    }


    
}
