using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace AutoClicker
{
    public class SinusInterpolation : ICursorInterpolation
    {
        public double step = 50.0;

        public SinusInterpolation(System.Drawing.Point end, double speed = 1) : base(end, speed) { }

        public override void Interpolate()
        {
            Vector origin = Cursor.Vector;
            Vector b = End - origin;

            double duration = b.Length / 10 * Speed;
            if (b.Length > 1)
            {
                double angle = -Vector.AngleBetween(new Vector(1, 0), b);
                double backAngle = ConvertToRadians(-angle);

                double interval = b.Length; //Math.Cos(ConvertToRadians(angle)) * b.X + -Math.Sin(ConvertToRadians(angle)) * b.Y;

                List<double> coefficients = new List<double>() { };

                int swings = 5;

                for (int i = 1; i < swings; i++)
                {
                    coefficients.Add((random.NextDouble() * 2 - 1) / i);
                }

                for (double x = 1 / step; x < 1.0; x += 1 / step)
                {
                    double y = 0;
                    for (int i = 0; i < coefficients.Count; i++)
                    {
                        y += coefficients[i] * Math.Sin(x * (i + 2) * Math.PI);
                    }

                    y *= random.NextDouble() * 15 + 5;
                    Vector current = new Vector(
                        Math.Cos(backAngle) * x * interval + -Math.Sin(backAngle) * y,
                        Math.Sin(backAngle) * x * interval + Math.Cos(backAngle) * y);

                    current += origin;
                    Cursor.Vector = current;
                    Thread.Sleep((int)(duration / step));
                }
            }

            Cursor.Vector = End;
        }
    }
}
