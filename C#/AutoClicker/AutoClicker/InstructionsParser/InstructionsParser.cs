using Antlr4.Runtime;
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
        public static List<Instructions.Instruction> Parse(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);

            AutoClickerLexer lexer = new AutoClickerLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            AutoClickerParser parser = new AutoClickerParser(commonTokenStream);

            AutoClickerParser.InstructionsContext context = parser.instructions();
            AutoClickerInstructionsVisitor visitor = new AutoClickerInstructionsVisitor();

            return visitor.Visit(context);
        }
    }
}
