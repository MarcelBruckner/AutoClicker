using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoClicker.Instructions
{
    class MouseClick : Instruction
    {
        public const int MAX_UNCERTAINTY = 10;

        public MouseClick() : this(0, 0, 0, 0, 1, false, false, false) { }

        public MouseClick(int button, int x, int y, int delay, int repetitions, bool shift, bool ctrl, bool alt) : base(Action.CLICK, delay, repetitions, shift, ctrl, alt)
        {
            Button = button;
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            string s = base.ToString() + " ";

            if (Button != 0)
            {
                s += Property.BUTTON.ToString() + "=" + Button + " ";
            }
            return s += Property.X.ToString() + "=" + X + " " +
               Property.Y.ToString() + "=" + Y;
        }

        public override bool Equals(object obj)
        {
            return obj is MouseClick click &&
                   base.Equals(obj) &&
                   Button == click.Button &&
                   Distance(click) < MAX_UNCERTAINTY;
        }



        public int Distance(MouseClick other)
        {
            return (int)Math.Sqrt((X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y));
        }

        public override int GetHashCode()
        {
            var hashCode = -1406273814;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Button.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}