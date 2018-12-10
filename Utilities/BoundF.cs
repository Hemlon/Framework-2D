using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D.Utilities
{
    [Serializable]
    public class BoundF
    {
        public Vector2F Min = new Vector2F(0,0);
        public Vector2F Max = new Vector2F(0, 0);

        public BoundF(float xmin, float ymin, float xmax ,float ymax)
        {
            Min.X = xmin; Min.Y = ymin; Max.X = xmax; Max.Y = ymax;
        }
       
    }
}
