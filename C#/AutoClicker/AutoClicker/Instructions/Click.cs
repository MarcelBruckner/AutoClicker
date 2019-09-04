using Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace AutoClicker.Instructions
{
    /// <summary>
    /// Instruction to click the mouse
    /// </summary>
    /// <seealso cref="AutoClicker.Instructions.Hover" />
    public class Click : Hover
    {
        /// <summary>
        /// The button that gets clicked
        /// </summary>
        /// <seealso cref="ButtonType"/>
        private ButtonType? _button;
        public ButtonType Button { get => _button ?? MainWindow.GlobalButtonType; set => _button = value; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Click"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="button">The button.</param>
        /// <param name="movement">The movement.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="repetitions">The repetitions.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="shift">if set to <c>true</c> [shift].</param>
        /// <param name="ctrl">if set to <c>true</c> [control].</param>
        /// <param name="alt">if set to <c>true</c> [alt].</param>
        public Click(int x=0, int y=0, ButtonType? button = null, MovementType? movement = null,
            IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : this(new IntTuple(x), new IntTuple(y), button, movement,
                delay, repetitions, speed, shift, ctrl, alt)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Click"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="button">The button.</param>
        /// <param name="movement">The movement.</param>
        /// <param name="instruction">The instruction.</param>
        public Click(IntTuple x, IntTuple y, ButtonType? button, MovementType? movement,
            Instruction instruction 
            ) : this(x, y, button, movement, instruction.Delay, instruction.Repetitions, instruction.Speed, instruction.Shift, instruction.Ctrl, instruction.Alt)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Click"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="button">The button.</param>
        /// <param name="movement">The movement.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="repetitions">The repetitions.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="shift">if set to <c>true</c> [shift].</param>
        /// <param name="ctrl">if set to <c>true</c> [control].</param>
        /// <param name="alt">if set to <c>true</c> [alt].</param>
        public Click(IntTuple x, IntTuple y, ButtonType? button = null, MovementType? movement = null,
            IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : base(x, y, movement, delay, repetitions, speed, shift, ctrl, alt)
        {
            _button = button;
        }

        #endregion

        /// <summary>
        /// Specific execute for mouse instructions.
        /// </summary>
        internal override void MouseSpecificExecute()
        {
            InputSimulator.MouseClick(Movement ?? MainWindow.GlobalMovementType, Button, RandomizedPosition, Randomize(Speed), Hotkeys);
        }

        /// <summary>
        /// Specifies the name of the instruction
        /// </summary>
        /// <returns>
        /// The name of the instruction
        /// </returns>
        internal override string GetName()
        {
            return "click";
        }

        /// <summary>
        /// Appends the key value pairs of specific properties of the instruction
        /// </summary>
        /// <param name="builder"></param>
        internal override void AppendSpecifics(StringBuilder builder)
        {
            base.AppendSpecifics(builder);
            Append(builder, "button", _button);
        }

        /// <summary>
        /// Determines whether the other specified click has the same button and position.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>
        ///   <c>true</c> if the other specified click has the same button and position; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSameClick(Click other) => Button == other.Button && Distance(other) < MAX_UNCERTAINTY;

        /// <summary>
        /// Calculates the euclidean distance to the specified other click.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public int Distance(Click other) => (int)Math.Sqrt(Math.Pow(X.Value - other.X.Value, 2) + Math.Pow(Y.Value - other.Y.Value, 2));

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is Click click &&
                   base.Equals(obj) &&
                   IsSameClick(click);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = 112211556;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ButtonType?>.Default.GetHashCode(_button);
            return hashCode;
        }        
    }
}