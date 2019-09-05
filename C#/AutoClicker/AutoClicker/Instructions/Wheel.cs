using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.Instructions
{
    public class Wheel : Hover
    {
        public DecimalTuple ScrollDistance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wheel"/> class.
        /// </summary>
        /// <param name="scrollDistance">The scroll distance.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="movement">The movement.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="repetitions">The repetitions.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="shift">if set to <c>true</c> [shift].</param>
        /// <param name="ctrl">if set to <c>true</c> [control].</param>
        /// <param name="alt">if set to <c>true</c> [alt].</param>
        public Wheel(int scrollDistance=0, int x = 0, int y = 0, MovementType? movement = null,
            DecimalTuple delay = null, DecimalTuple repetitions = null, DecimalTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : this(new DecimalTuple(scrollDistance), new DecimalTuple(x), new DecimalTuple(y), movement, delay, repetitions, speed, shift, ctrl, alt)
        {
            ScrollDistance = new DecimalTuple(scrollDistance);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wheel"/> class.
        /// </summary>
        /// <param name="scrollDistance">The scroll distance.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="movement">The movement.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="repetitions">The repetitions.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="shift">if set to <c>true</c> [shift].</param>
        /// <param name="ctrl">if set to <c>true</c> [control].</param>
        /// <param name="alt">if set to <c>true</c> [alt].</param>
        public Wheel(DecimalTuple scrollDistance, DecimalTuple x=null, DecimalTuple y=null, MovementType? movement = null,
            DecimalTuple delay = null, DecimalTuple repetitions = null, DecimalTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : base(x, y, movement, delay, repetitions, speed, shift, ctrl, alt)
        {
            ScrollDistance = scrollDistance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wheel"/> class.
        /// </summary>
        /// <param name="scrollDistance">The scroll distance.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="movement">The movement.</param>
        /// <param name="instruction">The instruction.</param>
        public Wheel(DecimalTuple scrollDistance, DecimalTuple x, DecimalTuple y, MovementType? movement = null,
            Instruction instruction = null
            ) : this(scrollDistance, x, y, movement, instruction.Delay(), instruction.Repetitions, instruction.Speed(), instruction.Shift, instruction.Ctrl, instruction.Alt)
        { }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, resembles this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public override bool Resembles(object obj)
        {
            return obj is Wheel wheel &&
                IsSamePosition(wheel);
        }

        /// <summary>
        /// Appends the key value pairs of specific properties of the instruction
        /// </summary>
        /// <param name="builder"></param>
        internal override void AppendSpecifics(StringBuilder builder)
        {
            base.AppendSpecifics(builder);
            Append(builder, "scroll", ScrollDistance);
        }

        /// <summary>
        /// Specifies the name of the instruction
        /// </summary>
        /// <returns>
        /// The name of the instruction
        /// </returns>
        internal override string GetName()
        {
            return "wheel";
        }
        
        /// <summary>
        /// Specific execute for mouse instructions.
        /// </summary>
        internal override void MouseSpecificExecute()
        {
            InputSimulator.MouseWheel(Movement, RandomizedPosition, 
                ScrollDistance.Get(MainWindow.GlobalRandomWheel), Speed(true).Get(MainWindow.GlobalRandomSpeed), Hotkeys);
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
            return obj is Wheel wheel &&
                   base.Equals(obj) &&
                   EqualityComparer<DecimalTuple>.Default.Equals(ScrollDistance, wheel.ScrollDistance);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = -437804947;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<DecimalTuple>.Default.GetHashCode(ScrollDistance);
            return hashCode;
        }
    }
}
