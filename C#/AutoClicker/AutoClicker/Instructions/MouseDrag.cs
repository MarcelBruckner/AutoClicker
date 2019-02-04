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

        public MouseDrag() : this(0, 0, 0,0,0, 0, 0, 1) { }

        public MouseDrag(int button, int x, int y, int endX, int endY, int[] standards) : this(button, x, y, endX, endY, standards[0], standards[1], standards[2]) { }

        public MouseDrag(int button, int x, int y, int endX, int endY, int delayPrevious, int delayAfter, int repetitions) : base(button, x, y, delayPrevious, delayAfter, repetitions)
        {
            Type = InstructionType.DRAG;
            End = new Point(endX, endY);
        }

        public MouseDrag(Dictionary<string, string> raw) : base(raw) { }

        protected override void SpecificExecute()
        {
            Console.WriteLine("Doing drag");

            MouseSimulator.MouseDown(Position, Button);
            MouseSimulator.MouseUp(End, Button);
        }

        protected override void Parse(Dictionary<string, string> raw)
        {
            base.Parse(raw);

            bool hasEndX = raw.ContainsKey(InstructionProperty.END_X.ToString());
            bool hasEndY = raw.ContainsKey(InstructionProperty.END_Y.ToString());

            if (!hasEndX || !hasEndY)
            {
                MissingPropertyException e = new MissingPropertyException();

                if (!hasEndX)
                {
                    e.AddMissingProperty(InstructionProperty.END_X.ToString(), "0");
                }
                if (!hasEndY)
                {
                    e.AddMissingProperty(InstructionProperty.END_Y.ToString(), "0");
                }

                throw e;
            }

            int endX = int.Parse(raw[InstructionProperty.END_X.ToString()]);
            int endY = int.Parse(raw[InstructionProperty.END_Y.ToString()]);
            End = new Point(endX, endY);
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
                InstructionProperty.END_X + "=" + End.X + " " +
                InstructionProperty.END_Y + "=" + End.Y;
        }
    }
}