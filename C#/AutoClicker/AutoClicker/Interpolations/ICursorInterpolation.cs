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

        public bool Finished { get => Distance(Cursor, End) < 3; }

        protected ICursorInterpolation(System.Drawing.Point end, double speed = 1)
        {
            End = new Vector(end.X, end.Y);
            Speed = speed;
        }

        public abstract void Interpolate();

        protected Vector Cursor { get => PointToVector(System.Windows.Forms.Cursor.Position); set => System.Windows.Forms.Cursor.Position = VectorToPoint(value); }

        #region Helpers
        public static double Distance(Vector a, Vector b)
        {
            return (a - b).Length;
        }

        public static double Distance(System.Drawing.Point a, Vector b)
        {
            return Distance(PointToVector(a), b);
        }

        public static Vector PointToVector(System.Drawing.Point p)
        {
            return new Vector(p.X, p.Y);
        }

        public static System.Drawing.Point VectorToPoint(System.Windows.Vector v)
        {
            return new System.Drawing.Point((int)v.X, (int)v.Y);
        }

        public static double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
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
