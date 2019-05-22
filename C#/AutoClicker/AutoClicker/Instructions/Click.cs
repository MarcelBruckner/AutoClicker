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
        #region Raw Properties
        private int _x = 0;
        private int _y = 0;
        private MovementType _movement = MainWindow.GlobalMovement;
        private int _randomX = MainWindow.GlobalRandomX;
        private int _randomY = MainWindow.GlobalRandomY;
        private ButtonType _button = ButtonType.LEFT;
        #endregion

        #region Properties
        public ButtonType Button { get => _button; set { _button = value; OnPropertyChanged("Button"); } }
        public MovementType Movement { get => _movement; set { _movement = value; OnPropertyChanged("Movement"); } }

        public int X { get => _x; set { _x = value; OnPropertyChanged("X"); } }
        public int RandomX { get => _randomX; set { _randomX = value; OnPropertyChanged("RandomX"); } }
        public int Y { get => _y; set { _y = value; OnPropertyChanged("Y"); } }
        public int RandomY { get => _randomY; set { _randomY = value; OnPropertyChanged("RandomY"); } }

        private Vector Randomized { get; set; } = new Vector(-100, -100);
        #endregion

        #region Constructors
        public Click(ButtonType button, int x, int y, bool shift, bool ctrl, bool alt) : base(shift, ctrl, alt) {

            SetMouse(button, x, y);
        }

        protected Click(ButtonType button, int x, int y, long delay, long randomDelay, int repetitions, int randomRepetitions, double speed, double randomSpeed, bool shift, bool ctrl, bool alt) : base(
            delay, randomDelay, repetitions, randomRepetitions, speed, randomSpeed, shift, ctrl, alt)
        {
            SetMouse(button, x, y);
        }
        #endregion

        internal override void SpecificExecute()
        {
            if (!AlreadyThere)
            {
                Randomized = new Vector(Randomize(X, RandomX), Randomize(Y, RandomY));
            }
            InputSimulator.MouseClick(Button, GetHotkeys());
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
            builder.Append("click:");
            Append(builder, "x", X);
            Append(builder, "y", Y);
            Append(builder, "button", Button);
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

        private void SetMouse(ButtonType button, int x, int y)
        {
            Button = button;
            X = x;
            Y = y;
        }
        #endregion

    }
}