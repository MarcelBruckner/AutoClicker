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

        public int Button { get; private set; }
        public Point Position { get; private set; }

        public MouseClick() : this(0,0, 0, 0, 1) { }
        
        public MouseClick(int button, int x, int y, int delay, int repetitions) : base(Action.CLICK, delay, repetitions)
        {
            Button = button;
            Position = new Point(x, y);
        }

        protected override void SpecificExecute()
        {
            InputSimulator.MouseClick(Position, Button);
        }
        
        public override string ToString()
        {
            string s = base.ToString() + " ";

            if (Button != 0)
            {
                s += Property.BUTTON.ToString() + "=" + Button + " ";
            }
             return s += Property.X.ToString() + "=" + Position.X + " " +
                Property.Y.ToString() + "=" + Position.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is MouseClick click &&
                   base.Equals(obj) &&
                   Button == click.Button &&
                   Distance(click) < MAX_UNCERTAINTY;
        }

        public override int GetHashCode()
        {
            var hashCode = -1258872215;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Button.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(Position);
            return hashCode;
        }

        public int Distance(MouseClick other)
        {
            return (int) Math.Sqrt((Position.X - other.Position.X) * (Position.X - other.Position.X) + (Position.Y - other.Position.Y) * (Position.Y - other.Position.Y));
        }
    }
}