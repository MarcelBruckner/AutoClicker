using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.Instructions
{
    /// <summary>
    /// An instruction that executes a keystroke
    /// </summary>
    /// <seealso cref="Instruction" />
    public class Keystroke : Instruction
    {
        /// <summary>
        /// The key code of the key that will be executed
        /// </summary>
        public VirtualKeyCode Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Keystroke"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="repetitions">The repetitions.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="shift">if set to <c>true</c> [shift].</param>
        /// <param name="ctrl">if set to <c>true</c> [control].</param>
        /// <param name="alt">if set to <c>true</c> [alt].</param>
        public Keystroke(VirtualKeyCode key=VirtualKeyCode.SPACE, DecimalTuple delay = null, DecimalTuple repetitions = null, DecimalTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : base(delay, repetitions, speed, shift, ctrl, alt)
        {
            Key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Keystroke"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="instruction">The instruction.</param>
        public Keystroke(VirtualKeyCode key, Instruction instruction = null
            ) : this(key, instruction.Delay(), instruction.Repetitions, instruction.Speed(), instruction.Shift, instruction.Ctrl, instruction.Alt)
        { }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is Keystroke keystroke &&
                   base.Equals(obj) &&
                   Key == keystroke.Key;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, resembles this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public override bool Resembles(object obj)
        {
            return obj is Keystroke keystroke &&
                     Key == keystroke.Key;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = -229860446;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Key.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Instruction specific execution.
        /// </summary>
        internal override void SpecificExecute()
        {
            InputSimulator.KeyPress(Key, Hotkeys);
        }

        /// <summary>
        /// Specifies the name of the instruction
        /// </summary>
        /// <returns>
        /// The name of the instruction
        /// </returns>
        internal override string GetName()
        {
            return "key";
        }

        /// <summary>
        /// Appends the key value pairs of specific properties of the instruction
        /// </summary>
        /// <param name="builder"></param>
        internal override void AppendSpecifics(StringBuilder builder)
        {
            Append(builder, "input", "\"" + Key + "\"");
        }
    }
}
