using System.Collections.Generic;
using System.Text.RegularExpressions;
using WindowsInput.Native;

namespace AutoClicker.Instructions
{
    class SpecialKeyboard : Keyboard
    {
        public VirtualKeyCode Key { get; private set; }


        public bool Shift { get; set; }
        public bool Ctrl { get; set; }
        public bool Alt { get; set; }

        public SpecialKeyboard() : this("enter", false, false, false, 0,0,1) { }

        public SpecialKeyboard(string text, bool shift, bool ctrl, bool alt, int[] standards) : this(text, shift, ctrl, alt, standards[0], standards[1], standards[2]) { }

        public SpecialKeyboard(string text, bool shift, bool ctrl, bool alt, int delayPrevious, int delayAfter, int repetitions) : base(text, delayPrevious, delayAfter, repetitions)
        {
            Shift = shift;
            Ctrl = ctrl;
            Alt = alt;
            Type = InstructionType.SPECIAL_KEYBOARD;
            Key = GetKeyCode(text);
        }

        public SpecialKeyboard(Dictionary<string, string> raw) : base(raw) { }

        protected override void SpecificExecute()
        {
            simulator.Keyboard.ModifiedKeyStroke(GetHotkeys(), Key);
        }

        private VirtualKeyCode GetKeyCode(string raw)
        {
            if (Regex.IsMatch(raw, StringManager.RETURN, RegexOptions.IgnoreCase))
            {
                return VirtualKeyCode.RETURN;
            }

            if (Regex.IsMatch(raw, StringManager.TAB, RegexOptions.IgnoreCase))
            {
                return VirtualKeyCode.TAB;
            }

            if (Regex.IsMatch(raw, StringManager.BACK, RegexOptions.IgnoreCase))
            {
                return VirtualKeyCode.BACK;
            }

            if (Regex.IsMatch(raw, StringManager.SPACE, RegexOptions.IgnoreCase))
            {
                return VirtualKeyCode.SPACE;
            }

            if (raw.Trim().StartsWith(StringManager.NUMPAD, System.StringComparison.OrdinalIgnoreCase))
            {
                string removed = raw.Trim().ToLower().Replace(StringManager.NUMPAD.ToLower(), "");
                int numpad = 96 + int.Parse(removed);
                return (VirtualKeyCode)numpad;
            }

            if (raw.Trim().StartsWith(StringManager.F_KEYS, System.StringComparison.OrdinalIgnoreCase))
            {
                string removed = raw.Trim().ToLower().Replace(StringManager.F_KEYS.ToLower(), "");
                int numpad = 111 + int.Parse(removed);
                return (VirtualKeyCode)numpad;
            }

            if (raw.Trim().StartsWith(StringManager.NUMBERS, System.StringComparison.OrdinalIgnoreCase))
            {
                string removed = raw.Trim().ToLower().Replace(StringManager.NUMBERS.ToLower(), "");
                int numpad = 48 + int.Parse(removed);
                return (VirtualKeyCode)numpad;
            }

            return (VirtualKeyCode) raw[0];
        }

        protected override void Parse(Dictionary<string, string> raw)
        {
            base.Parse(raw);

            Key = GetKeyCode(raw[InstructionProperty.TEXT.ToString()]);

            Shift = HasShift(raw);
            Alt = HasAlt(raw);
            Ctrl = HasCtrl(raw);
        }

        private bool HasShift(Dictionary<string, string> raw)
        {
            if (raw.ContainsKey(InstructionProperty.SHIFT.ToString()))
            {
                return bool.Parse(raw[InstructionProperty.SHIFT.ToString()]);
            }
            return false;
        }

        private bool HasAlt(Dictionary<string, string> raw)
        {
            if (raw.ContainsKey(InstructionProperty.ALT.ToString()))
            {
                return bool.Parse(raw[InstructionProperty.ALT.ToString()]);
            }
            return false;
        }

        private bool HasCtrl(Dictionary<string, string> raw)
        {
            if (raw.ContainsKey(InstructionProperty.CONTROL.ToString()))
            {
                return bool.Parse(raw[InstructionProperty.CONTROL.ToString()]);
            }
            return false;
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
                InstructionProperty.SHIFT + "=" + Shift + " " +
                InstructionProperty.CONTROL + "=" + Ctrl + " " +
                InstructionProperty.ALT + "=" + Alt;
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