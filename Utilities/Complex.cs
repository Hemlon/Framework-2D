using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D.Utilities
{
    public class Complex
    {
        double real;
        double imz;

        public double Real
        {
            get
            {
                return real;
            }

            set
            {
                real = value;
            }
        }
        public double Imz
        {
            get
            {
                return imz;
            }

            set
            {
                imz = value;
            }
        }
        public Complex(double real, double imz)
        {
            Real = real; Imz = imz;
        }
        public Complex(Complex complex)
        {
            Real = complex.Real;
            Imz = complex.Imz;
        }
        public static Complex cis(double angle)
        {
            return new Complex(Math.Cos(angle), Math.Sin(angle));
        }
        public double GetRadius()
        {
            return Mag(this);
        }
        public static Complex operator *(double r, Complex complex)
        {
            return new Complex(complex.Real * r, complex.Imz * r);
        }
        public static Complex operator *( Complex complex, double r)
        {
            return new Complex(complex.Real * r, complex.Imz * r);
        }
        public static Complex operator *(Complex c1, Complex c2)
        {
            var real = (c1.Real * c2.Real - c1.Imz * c2.Imz);
            var imz = (c1.Imz * c2.Real + c1.Real * c2.Imz);
            return new Complex(real, imz);
        }
        public static Complex operator +(Complex c1, Complex c2)
        {
            return new Complex(c1.Real + c2.Real, c1.Imz + c2.Imz);
        }
        public static Complex operator -(Complex c1, Complex c2)
        {
            return new Complex(c1.Real - c2.Real, c1.Imz - c2.Imz);
        }
        public static Complex operator /(Complex c1, Complex c2)
        {
            var div = Math.Pow(c2.Real, 2) + Math.Pow(c2.Imz, 2);
            var real = (c1.Real * c2.Real + c1.Imz * c2.Imz);
            var imz = (c1.Imz * c2.Real - c1.Real * c2.Imz);
            return new Complex(real / div, imz / div);
        }
        public static Complex operator /(Complex c2, double r)
        {
            return new Complex(c2.Real / r, c2.Imz / r);
        }
        public static Complex operator /(double r, Complex c2)
        {
            return new Complex(r, 0) / c2;
        }

        public static Complex Sin(Complex c)
        {
            return new Complex(Math.Sin(c.Real) * Math.Cosh(c.Imz), Math.Cos(c.Real) * Math.Sinh(c.Imz));
        }
        public static Complex Asin(Complex c)
        {
            //var ix = new Complex(-c.Imz, c.Real);
            //var xsq = c * c;
            //var oneminusxsq = new Complex(1, 0) - xsq;
            //var r = Math.Pow(oneminusxsq.GetMag(), 0.5);
            //var ang = oneminusxsq.GetArgz() / 2;
            //var sqrt = r*Complex.cis(ang);
            //return (ix + sqrt);
            var r = c.Real;
            var i = c.Imz;
            var a = Math.Pow((r * r + 2 * r + i * i + 1), 0.5);
            var b = Math.Pow((r * r - 2 * r + i * i + 1), 0.5);
            var real = Math.Asin(a / 2 - b / 2);
            var inner = (Math.Pow(2*(a * b+r*r +i*i -1),0.5) + a + b)/2;
            var imz = -Math.Log(inner)*MathAssist.sign(i);
            return new Complex(real, imz);

        }
        public static double Mag(Complex c)
        {
            return Math.Pow(Math.Pow(c.Real, 2) + Math.Pow(c.Imz, 2), 0.5);
        }
        public static double Argz(Complex c)
        {
            return Math.Atan(c.Imz / c.Real);
        }
        public double GetMag()
        {
            return Mag(this);
        }
        public double GetArgz()
        {
            return Argz(this);
        }
        public Complex ShowPolar()
        {
            Console.WriteLine(GetMag() + "cis" + GetArgz().ToDegrees());
            return this;
        }


    }
}
