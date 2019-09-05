using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Helpers
{
    class Program
    {

        public static void Main(string[] args)
        {
            Dictionary<string, KeyValuePair<string[], string[]>> instructions = new Dictionary<string, KeyValuePair<string[], string[]>>();

            instructions.Add("commons", new KeyValuePair<string[], string[]>(new string[] { "delay", "repetitions", "speed", "shift", "ctrl", "alt" }, new string[0]));
            instructions.Add("hoverValues", new KeyValuePair<string[], string[]>(new string[] { "xPos", "yPos", "movement" }, new string[] { "commons" }));
            instructions.Add("clickValues", new KeyValuePair<string[], string[]>(new string[] { "button" }, new string[] { "hoverValues", "commons" }));
            instructions.Add("dragValues", new KeyValuePair<string[], string[]>(new string[] { "endX", "endY" }, new string[] { "clickValues", "hoverValues", "commons" }));
            instructions.Add("wheelValues", new KeyValuePair<string[], string[]>(new string[] { "scroll" }, new string[] { "hoverValues", "commons" }));
            instructions.Add("keyValues", new KeyValuePair<string[], string[]>(new string[] { "key" }, new string[] {"commons" }));
            instructions.Add("textValues", new KeyValuePair<string[], string[]>(new string[] { "input" }, new string[] {"commons" }));

            foreach(var instruction in instructions)
            {
                List<string[]> values = new List<string[]>
                {
                    instruction.Value.Key
                };

                foreach (string name in instruction.Value.Value)
                {
                    values.Add(instructions[name].Key);
                }

                GenerateAntlrPermutations(instruction.Key, values.ToArray());
            }
        }

        static string Flatten(string name, List<string> permutations)
        {
            string result = name + ":\t\t";
            foreach(string permutation in permutations)
            {
                result += " (";
                result += " " + permutation + " ";

                //foreach(string variable in permutation)
                //{
                //    result += " " + variable + " |";
                //}
                result = result.Remove(result.Length - 1);
                result += ") |" ;
            }
            result = result.Remove(result.Length - 1) ;
            return result + "\n\n";
        }


        static void GenerateAntlrPermutations(string name, params string[][] allValues)
        {
            List<string> results = GenerateAntlrPermutations(allValues);
            Console.WriteLine(Flatten(name, results));
        }


        static List<string> GenerateAntlrPermutations(params string[][] allValues)
        {
            List<string> values = new List<string>();

            foreach(string[] allValue in allValues)
            {
                values.AddRange(allValue);
            }

            List<string> results = new List<string>();
            GenerateAntlrPermutations(ref results, "", values.ToArray());
            return results;
        }

        static void GenerateAntlrPermutations(ref List<string> results, string current, params string[] values)
        {
            string newCurrent = current;

            if(values.Length == 0)
            {
                newCurrent = newCurrent.Remove(newCurrent.Length - " | ".Length);
                results.Add(newCurrent);
                return;
            }

            string added;
            for (int j = 0; j < values.Length; j++)
            {
                added = values[j] + " | ";
                newCurrent += added;

                List<string> newValues = new List<string>(values);
                newValues.RemoveAt(j);

                GenerateAntlrPermutations(ref results, newCurrent, newValues.ToArray());

                newCurrent = newCurrent.Remove(newCurrent.Length - added.Length);
            }
        }
    }
}
