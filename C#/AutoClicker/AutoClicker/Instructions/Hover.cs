using Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace AutoClicker.Instructions
{
    /// <summary>
    /// Instruction to hover the mouse
    /// </summary>
    /// <seealso cref="AutoClicker.Instructions.Instruction" />
    public class Hover : Instruction
    {
        #region Properties
        /// <summary>
        /// The maximal uncertainty in pixels that distinguishs two mouse positions
        /// </summary>
        internal static readonly int MAX_UNCERTAINTY = 20;

        /// <summary>
        /// The movement type of the mouse
        /// </summary>
        public MovementType? Movement { get; set; }

        /// <summary>
        /// The on screen X position of the desired mouse position
        /// </summary>
        public IntTuple X { get; set; }

        /// <summary>
        /// The on screen Y position of the desired mouse position
        /// </summary>
        public IntTuple Y { get; set; }

        /// <summary>
        /// The final on screen position after randomization
        /// </summary>
        internal System.Windows.Vector RandomizedPosition { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Hover"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="movement">The movement.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="repetitions">The repetitions.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="shift">if set to <c>true</c> [shift].</param>
        /// <param name="ctrl">if set to <c>true</c> [control].</param>
        /// <param name="alt">if set to <c>true</c> [alt].</param>
        public Hover(int x = 0, int y = 0, MovementType? movement = null,
            IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : this(new IntTuple(x), new IntTuple(y), movement,
                delay, repetitions, speed, shift, ctrl, alt)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hover"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="movement">The movement.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="repetitions">The repetitions.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="shift">if set to <c>true</c> [shift].</param>
        /// <param name="ctrl">if set to <c>true</c> [control].</param>
        /// <param name="alt">if set to <c>true</c> [alt].</param>
        public Hover(IntTuple x, IntTuple y, MovementType? movement = null,
            IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : base(delay, repetitions, speed, shift, ctrl, alt)
        {
            X = x;
            Y = y;

            Movement = movement;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hover"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="movement">The movement.</param>
        /// <param name="instruction">The instruction.</param>
        public Hover(IntTuple x, IntTuple y, MovementType? movement = null,
            Instruction instruction = null
            ) : this(x, y, movement, instruction.Delay, instruction.Repetitions, instruction.Speed, instruction.Shift, instruction.Ctrl, instruction.Alt)
        { }

        #endregion

        /// <summary>
        /// Instruction specific execution. Calls the mouse specific execution.
        /// </summary>
        internal override void SpecificExecute()
        {
            if (!AlreadyThere)
            {
                RandomizePosition(X, Y);
                MouseSpecificExecute();
            }
        }

        /// <summary>
        /// Specific execute for mouse instructions.
        /// </summary>
        internal virtual void MouseSpecificExecute()
        {
            InputSimulator.MouseMove(Movement ?? MainWindow.GlobalMovementType, RandomizedPosition, Randomize(Speed));
        }

        /// <summary>
        /// Randomizes the position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        internal void RandomizePosition(IntTuple x, IntTuple y)
        {
            RandomizedPosition = new Vector(Randomize(x), Randomize(y));
        }

        /// <summary>
        /// Specifies the name of the instruction
        /// </summary>
        /// <returns>
        /// The name of the instruction
        /// </returns>
        internal override string GetName()
        {
            return "hover";
        }

        /// <summary>
        /// Appends the key value pairs of specific properties of the instruction
        /// </summary>
        /// <param name="builder"></param>
        internal override void AppendSpecifics(StringBuilder builder)
        {
            Append(builder, "x", X);
            Append(builder, "y", Y);
            Append(builder, "movement", Movement);
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
            return obj is Hover hover &&
                   base.Equals(obj) &&
                   EqualityComparer<MovementType?>.Default.Equals(Movement, hover.Movement) &&
                   EqualityComparer<IntTuple>.Default.Equals(X, hover.X) &&
                   EqualityComparer<IntTuple>.Default.Equals(Y, hover.Y);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = 295077388;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<MovementType?>.Default.GetHashCode(Movement);
            hashCode = hashCode * -1521134295 + EqualityComparer<IntTuple>.Default.GetHashCode(X);
            hashCode = hashCode * -1521134295 + EqualityComparer<IntTuple>.Default.GetHashCode(Y);
            return hashCode;
        }

        /// <summary>
        /// Gets a value indicating whether the mouse cursor is already at the desired position.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the mouse cursor is already at the desired position; otherwise, <c>false</c>.
        /// </value>
        private bool AlreadyThere
        {
            get
            {
                int dx = (int)Math.Abs(RandomizedPosition.X - Cursor.Vector.X);
                int dy = (int)Math.Abs(RandomizedPosition.Y - Cursor.Vector.Y);

                double next = random.NextDouble();
                Console.WriteLine(next);
                return next > 0.025 && dx < (X.Random ?? MainWindow.GlobalRandomX) && dy < (Y.Random ?? MainWindow.GlobalRandomY);
            }
        }
    }
}
