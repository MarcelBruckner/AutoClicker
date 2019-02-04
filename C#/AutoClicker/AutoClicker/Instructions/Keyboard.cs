using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace AutoClicker.Instructions
{
    class Keyboard : Instruction
    {
        public string Text { get; set; }

        protected static readonly InputSimulator simulator = new InputSimulator();

        public Keyboard() : this("", 0, 0, 1) { }

        public Keyboard(string text, int[] standards) : this(text, standards[0], standards[1], standards[2]) { }

        public Keyboard(string text, int delayPrevious, int delayAfter, int repetitions) : base(InstructionType.KEYBOARD, delayPrevious, delayAfter, repetitions)
        {            
            Text = text;
        }

        public Keyboard(Dictionary<string, string> raw) : base(raw) { }

        protected override void SpecificExecute()
        {
            foreach(char c in Text.ToUpper())
            {
                simulator.Keyboard.KeyPress((VirtualKeyCode)c);
            }
        }

        protected override void Parse(Dictionary<string, string> raw)
        {
            base.Parse(raw);

            bool hasText = raw.ContainsKey(InstructionProperty.TEXT.ToString());
            if (!hasText)
            {
                throw new MissingPropertyException(InstructionProperty.TEXT.ToString(), "x");
            }
            Text = raw[InstructionProperty.TEXT.ToString()];
        }

        public override string ToString()
        {
            return base.ToString() + " " +
                InstructionProperty.TEXT.ToString() + "=" + Text;
        }

        public override bool Equals(object obj)
        {
            var keyboard = obj as Keyboard;
            return keyboard != null &&
                   base.Equals(obj) &&
                   Text == keyboard.Text;
        }

        public override int GetHashCode()
        {
            var hashCode = 1665531892;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Text);
            return hashCode;
        }
    }
}
