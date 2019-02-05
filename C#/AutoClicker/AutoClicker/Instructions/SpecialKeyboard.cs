using System.Collections.Generic;
using System.Text.RegularExpressions;
using WindowsInput;
using WindowsInput.Native;

namespace AutoClicker.Instructions
{
    class SpecialKeyboard : Instruction
    {
        public VirtualKeyCode Key { get; private set; }

        protected static readonly InputSimulator simulator = new InputSimulator();

        public bool Shift { get; set; }
        public bool Ctrl { get; set; }
        public bool Alt { get; set; }

        public SpecialKeyboard() : this(VirtualKeyCode.RETURN, false, false, false, 0,1) { }

        public SpecialKeyboard(VirtualKeyCode key, bool shift, bool ctrl, bool alt,  int delay, int repetitions) : base(Action.SPECIAL_KEYBOARD, delay, repetitions)
        {
            Shift = shift;
            Ctrl = ctrl;
            Alt = alt;
            Type = Action.SPECIAL_KEYBOARD;
            Key = key;
        }

        protected override void SpecificExecute()
        {
            simulator.Keyboard.ModifiedKeyStroke(GetHotkeys(), Key);
        }
     
        protected List<VirtualKeyCode> GetHotkeys()
        {
            List<VirtualKeyCode> hotkeys = new List<VirtualKeyCode>();
            if (Shift)
            {
                hotkeys.Add(VirtualKeyCode.SHIFT);
            }
            if (Alt)
            {
                hotkeys.Add(VirtualKeyCode.MENU);
            }
            if (Ctrl)
            {
                hotkeys.Add(VirtualKeyCode.CONTROL);
            }
            return hotkeys;
        }

        public override string ToString()
        {
            return base.ToString() + " " +
                Property.KEY + "=" + Key + " " +
                Property.SHIFT + "=" + Shift + " " +
                Property.CONTROL + "=" + Ctrl + " " +
                Property.ALT + "=" + Alt;
        }

        public override bool Equals(object obj)
        {
            var keyboard = obj as SpecialKeyboard;
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