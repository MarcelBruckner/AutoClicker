using AutoClicker.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace AutoClicker
{
    public class InstructionsParser
    {
        private MainWindow window;
        public InstructionsParser(MainWindow window)
        {
            this.window = window;
        }

        public Instruction Parse(string instructions, int delay, int repetitions, bool shift, bool ctrl, bool alt)
        {
            Loop mainLoop = new Loop(delay, repetitions, shift, ctrl, alt);

            string[] lines = instructions.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    string line = lines[i];
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    Dictionary<Instruction.Property, object> parsed = Parse(line);

                    Instruction.Action type = (Instruction.Action) parsed[Instruction.Property.TYPE];
                    int pDelay = 0;
                    int pRepetitions = 1;

                    if (parsed.ContainsKey(Instruction.Property.AFTER))
                    {
                        pDelay = (int)parsed[Instruction.Property.AFTER];
                    }
                    if (parsed.ContainsKey(Instruction.Property.REPETITIONS))
                    {
                        pRepetitions = (int)parsed[Instruction.Property.REPETITIONS];
                    }

                    bool hasShift = false;
                    bool hasCtrl = false;
                    bool hasAlt = false;
                    if (parsed.ContainsKey(Instruction.Property.SHIFT))
                    {
                        hasShift = (bool)parsed[Instruction.Property.SHIFT];
                    }
                    if (parsed.ContainsKey(Instruction.Property.CONTROL))
                    {
                        hasCtrl = (bool)parsed[Instruction.Property.CONTROL];
                    }
                    if (parsed.ContainsKey(Instruction.Property.ALT))
                    {
                        hasAlt = (bool)parsed[Instruction.Property.ALT];
                    }


                    if (type.Equals(Instruction.Action.CLICK) || type.Equals(Instruction.Action.DRAG))
                    {
                        int button = 0;
                        if (parsed.ContainsKey(Instruction.Property.BUTTON))
                        {
                            button = (int)parsed[Instruction.Property.BUTTON];
                        }

                        if (type.Equals(Instruction.Action.CLICK))
                        {
                            mainLoop.Add(new MouseClick(button,
                                (int)parsed[Instruction.Property.X],
                                (int)parsed[Instruction.Property.Y],
                                pDelay, pRepetitions,
                                hasShift, hasCtrl, hasAlt
                                ));
                        }
                        else
                        {
                            mainLoop.Add(new MouseDrag(button,
                            (int)parsed[Instruction.Property.X],
                            (int)parsed[Instruction.Property.Y],
                            (int)parsed[Instruction.Property.END_X],
                            (int)parsed[Instruction.Property.END_Y],
                            pDelay, pRepetitions,
                                hasShift, hasCtrl, hasAlt));
                        }
                    }
                    else if (type.Equals(Instruction.Action.SPECIAL_KEYBOARD))
                    {
                        

                        mainLoop.Add(new Keyboard(
                            (VirtualKeyCode)parsed[Instruction.Property.KEY],
                            pDelay, pRepetitions,
                            hasShift, hasCtrl, hasAlt
                            ));
                    }
                    else if (type.Equals(Instruction.Action.KEYBOARD))
                    {
                        mainLoop.Add(new Keyboard((VirtualKeyCode)parsed[Instruction.Property.KEY], pDelay, pRepetitions,
                                hasShift, hasCtrl, hasAlt));
                    }
                    else if (type.Equals(Instruction.Action.DELAY))
                    {
                        mainLoop.Add(new Delay(pDelay, pRepetitions));
                    }
                    else if (type.Equals(Instruction.Action.LOOP))
                    {
                        string loopText = "";
                        while (i++ < lines.Length - 1)
                        {
                            line = lines[i];

                            if (Regex.IsMatch(line, Instruction.Action.END_LOOP + "", RegexOptions.IgnoreCase))
                            {
                                break;
                            }

                            loopText += line + "\n";
                        }
                        mainLoop.Add(Parse(loopText, pDelay, pRepetitions, hasShift, hasCtrl, hasAlt));
                    }
                    //else if (parsed[Instruction.Property.TYPE].Equals(Instruction.Action.END_LOOP)) { }
                    else
                    {
                        throw new ArgumentException("Instruction not found in line: " + i);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error in line: " + i + " --- " + e.Message);
                }
            }

            Console.WriteLine();
            return mainLoop;
        }

        public bool SpellCheck(FlowDocument document)
        {
            bool isCorrect = true;
            int i = 0;
            foreach (Block block in document.Blocks)
            {
                TextRange range = new TextRange(block.ContentStart, block.ContentEnd);
                try
                {
                    string text = range.Text;
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        continue;
                    }
                    Dictionary<Instruction.Property, object> rawInstructions = Parse(text);
                    window.HighlightLine(i, Brushes.Black);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error while parsing: " + e.Message);
                    window.HighlightLine(i, Brushes.Red);
                    isCorrect = false;
                }
                i++;
            }

            //foreach (Block block in document.Blocks)
            //{
            //    TextRange range = new TextRange(block.ContentStart, block.ContentEnd);
            //    try
            //    {
            //        string text = range.Text;
            //        if (string.IsNullOrWhiteSpace(text))
            //        {
            //            continue;
            //        }
            //        Dictionary<Instruction.Property, object> rawInstructions = Parse(text);
            //        range.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
            //    }
            //    catch (ArgumentException e)
            //    {
            //        Console.WriteLine("Error while parsing: " + e.Message);
            //        range.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            //        isCorrect = false;
            //    }
            //}

            return isCorrect;
        }

        private List<string> GetKeyValuePairs(string raw)
        {
            List<string> unParsedKayValuePairs = new List<string>(raw.Trim().Split(null));
            string action = unParsedKayValuePairs[0];
            unParsedKayValuePairs.RemoveAt(0);
            unParsedKayValuePairs.Add(Instruction.Property.TYPE + "=" + (Instruction.Action)Enum.Parse(typeof(Instruction.Action), action));
            return unParsedKayValuePairs;
        }

        private Dictionary<Instruction.Property, object> Parse(string line)
        {
            List<string> unParsedKayValuePairs = GetKeyValuePairs(line);

            Dictionary<Instruction.Property, object> rawInstruction = new Dictionary<Instruction.Property, object>();
            foreach (string keyValuePair in unParsedKayValuePairs)
            {
                if (string.IsNullOrWhiteSpace(keyValuePair))
                {
                    continue;
                }
                string[] split = keyValuePair.Trim().Split(new[] { '=' },2);

                try
                {
                    object value;
                    Instruction.Property property = (Instruction.Property)Enum.Parse(typeof(Instruction.Property), split[0].ToUpper());
                    switch (property)
                    {
                        case Instruction.Property.TYPE:
                            value = (Instruction.Action)Enum.Parse(typeof(Instruction.Action), split[1]);
                            break;
                        case Instruction.Property.AFTER:
                        case Instruction.Property.REPETITIONS:
                        case Instruction.Property.X:
                        case Instruction.Property.Y:
                        case Instruction.Property.BUTTON:
                        case Instruction.Property.END_X:
                        case Instruction.Property.END_Y:
                            value = int.Parse(split[1]);
                            break;
                        case Instruction.Property.CONTROL:
                        case Instruction.Property.SHIFT:
                        case Instruction.Property.ALT:
                            value = bool.Parse(split[1]);
                            break;
                        case Instruction.Property.KEY:

                            value = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), split[1]);
                            break;
                        default:
                            value = split[1];
                            break;
                    }
                    rawInstruction.Add(property, value);
                }
                catch (Exception)
                {
                    throw new ArgumentException("Cannot parse: " + keyValuePair);
                }
            }
            return rawInstruction;
        }

        private int ParseInt(string raw)
        {
            foreach(char c in raw)
            {
                if (!char.IsNumber(c))
                {
                    throw new ArgumentException("Not a number: " + raw);
                }
            }
            return int.Parse(raw);
        }

        //public VirtualKeyCode GetKeyCode(string raw)
        //{
        //    if (Regex.IsMatch(raw, StringManager.RETURN, RegexOptions.IgnoreCase))
        //    {
        //        return VirtualKeyCode.RETURN;
        //    }

        //    if (Regex.IsMatch(raw, StringManager.TAB, RegexOptions.IgnoreCase))
        //    {
        //        return VirtualKeyCode.TAB;
        //    }

        //    if (Regex.IsMatch(raw, StringManager.BACK, RegexOptions.IgnoreCase))
        //    {
        //        return VirtualKeyCode.BACK;
        //    }

        //    if (Regex.IsMatch(raw, StringManager.SPACE, RegexOptions.IgnoreCase))
        //    {
        //        return VirtualKeyCode.SPACE;
        //    }

        //    string trimmed = raw.Trim();
        //    if (raw.Length > 1)
        //    {
        //        if (raw.Trim().StartsWith(StringManager.NUMPAD, StringComparison.OrdinalIgnoreCase))
        //        {
        //            string removed = raw.Trim().ToLower().Replace(StringManager.NUMPAD.ToLower(), "");
        //            int numpad = 96 + int.Parse(removed);
        //            return (VirtualKeyCode)numpad;
        //        }

        //        if (raw.Trim().StartsWith(StringManager.F_KEYS, StringComparison.OrdinalIgnoreCase))
        //        {
        //            string removed = raw.Trim().ToLower().Replace(StringManager.F_KEYS.ToLower(), "");
        //            int numpad = 111 + int.Parse(removed);
        //            return (VirtualKeyCode)numpad;
        //        }

        //        if (raw.Trim().StartsWith(StringManager.NUMBERS, StringComparison.OrdinalIgnoreCase))
        //        {
        //            string removed = raw.Trim().ToLower().Replace(StringManager.NUMBERS.ToLower(), "");
        //            int numpad = 48 + int.Parse(removed);
        //            return (VirtualKeyCode)numpad;
        //        }
        //    }

        //    return (VirtualKeyCode)raw[0];
        //}
    }
}