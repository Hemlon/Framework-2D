using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D.Utilities
{
    public static class Change
    {
        public static double By(this double item, double dx, Predicate<double> ifThisIsTrue)
        {
            return ifThisIsTrue(item) ? item+dx :item; 
        }
    }
}
