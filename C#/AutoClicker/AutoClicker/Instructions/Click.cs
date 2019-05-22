using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoClicker.Instructions
{
    public class Click : Instruction
    {
        #region Properties
        public ButtonType? Button { get; set; }
        public MovementType? Movement { get; set; }

        public int X { get; set; } = 0;
        public int? RandomX { get; set; } 
        public int Y { get; set; } = 0;
        public int? RandomY { get; set; }

        private Vector Randomized { get; set; } = new Vector(-100, -100);
        #endregion

        #region Constructors
        public Click() : base() { }

        public Click(int x, int y, int? randomX = null, int? randomY = null, ButtonType? button = null, MovementType? movement = null,
            int? delay = null, int? randomDelay = null,
            int? repetitions = null, int? randomRepetitions = null,
            double? speed = null, double? randomSpeed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : base(delay, randomDelay, repetitions, randomRepetitions, speed, randomSpeed, shift, ctrl, alt)
        {
            X = x;
            Y = y;
            RandomX = randomX;
            RandomY = randomY;

            Button = button;
            Movement = movement;
        }
 
        #endregion

        internal override void SpecificExecute()
        {
            if (!AlreadyThere)
            {
                Randomized = new Vector(Randomize(X, RandomX ?? MainWindow.GlobalRandomX), Randomize(Y, RandomY ?? MainWindow.GlobalRandomY));
            }
            InputSimulator.MouseClick(Button ?? ButtonType.LEFT, GetHotkeys());
        }

        public override bool Equals(object obj) => base.Equals(obj) &&
                obj is Instructions.Click other &&
                ClickSame(other);
        
        public override int GetHashCode()
        {
            var hashCode = 1512054787;
            hashCode = hashCode * -1521134295 + Button.GetHashCode();
            hashCode = hashCode * -1521134295 + Movement.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + RandomX.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + RandomY.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("mouse:");
            Append(builder, "x", X, RandomX);
            Append(builder, "y", Y, RandomY);
            Append(builder, "button", Button);
            Append(builder, "move", Movement);
            return builder.ToString() + base.ToString();
        }

        #region Helpers
        private bool AlreadyThere
        {
            get
            {
                int dx = (int)Math.Abs(Randomized.X - Cursor.Vector.X);
                int dy = (int)Math.Abs(Randomized.Y - Cursor.Vector.Y);

                return dx < RandomX && dy < RandomY;
            }
        }


        public bool ClickSame(Click other) => Button == other.Button && Distance(other) < MAX_UNCERTAINTY;

        public int Distance(Click other) => (int)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));

        #endregion

    }
}