using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AutoClicker.Instructions
{
    class Keyboard : Instruction
    {
        //protected static readonly InputSimulator simulator = new InputSimulator();

        public Keyboard() : this(VirtualKeyCode.RETURN, 0, 1, false, false, false) { }

        public Keyboard(VirtualKeyCode key, int delay, int repetitions, bool shift, bool ctrl, bool alt) : base(Action.KEYBOARD, delay, repetitions, shift, ctrl, alt)
        {
            Key = key;
        }



        public override string ToString()
        {
            string s = base.ToString() + " " +
                Property.KEY + "=" + Key + " ";

            if (Shift)
            {
                s += Property.SHIFT + "=" + Shift + " ";
            }
            if (Ctrl)
            {
                s += Property.CONTROL + "=" + Ctrl + " ";
            }
            if (Alt)
            {
                s += Property.ALT + "=" + Alt + " ";
            }

            return s;
        }

        public override bool Equals(object obj)
        {
            var keyboard = obj as Keyboard;
            return keyboard != null &&
                   base.Equals(obj) &&
                   Key == keyboard.Key &&
                   Shift == keyboard.Shift &&
                   Ctrl == keyboard.Ctrl &&
                   Alt == keyboard.Alt;
        }

        public override int GetHashCode()
        {
            var hashCode = -1323275767;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Key.GetHashCode();
            hashCode = hashCode * -1521134295 + Shift.GetHashCode();
            hashCode = hashCode * -1521134295 + Ctrl.GetHashCode();
            hashCode = hashCode * -1521134295 + Alt.GetHashCode();
            return hashCode;
        }
    }
}