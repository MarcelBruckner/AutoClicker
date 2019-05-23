using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace AutoClicker.Instructions
{
    public class Click : Hover
    {
        private ButtonType? _button;

        public ButtonType Button { get => _button ?? MainWindow.GlobalButtonType; set => _button = value; }

        #region Constructors
        public Click(int x, int y, ButtonType? button = null, MovementType? movement = null,
            IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : this(new IntTuple(x), new IntTuple(y), button, movement,
                delay, repetitions, speed, shift, ctrl, alt)
        { }

        public Click(IntTuple x, IntTuple y, ButtonType? button, MovementType? movement,
            Instruction instruction 
            ) : this(x, y, button, movement, instruction._delay, instruction._repetitions, instruction._speed, instruction.Shift, instruction.Ctrl, instruction.Alt)
        { }

        public Click(IntTuple x, IntTuple y, ButtonType? button = null, MovementType? movement = null,
            IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : base(x, y, movement, delay, repetitions, speed, shift, ctrl, alt)
        {
            _button = button;
        }

        #endregion

        internal override void SpecificExecute()
        {
            base.SpecificExecute();
            InputSimulator.MouseClick(Movement, Button, Randomized, Randomize(Speed), Hotkeys);
        }
        
        internal override string GetName()
        {
            return "click";
        }

        internal override void AppendSpecifics(StringBuilder builder)
        {
            base.AppendSpecifics(builder);
            Append(builder, "button", _button);
        }

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

        public override bool Equals(object obj)
        {
            return obj is Click click &&
                   base.Equals(obj) &&
                   EqualityComparer<ButtonType?>.Default.Equals(_button, click._button);
        }

        public override int GetHashCode()
        {
            var hashCode = 112211556;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ButtonType?>.Default.GetHashCode(_button);
            return hashCode;
        }        
    }
}