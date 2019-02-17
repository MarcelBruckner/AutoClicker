using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoClicker
{
    public class CubicSplines
    {
        public float A { get; private set; }
        public float B { get; private set; }
        public float C { get; private set; }
        public float D { get; private set; }

        private int _x1;
        private int _y1;
        private int _x2;
        private int _y2;

        private bool startEndDifferent;

        public CubicSplines(int x1, int y1, int x2, int y2, float m1, float m2)
        {
            CalculateDirection(x1, y1, x2, y2);

            if (startEndDifferent)
            {
                A = (m1 + m2 - 2 * ((_y2 - _y1) / (float)(_x2 - _x1))) / ((_x1 - _x2) - (_x1 - _x2));

                B = ((m2 - m1) / 2 * (_x2 - _x1)) - 1.5f * (_x1 + _x2) * A;

                C = m1 - 3 * _x1 * _x1 * A - 2 * _x1 * B;

                D = _y1 - _x1 * _x1 * _x1 * A - _x1 * _x1 * B - _x1 * C;
            }
        }
        
        private void CalculateDirection(int x1, int y1, int x2, int y2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;

            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if(dx > 0)
                {
                    this._x1 = x1;
                    this._y1 = y1;
                    this._x2 = x2;
                    this._y2 = y2;
                }
                else
                {
                    this._x1 = x2;
                    this._y1 = y2;
                    this._x2 = x1;
                    this._y2 = y1;
                }
                startEndDifferent = true;
            }
            else if(Math.Abs(dx) < Math.Abs(dy))
            {
                if (dy > 0)
                {
                    this._x1 = y1;
                    this._y1 = x1;
                    this._x2 = y1;
                    this._y2 = x2;
                }
                else
                {
                    this._x1 = y2;
                    this._y1 = x2;
                    this._x2 = y1;
                    this._y2 = x1;
                }
                startEndDifferent = true;
            }
            else
            {
                startEndDifferent = false;
            }
        }
    }
}