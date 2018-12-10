using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D
{
    public static class MathAssist

    {

        static Random rand = new Random();

        public static Tin Upper<Tin>(Tin x, Tin limit)
        {
            if (Comparer<Tin>.Default.Compare(x, limit) > 0)
                return limit;
            else
                return x;
        }
        public static Tin Lower<Tin>(Tin x, Tin limit)
        {
            if (Comparer<Tin>.Default.Compare(x, limit) < 0)
                return limit;
            else
                return x;
        }
        public static Tin Between<Tin>(Tin x, Tin lowerlimit, Tin upperlimit)
        {
            x = Upper<Tin>(x, upperlimit);
            x = Lower<Tin>(x, lowerlimit);
            return x;

        }
        public static float Mag(float x, float y)
        {
            return (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        public static int RandInt(int min, int max)
        {
            return rand.Next(min, max);
        } 

        public static float Rand(float multiplier, float offset)
        {
            return rand.Next() * multiplier + offset;
        }

        public static double sign(double c)
        {
            return (c < 0) ? -1:1;
        }

    }
}
