using Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoClicker.Instructions
{
    /// <summary>
    /// Drag instruction
    /// </summary>
    /// <seealso cref="AutoClicker.Instructions.Click" />
    public class Drag : Click
    {
        /// <summary>
        /// The end x
        /// </summary>
        private IntTuple _endX;
        public IntTuple EndX { get => new IntTuple(_endX.Value, _endX.Random ?? MainWindow.GlobalRandomDragX); set => _endX = value; }

        /// <summary>
        /// The end y
        /// </summary>
        private IntTuple _endY;
        public IntTuple EndY { get => new IntTuple(_endY.Value, _endY.Random ?? MainWindow.GlobalRandomDragY); set => _endY = value; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Drag"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="endX">The end x.</param>
        /// <param name="endY">The end y.</param>
        /// <param name="button">The button.</param>
        /// <param name="movement">The movement.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="repetitions">The repetitions.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="shift">if set to <c>true</c> [shift].</param>
        /// <param name="ctrl">if set to <c>true</c> [control].</param>
        /// <param name="alt">if set to <c>true</c> [alt].</param>
        public Drag(int x=0, int y=0, int endX=0, int endY=0, ButtonType? button = null, MovementType? movement = null,
            IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : this(new IntTuple(x), new IntTuple(y), new IntTuple(endX), new IntTuple(endY), button, movement,
                delay, repetitions, speed, shift, ctrl, alt)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Drag"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="endX">The end x.</param>
        /// <param name="endY">The end y.</param>
        /// <param name="button">The button.</param>
        /// <param name="movement">The movement.</param>
        /// <param name="instruction">The instruction.</param>
        public Drag(IntTuple x, IntTuple y, IntTuple endX, IntTuple endY, ButtonType? button = null, MovementType? movement = null,
            Instruction instruction = null
            ) : this(x, y, endX, endY, button, movement, instruction.Delay, instruction.Repetitions, instruction.Speed, instruction.Shift, instruction.Ctrl, instruction.Alt)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Drag"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="endX">The end x.</param>
        /// <param name="endY">The end y.</param>
        /// <param name="button">The button.</param>
        /// <param name="movement">The movement.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="repetitions">The repetitions.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="shift">if set to <c>true</c> [shift].</param>
        /// <param name="ctrl">if set to <c>true</c> [control].</param>
        /// <param name="alt">if set to <c>true</c> [alt].</param>
        public Drag(IntTuple x, IntTuple y, IntTuple endX, IntTuple endY, ButtonType? button = null, MovementType? movement = null,
            IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : base(x, y, button, movement, delay, repetitions, speed, shift, ctrl, alt)
        {
            EndX = endX;
            EndY = endY;
        }
        #endregion

        /// <summary>
        /// Instruction specific execution. Calls the specific execution.
        /// </summary>
        internal override void SpecificExecute()
        {
            InputSimulator.MouseDrag(MainWindow.GlobalMovementType, Button,
                (int)Randomize(X), (int)Randomize(Y), (int)Randomize(EndX), (int)Randomize(EndY), Randomize(Speed), Hotkeys);
        }

        /// <summary>
        /// Specifies the name of the instruction
        /// </summary>
        /// <returns>
        /// The name of the instruction
        /// </returns>
        internal override string GetName()
        {
            return "drag";
        }

        /// <summary>
        /// Appends the key value pairs of specific properties of the instruction
        /// </summary>
        /// <param name="builder"></param>
        internal override void AppendSpecifics(StringBuilder builder)
        {
            base.AppendSpecifics(builder);
            Append(builder, "endX", _endX);
            Append(builder, "endY", _endY);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is Drag drag &&
                   base.Equals(obj) &&
                   EqualityComparer<IntTuple>.Default.Equals(_endX, drag._endX) &&
                   EqualityComparer<IntTuple>.Default.Equals(_endY, drag._endY);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
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