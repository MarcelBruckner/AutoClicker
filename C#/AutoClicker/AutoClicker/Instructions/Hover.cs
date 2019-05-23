using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace AutoClicker.Instructions
{
    public class Hover : Instruction
    {
        #region Properties
        internal static readonly int MAX_UNCERTAINTY = 20;
        private MovementType? _movement;
        public MovementType Movement { get => _movement ?? MainWindow.GlobalMovementType; set => _movement = value; }

        public IntTuple _x;
        public IntTuple X { get => new IntTuple(_x.Value, _x.Random ?? MainWindow.GlobalRandomX); set => _x = value; }
        public IntTuple _y;
        public IntTuple Y { get => new IntTuple(_y.Value, _y.Random ?? MainWindow.GlobalRandomY); set => _y = value; }

        internal System.Windows.Vector Randomized { get; set; }
        #endregion

        #region Constructors
        public Hover(int x, int y, MovementType? movement = null,
            IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : this(new IntTuple(x), new IntTuple(y), movement,
                delay, repetitions, speed, shift, ctrl, alt)
        { }

        public Hover(IntTuple x, IntTuple y, MovementType? movement = null,
            IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : base(delay, repetitions, speed, shift, ctrl, alt)
        {
            X = x;
            Y = y;

            _movement = movement;
        }

        public Hover(IntTuple x, IntTuple y, MovementType? movement = null,
            Instruction instruction = null
            ) : this(x, y, movement, instruction._delay, instruction._repetitions, instruction._speed, instruction.Shift, instruction.Ctrl, instruction.Alt)
        { }

        #endregion

        internal override void SpecificExecute()
        {
            MoveTo(X, Y);
        }

        internal void MoveTo(IntTuple x, IntTuple y)
        {
            if (!AlreadyThere)
            {
                Randomized = new Vector(Randomize(x), Randomize(y));
                InputSimulator.MoveMouse(Movement, Randomized, Randomize(Speed));
            }
        }

        internal override string GetName()
        {
            return "hover";
        }

        internal override void AppendSpecifics(StringBuilder builder)
        {
            Append(builder, "x", _x);
            Append(builder, "y", _y);
            Append(builder, "movement", _movement);
        }

        public override bool Equals(object obj)
        {
            return obj is Hover hover &&
                   base.Equals(obj) &&
                   EqualityComparer<MovementType?>.Default.Equals(_movement, hover._movement) &&
                   EqualityComparer<IntTuple>.Default.Equals(_x, hover._x) &&
                   EqualityComparer<IntTuple>.Default.Equals(_y, hover._y);
        }

        public override int GetHashCode()
        {
            var hashCode = 295077388;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<MovementType?>.Default.GetHashCode(_movement);
            hashCode = hashCode * -1521134295 + EqualityComparer<IntTuple>.Default.GetHashCode(_x);
            hashCode = hashCode * -1521134295 + EqualityComparer<IntTuple>.Default.GetHashCode(_y);
            return hashCode;
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
    }
}
