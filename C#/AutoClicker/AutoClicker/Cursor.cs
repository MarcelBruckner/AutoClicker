using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace AutoClicker
{
    public static class Cursor
    {
        public static Vector Vector { get => PointToVector(Point); set => Point = VectorToPoint(value); }
        public static System.Drawing.Point Point { get => System.Windows.Forms.Cursor.Position; set => System.Windows.Forms.Cursor.Position = value; }

        public static double Distance(Vector b)
        {
            return (Vector - b).Length;
        }

        public static Vector PointToVector(System.Drawing.Point p)
        {
            return new Vector(p.X, p.Y);
        }

        public static System.Drawing.Point VectorToPoint(Vector v)
        {
            return new System.Drawing.Point((int)v.X, (int)v.Y);
        }
    }
}