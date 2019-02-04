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
        public List<int> ErrorLines { get; private set; } = new List<int>();

        #region Singleton
        private static InstructionsParser _instance;

        private InstructionsParser() { }

        public static InstructionsParser Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new InstructionsParser();
                }
                return _instance;
            }
        }
        #endregion
        
        public Instruction Parse(string instructions, int delayPrevious, int delayAfter, int repetitions)
        {
            Instruction mainLoop = new Instruction(Instruction.InstructionType.LOOP, repetitions);

            string[] lines = instructions.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    List<string> unParsedKayValuePairs = new List<string>(line.Split(null));
                    Dictionary<string, string> rawInstruction = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                    foreach (string keyValuePair in unParsedKayValuePairs)
                    {
                        if (string.IsNullOrWhiteSpace(keyValuePair))
                        {
                            continue;
                        }
                        string[] split = keyValuePair.Trim().Split('=');
                        rawInstruction.Add(split[0].ToLower(), split[1]);
                    }

                    if (Regex.IsMatch(rawInstruction[Instruction.InstructionProperty.TYPE.ToString()], Instruction.InstructionType.CLICK.ToString(), RegexOptions.IgnoreCase))
                    {
                        mainLoop.Add(new MouseClick(rawInstruction));
                    }
                    else if (Regex.IsMatch(rawInstruction[Instruction.InstructionProperty.TYPE.ToString()], Instruction.InstructionType.SPECIAL_KEYBOARD.ToString(), RegexOptions.IgnoreCase))
                    {
                        mainLoop.Add(new SpecialKeyboard(rawInstruction));
                    }
                    else if (Regex.IsMatch(rawInstruction[Instruction.InstructionProperty.TYPE.ToString()], Instruction.InstructionType.KEYBOARD.ToString(), RegexOptions.IgnoreCase))
                    {
                        mainLoop.Add(new Keyboard(rawInstruction));
                    }
                    else if (Regex.IsMatch(rawInstruction[Instruction.InstructionProperty.TYPE.ToString()], Instruction.InstructionType.LOOP.ToString() , RegexOptions.IgnoreCase))
                    {
                        string loopText = "";
                        while (i++ < lines.Length)
                        {
                            line = lines[i];
                            bool isEndLoop = Regex.IsMatch(line, StringManager.END_LOOP, RegexOptions.IgnoreCase);
                            if (!isEndLoop)
                            {
                                loopText += line + "\n";
                            }
                            else
                            {
                                i++;
                                break;
                            }
                        }
                        mainLoop.Add(Parse(loopText, 
                            int.Parse(rawInstruction[Instruction.InstructionProperty.PREVIOUS.ToString()]),
                            int.Parse(rawInstruction[Instruction.InstructionProperty.AFTER.ToString()]),
                            int.Parse(rawInstruction[Instruction.InstructionProperty.REPETITIONS.ToString()])));
                    }
                    else if (Regex.IsMatch(rawInstruction[Instruction.InstructionProperty.TYPE.ToString()], Instruction.InstructionType.DELAY.ToString(), RegexOptions.IgnoreCase))
                    {
                        mainLoop.Add(new Instruction(rawInstruction));
                    }
                    else if (Regex.IsMatch(rawInstruction[Instruction.InstructionProperty.TYPE.ToString()], Instruction.InstructionType.DRAG.ToString(), RegexOptions.IgnoreCase))
                    {
                        mainLoop.Add(new MouseDrag(rawInstruction));
                    }
                    else
                    {
                        throw new ArgumentException("Instruction not found.");
                    }
                }
                catch (Exception e)
                {
                    if (e is ArgumentException || e is MissingPropertyException)
                    {
                        Console.WriteLine("Error in line: " + i + " --- " + e.Message);
                        ErrorLines.Add(i);
                    }
                    throw;
                }
            }

            Console.WriteLine();
            return mainLoop;
        }
    }
}