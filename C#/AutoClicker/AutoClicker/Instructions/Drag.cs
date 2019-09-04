using Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoClicker.Instructions
{
    public class Drag : Click
    {
        private IntTuple _endX;
        public IntTuple EndX { get => new IntTuple(_endX.Value, _endX.Random ?? MainWindow.GlobalRandomDragX); set => _endX = value; }
        private IntTuple _endY;
        public IntTuple EndY { get => new IntTuple(_endY.Value, _endY.Random ?? MainWindow.GlobalRandomDragY); set => _endY = value; }

        #region Constructors
        public Drag(int x, int y, int endX, int endY, ButtonType? button = null, MovementType? movement = null,
            IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : this(new IntTuple(x), new IntTuple(y), new IntTuple(endX), new IntTuple(endY), button, movement,
                delay, repetitions, speed, shift, ctrl, alt)
        { }

        public Drag(IntTuple x, IntTuple y, IntTuple endX, IntTuple endY, ButtonType? button = null, MovementType? movement = null,
            Instruction instruction = null
            ) : this(x, y, endX, endY, button, movement, instruction.Delay, instruction.Repetitions, instruction.Speed, instruction.Shift, instruction.Ctrl, instruction.Alt)
        {
        }

        public Drag(IntTuple x, IntTuple y, IntTuple endX, IntTuple endY, ButtonType? button = null, MovementType? movement = null,
            IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : base(x, y, button, movement, delay, repetitions, speed, shift, ctrl, alt)
        {
            EndX = endX;
            EndY = endY;
        }
        #endregion

        internal override void SpecificExecute()
        {
            InputSimulator.MouseDrag(Movement, Button,
                (int)Randomize(X), (int)Randomize(Y), (int)Randomize(EndX), (int)Randomize(EndY), Randomize(Speed), Hotkeys);
        }        

        internal override string GetName()
        {
            return "drag";
        }

        internal override void AppendSpecifics(StringBuilder builder)
        {
            base.AppendSpecifics(builder);
            Append(builder, "endX", _endX);
            Append(builder, "endY", _endY);
        }

        public override bool Equals(object obj)
        {
            return obj is Drag drag &&
                   base.Equals(obj) &&
                   EqualityComparer<IntTuple>.Default.Equals(_endX, drag._endX) &&
                   EqualityComparer<IntTuple>.Default.Equals(_endY, drag._endY);
        }

        public override int GetHashCode()
        {
            var hashCode = 1210200173;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<IntTuple>.Default.GetHashCode(_endX);
            hashCode = hashCode * -1521134295 + EqualityComparer<IntTuple>.Default.GetHashCode(_endY);
            return hashCode;
        }
    }
}