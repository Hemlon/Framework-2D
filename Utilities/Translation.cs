using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D.Utilities
{
    public static class Translation
    {
        public static Vector2F Move2D(Vector2F v, float speed, double angle)
        {
            return new Vector2F(v.X + speed * (float)Math.Cos(angle * Math.PI / 180d), v.Y + speed * (float)Math.Sin(angle * Math.PI / 180d));
        }
    }
}
