using Enums;
using AutoClicker.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AutoClicker.Instructions
{
    /// <summary>
    /// Instruction to input text
    /// </summary>
    /// <seealso cref="AutoClicker.Instructions.Instruction" />
    public class Text : Instruction
    {
        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public string Input { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="repetitions">The repetitions.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="ctrl">if set to <c>true</c> [control].</param>
        /// <param name="alt">if set to <c>true</c> [alt].</param>
        public Text(string input="", IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
            bool ctrl = false, bool alt = false
            ) : base(delay, repetitions, speed, false, ctrl, alt)
        {
            Input = input;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="instruction">The instruction.</param>
        public Text(string input, AutoClicker.Instructions.Instruction instruction = null
            ) : this(input, instruction.Delay, instruction.Repetitions, instruction.Speed, instruction.Ctrl, instruction.Alt)
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
            return obj is Text text &&
                   base.Equals(obj) &&
                   Input == text.Input;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, resembles this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public override bool Resembles(object obj)
        {
            return obj is Text;
        }
    
        /// <summary>
        /// Appends the specified other string.
        /// </summary>
        /// <param name="other">The other.</param>
        public void Append(string other)
        {
            Input += other;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = -993676367;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Input);
            return hashCode;
        }

        /// <summary>
        /// Instruction specific execution.
        /// </summary>
        internal override void SpecificExecute()
        {
            InputSimulator.Text(Input, Hotkeys);
        }

        /// <summary>
        /// Converts the text to keys.
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<VirtualKeyCode, bool>> Converted
        {
            get
            {
                List<KeyValuePair<VirtualKeyCode, bool>> keys = new List<KeyValuePair<VirtualKeyCode, bool>>();

                foreach (char c in Input)
                {
                    Console.WriteLine(c);
                    bool upper = char.IsUpper(c);
                    //if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                    {
                        keys.Add(new KeyValuePair<VirtualKeyCode, bool>(VirtualKeyCode.NONAME.FromString(c + ""), upper));
                    }
                }

                return keys;
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
            return "text";
        }

        /// <summary>
        /// Appends the key value pairs of specific properties of the instruction
        /// </summary>
        /// <param name="builder"></param>
        internal override void AppendSpecifics(StringBuilder builder)
        {
            Append(builder, "input", "\"" + Input + "\"");
        }
    }
}