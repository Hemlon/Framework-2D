using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D
{
    [Serializable]
    public class Vector2F
    {
        public float X;
        public float Y;
        public Vector2F(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Vector2F operator +(Vector2F p1, Vector2F p2)
        {
            if (p2 != null)
                return new Vector2F(p1.X + p2.X, p1.Y + p2.Y);
            else
                return new Vector2F(p1.X, p1.X);
        }
        public static Vector2F operator -(Vector2F p1, Vector2F p2)
        {
            return new Vector2F(p1.X - p2.X, p1.Y - p2.Y);
        }

        public float Mag()
        { return (float)Math.Sqrt(Math.Pow(X.ToType<double>(), 2) + Math.Pow(Y.ToType<double>(), 2)); }
    }

}
