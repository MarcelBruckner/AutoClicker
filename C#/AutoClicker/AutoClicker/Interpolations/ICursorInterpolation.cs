using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace AutoClicker
{
    public abstract class ICursorInterpolation
    {
        protected static Random random = new Random();
        public Vector End { get; protected set; }
        public double Dt { get; set; }
        public double Speed { get; set; }
        
        public bool Finished { get => Cursor.Distance(End) < 3; }

        protected ICursorInterpolation(System.Drawing.Point end, double speed = 1)
        {
            End = new Vector(end.X, end.Y);
            Speed = speed;
        }

        public abstract void Interpolate();

        #region Helpers

        public static double ConvertToRadians(double angle)
        {
            return Math.PI / 180 * angle;
        }

        public static Vector RandomVector(int maxLength)
        {
            double angle = ConvertToRadians(random.Next(360));
            return new Vector(Math.Sin(angle), Math.Cos(angle)) * random.Next(0, maxLength);
        }

        public static void PrintVector(string message, Vector v)
        {
            Console.WriteLine(message + ": " + v.X + " - " + v.Y);
        }

        public static Vector[] Normals(Vector a, Vector b)
        {
            Vector[] normals = new Vector[2];

            Vector r = a - b;

            normals[0] = new Vector(r.Y, -r.X) / r.Length;
            normals[1] = new Vector(-r.Y, r.X) / r.Length;

            return normals;
        }

        public static Vector RandomNormal(Vector a, Vector b, int maxLength)
        {
            return Normals(a, b)[(int)Math.Round(random.NextDouble())] * random.NextDouble() * maxLength;
        }
        #endregion
    }
}
