using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework2D;
using Framework2D.Scenes.StarFighters;
using System.Drawing;
using Framework2D.Interface;
using Framework2D.Utilities;

namespace Framework2D
{
    public static class ExtensionMethods
    {
        public static Tout ToType<Tout>(this System.Object item)
        {
            return (Tout)Convert.ChangeType(item, typeof(Tout));
        }
        public static void SetByKeyTo<TKey, TVal>(this TKey key, IDictionary<TKey, TVal> dic, TVal value )
        {
            if (!dic.ContainsKey(key))
                dic.Add(key, value);
            else
                dic[key] = value;
        }

        public static TVal GetByKey<TKey, TVal>(this TKey key, IDictionary<TKey, TVal> dic)
        {
            if (dic.ContainsKey(key))
                return (TVal)dic[key];
            else
                return default(TVal);             
        }

        public static bool GetByKeyFrom<TKey>(this TKey key, IDictionary<TKey, bool> dic)
        {
            if (dic.ContainsKey(key))
                return dic[key];
            else
                return false;
        }
        public static Size ToPoint(this SizeF p)
        {
            return new Size(p.Width.ToType<int>(), p.Height.ToType<int>());
        }
        public static PointF ToPointF( this Vector2F v)
        {
            return new PointF(v.X, v.Y);
        }
        public static Point ToPoint (this Vector2F v)
        {
            return new Point((int)v.X, (int)v.Y);
        }

        public static byte[] ToBytes(this string p)
        {
            return ASCIIEncoding.ASCII.GetBytes(p);
        }
        public static Vector2F ToVector2F(this Point p)
        {
            return new Framework2D.Vector2F(p.X, p.Y);
        }
        public static bool IsNotTrue(this bool item)
        {
            return !item;
        }
        public static IGame2DProperties KeepInBounds(this IGame2DProperties obj, BoundF b)
        {
            if (obj.Location.X < b.Min.X) { obj.Location.X = b.Max.X; }
            else if (obj.Location.X > b.Max.X) { obj.Location.X = b.Min.X; }
            if (obj.Location.Y < b.Min.Y) { obj.Location.Y = b.Max.Y; }
            else if (obj.Location.Y > b.Max.Y)
            {
                obj.Location.Y = b.Min.Y;
            }
            return obj;
        }        
        public static double angle(this Vector2F item)
        {
            var ang = Math.Atan(item.Y / item.X);

            if (item.Y > 0 && item.X < 0)
                ang = 180 - ang;
            else if (item.Y < 0 && item.X < 0)
                ang = 180 + ang;
            else if (item.Y > 0 && item.X > 0)
                ang = 360 - ang;
            else if (item.Y == 0 && item.X > 0)
                ang = 0;
            else if (item.Y == 0 && item.X < 0)
                ang = 180;
            else if (item.Y > 0 && item.Y == 0)
                ang = 270;
            else if (item.Y < 0 && item.Y == 0)
                ang = 90;
            return ang;


        }      
        public static bool InBounds(this IGame2DProperties obj, BoundF b)
        {
            var inside = false;
            
            if (obj.Location.X >= b.Min.X && obj.Location.X <= b.Max.X)
            {
                if (obj.Location.Y >= b.Min.Y && obj.Location.Y <= b.Max.Y)
                {
                    inside = true;
                }
            }
            return inside;
        }

        public static double ToDegrees(this double a)
        {
            return a / Math.PI * 180;
        }
        public static double ToRadians(this double a)
        {
            return a * Math.PI / 180;
        }
        public static Complex ShowCartesian(this Complex c)
        {
            var sign = c.Imz < 0 ? " - " : " + ";
            Console.WriteLine(c.Real + sign + Math.Pow(Math.Pow(c.Imz, 2), 0.5) + "i");
            return c;
        }
        public static double ToConsole(this double d)
        {
            Console.WriteLine(d);
            return d;
        }
        public static T ChangeBy<T>(this T item, T dx, Predicate<T> ifThisIsTrue, T defaultTo)
        {
            dynamic a = item; dynamic b = dx;
            return ifThisIsTrue(item) ? a+b : defaultTo;
        }
    }
}
