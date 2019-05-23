using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.Instructions
{
    /** An instruction that executes a keystroke. */
    public class Keystroke : Instruction
    {
        /** The key code of the key that will be executed */
        public VirtualKeyCode Key { get; set; }

        /** constructor */
        public Keystroke(VirtualKeyCode key, IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false
            ) : base(delay, repetitions, speed, shift, ctrl, alt)
        {
            Key = key;
        }
        
        /** constructor */
        public Keystroke(VirtualKeyCode key, Instruction instruction = null
            ) : this(key, instruction._delay, instruction._repetitions, instruction._speed, instruction.Shift, instruction.Ctrl, instruction.Alt)
        { }

        /** equals */
        public override bool Equals(object obj)
        {
            return obj is Keystroke keystroke &&
                   base.Equals(obj) &&
                   Key == keystroke.Key;
        }

        /** hash code */
        public override int GetHashCode()
        {
            var hashCode = -229860446;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Key.GetHashCode();
            return hashCode;
        }

        internal override void SpecificExecute()
        {
            InputSimulator.KeyPress(Key, Hotkeys);
        }

        internal override string GetName()
        {
            return "keystroke";
        }

        internal override void AppendSpecifics(StringBuilder builder)
        {
            Append(builder, "key", Key);
        }
    }
}
