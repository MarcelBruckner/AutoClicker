using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoClicker.Instructions
{
    class MouseDrag : MouseClick
    {
        public Point End { get; private set; }

        public MouseDrag() : this(0, 0, 0,0, 0, 0, 1) { }

        public MouseDrag(int button, int x, int y, int endX, int endY, int delay, int repetitions) : base(button, x, y, delay, repetitions)
        {
            Type = Action.DRAG;
            End = new Point(endX, endY);
        }

        protected override void SpecificExecute()
        {
            Console.WriteLine("Doing drag");

            InputSimulator.MouseDown(Position, Button);
            InputSimulator.MouseUp(End, Button);
        }
        
        public override bool Equals(object obj)
        {
            var drag = obj as MouseDrag;
            return drag != null &&
                   base.Equals(obj) &&
                   EqualityComparer<Point>.Default.Equals(End, drag.End);
        }

        public override int GetHashCode()
        {
            var hashCode = -188397344;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(End);
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString() + " " +
                Property.END_X + "=" + End.X + " " +
                Property.END_Y + "=" + End.Y;
        }
    }
}