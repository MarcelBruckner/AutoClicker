using System;
using System.Text;
using System.Windows;

namespace AutoClicker.Instructions
{
    public class Click : Instruction
    {
        #region Properties
        public ButtonType? Button { get; set; }
        public MovementType? Movement { get; set; }

        public IntTuple X { get; set; }
        public IntTuple Y { get; set; }

        private System.Windows.Vector Randomized { get; set; } = new Vector(-100, -100);
        #endregion

        #region Constructors
        public Click() : base() { }

        public Click(int x, int y, ButtonType? button = null, MovementType? movement = null,
            IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : this(new IntTuple(x), new IntTuple(y), button, movement,
                delay, repetitions, speed, shift, ctrl, alt)
        { }

        public Click(IntTuple x, IntTuple y, ButtonType? button = null, MovementType? movement = null,
            IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : base(delay, repetitions, speed, shift, ctrl, alt)
        {
            X = x;
            Y = y;

            Button = button;
            Movement = movement;
        }

        public Click(IntTuple x, IntTuple y, ButtonType? button = null, MovementType? movement = null,
            Instruction instruction = null
            ) : this(x, y, button, movement ,instruction.Delay, instruction.Repetitions, instruction.Speed, instruction.Shift, instruction.Ctrl, instruction.Alt)
        { }

        #endregion

        internal override void SpecificExecute()
        {
            if (!AlreadyThere)
            {
                Randomized = new System.Windows.Vector(Randomize(X.Value, X.Random ?? MainWindow.GlobalRandomX), Randomize(Y.Value, Y.Random ?? MainWindow.GlobalRandomY));
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
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("click:");
            Append(builder, "x", X);
            Append(builder, "y", Y);
            Append(builder, "button", Button);
            Append(builder, "movement", Movement);
            return builder.ToString() + base.ToString();
        }

        #region Helpers
        private bool AlreadyThere
        {
            get
            {
                int dx = (int)Math.Abs(Randomized.X - Cursor.Vector.X);
                int dy = (int)Math.Abs(Randomized.Y - Cursor.Vector.Y);

                return dx < (X.Random ?? MainWindow.GlobalRandomX) && dy < (Y.Random ?? MainWindow.GlobalRandomY);
            }
        }


        public bool ClickSame(Click other) => Button == other.Button && Distance(other) < MAX_UNCERTAINTY;

        public int Distance(Click other) => (int)Math.Sqrt(Math.Pow(X.Value - other.X.Value, 2) + Math.Pow(Y.Value - other.Y.Value, 2));

        #endregion

    }
}