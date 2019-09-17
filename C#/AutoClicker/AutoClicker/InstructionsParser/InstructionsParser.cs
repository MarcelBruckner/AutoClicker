using Antlr4.Runtime;
using AutoClicker.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.InstructionsParser
{
    /// <summary>
    /// Parser for the instructions input field
    /// </summary>
    public class InstructionsParser
    {
        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static List<Instruction> Parse(string input, GlobalData globalData = null)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);

            AutoClickerLexer lexer = new AutoClickerLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            AutoClickerParser parser = new AutoClickerParser(commonTokenStream);

            AutoClickerParser.InstructionsContext context = parser.instructions();
            AutoClickerInstructionsVisitor visitor = new AutoClickerInstructionsVisitor();

            List<Instruction> instructions = visitor.Visit(context);

            if(globalData != null)
            {
                foreach(var instruction in instructions)
                {
                    instruction.GlobalData = globalData;
                }
            }

            return instructions;
        }
    }
}
