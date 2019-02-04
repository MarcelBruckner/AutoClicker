using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowsInput;

namespace AutoClicker.Instructions
{
    class MouseClick : Instruction
    {
        public const int MAX_UNCERTAINTY = 10;

        public int Button { get; private set; }
        public Point Position { get; private set; }

        public MouseClick() : this(0,0,0, 0, 0, 1) { }

        public MouseClick(int button, int x, int y, int[] standards) : this(button, x, y, standards[0], standards[1], standards[2]) { }

        public MouseClick(int button, int x, int y, int delayPrevious, int delayAfter, int repetitions) : base(InstructionType.CLICK, delayPrevious, delayAfter, repetitions)
        {
            Button = button;
            Position = new Point(x, y);
        }

        public MouseClick(Dictionary<string, string> raw) : base(raw) { }

        protected override void SpecificExecute()
        {
            Console.WriteLine("Doing click");

            MouseSimulator.MouseClick(Position, Button);
        }

        protected override void Parse(Dictionary<string, string> raw)
        {
            base.Parse(raw);

            bool hasButton = raw.ContainsKey(InstructionProperty.BUTTON.ToString());
            bool hasX = raw.ContainsKey(InstructionProperty.X.ToString());
            bool hasY = raw.ContainsKey(InstructionProperty.Y.ToString());

            if (!hasButton || !hasX || !hasY)
            {
                MissingPropertyException e = new MissingPropertyException();

                if (!hasButton)
                {
                    e.AddMissingProperty(InstructionProperty.BUTTON.ToString(), "0");
                }
                if (!hasX)
                {
                    e.AddMissingProperty(InstructionProperty.X.ToString(), "0");
                }
                if (!hasY)
                {
                    e.AddMissingProperty(InstructionProperty.Y.ToString(), "0");
                }

                throw e;
            }

            Button = int.Parse(raw[InstructionProperty.BUTTON.ToString()]);
            Position = new Point(int.Parse(raw[InstructionProperty.X.ToString()]), int.Parse(raw[InstructionProperty.Y.ToString()]));
        }

        public override string ToString()
        {
            return base.ToString() + " " +
                InstructionProperty.BUTTON.ToString() + "=" + Button + " " +
                InstructionProperty.X.ToString() + "=" + Position.X + " " +
                InstructionProperty.Y.ToString() + "=" + Position.Y;
        }

        public override bool Equals(object obj)
        {
            var click = obj as MouseClick;
            return click != null &&
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