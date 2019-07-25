using AutoClicker;
using AutoClicker.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Text : AutoClicker.Instructions.Instruction
{
    public string Input { get; set; }

    /** constructor */
    public Text(string input, IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null,
        bool ctrl = false, bool alt = false
        ) : base(delay, repetitions, speed, false, ctrl, alt)
    {
        Input = input;
    }

    /** constructor */
    public Text(string input, AutoClicker.Instructions.Instruction instruction = null
        ) : this(input, instruction._delay, instruction._repetitions, instruction._speed, instruction.Ctrl, instruction.Alt)
    { }

    public override bool Equals(object obj)
    {
        return obj is Text text &&
               base.Equals(obj) &&
               Input == text.Input;
    }

    public override int GetHashCode()
    {
        var hashCode = -993676367;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Input);
        return hashCode;
    }

    internal override void SpecificExecute()
    {

    }

    public List<KeyValuePair<VirtualKeyCode, bool>> ConvertTextToKeys()
    {
        List<KeyValuePair<VirtualKeyCode, bool>> keys = new List<KeyValuePair<VirtualKeyCode, bool>>();

        foreach(char c in Input)
        {
            Console.WriteLine(c);
            bool upper = char.IsUpper(c);
            if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
            {
                keys.Add(new KeyValuePair<VirtualKeyCode, bool>(VirtualKeyCode.NONAME.FromString(c + ""), upper));
            }
        }

        return keys;
    }

    internal override string GetName()
    {
        return "text";
    }

    internal override void AppendSpecifics(StringBuilder builder)
    {
        Append(builder, "input", Input);
    }
}