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
    /// <seealso cref="Instruction" />
    public class Hover : Instruction
    {
        #region Properties
        /// <summary>
        /// The maximal uncertainty in pixels that distinguishs two mouse positions
        /// </summary>
        internal static readonly int MAX_UNCERTAINTY = 20;
        public MovementType Movement { get; }

        /// <summary>
        /// The on screen X position of the desired mouse position
        /// </summary>
        public DecimalTuple X { get; set; }

        /// <summary>
        /// The on screen Y position of the desired mouse position
        /// </summary>
        public DecimalTuple Y { get; set; }

        /// <summary>
        /// The final on screen position after randomization
        /// </summary>
        internal Vector RandomizedPosition { get; set; }
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
        public Hover(int x, int y, MovementType movement = MovementType.GLOBAL,
            DecimalTuple delay = null, DecimalTuple repetitions = null, DecimalTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false, GlobalData globalData = null
            ) : this(new DecimalTuple(x), new DecimalTuple(y), movement,
                delay, repetitions, speed, shift, ctrl, alt, globalData)
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
        public Hover(DecimalTuple x = null, DecimalTuple y = null, MovementType movement = MovementType.GLOBAL,
            DecimalTuple delay = null, DecimalTuple repetitions = null, DecimalTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false, GlobalData globalData = null
            ) : base(delay, repetitions, speed, shift, ctrl, alt, globalData)
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
        public Hover(Instruction instruction, DecimalTuple x = null, DecimalTuple y = null, MovementType movement = MovementType.GLOBAL, GlobalData globalData = null) 
            : this(x, y, movement, instruction.Delay(), instruction.Repetitions, instruction.Speed(), instruction.Shift, instruction.Ctrl, instruction.Alt, globalData)
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
            }

            MouseSpecificExecute();
        }

        /// <summary>
        /// Specific execute for mouse instructions.
        /// </summary>
        internal virtual void MouseSpecificExecute()
        {
            InputSimulator.MouseMove(Movement, RandomizedPosition, Speed(true).Get(GlobalData.RandomSpeed));
        }

        /// <summary>
        /// Randomizes the position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        internal void RandomizePosition(DecimalTuple x, DecimalTuple y)
        {
            if (x == null || y == null)
            {
                RandomizedPosition = Cursor.Vector;
            }
            else
            {
                RandomizedPosition = new Vector(x.Get(0), y.Get(0));
            }
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
            if (X != null)
            {
                Append(builder, "x", X);
            }
            if (Y != null)
            {
                Append(builder, "y", Y);
            }
            if (Movement != MovementType.GLOBAL)
            {
                Append(builder, "movement", Movement);
            }
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
                   EqualityComparer<DecimalTuple>.Default.Equals(X, hover.X) &&
                   EqualityComparer<DecimalTuple>.Default.Equals(Y, hover.Y);
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
            hashCode = hashCode * -1521134295 + EqualityComparer<DecimalTuple>.Default.GetHashCode(X);
            hashCode = hashCode * -1521134295 + EqualityComparer<DecimalTuple>.Default.GetHashCode(Y);
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
                if (X == null || Y == null)
                {
                    return false;
                }
                int dx = (int)Math.Abs(RandomizedPosition.X - Cursor.Vector.X);
                int dy = (int)Math.Abs(RandomizedPosition.Y - Cursor.Vector.Y);

                double next = random.NextDouble();
                Console.WriteLine(next);
                return next > 0.025 && dx < (X.Random ?? GlobalData.RandomX) && dy < (Y.Random ?? GlobalData.RandomY);
            }
        }


        /// <summary>
        /// Determines whether the mouse positions are roughly the same.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>
        ///   <c>true</c> if the mouse positions are roughly the same; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSamePosition(Hover other)
        {
            return Distance(other) < MAX_UNCERTAINTY;
        }

        /// <summary>
        /// Calculates the euclidean distance to the specified other click.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public int Distance(Hover other)
        {
            if (X == null || Y == null ||
                other.X == null || other.Y == null)
            {
                return -1;
            }
            return (int)Math.Sqrt(Math.Pow(X.Value - other.X.Value, 2) + Math.Pow(Y.Value - other.Y.Value, 2));
        }
    }
}