using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.Instructions
{
    class Keyboard : Instruction
    {
        public string Text { get; set; }

        public Keyboard() : this("", 0, 1) { }

        public Keyboard(string text, int delay, int repetitions) : base(Action.KEYBOARD,  delay, repetitions)
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
