using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.Instructions
{
    class OldKeyboard : Instruction
    {
        public string Text { get; set; }

        public OldKeyboard() : this("", 0, 1) { }

        public OldKeyboard(string text, int delay, int repetitions) : base(Action.KEYBOARD,  delay, repetitions, false, false, false)
        {            
            Text = text;
        }

        protected override void SpecificExecute()
        {
            foreach(char c in Text.ToUpper())
            {
                InputSimulator.KeyPress((VirtualKeyCode)c);
                //simulator.Keyboard.KeyPress((VirtualKeyCode)c);
            }
        }
        
        public override string ToString()
        {
            return base.ToString() + " " +
                Property.TEXT.ToString() + "=" + Text;
        }

        public override bool Equals(object obj)
        {
            var keyboard = obj as OldKeyboard;
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
