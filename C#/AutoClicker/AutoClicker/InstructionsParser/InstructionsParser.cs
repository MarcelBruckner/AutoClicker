using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.InstructionsParser
{
    public class InstructionsParser
    {
        public List<Instructions.Instruction> Parse(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);

            AutoClickerLexer lexer = new AutoClickerLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            AutoClickerParser parser = new AutoClickerParser(commonTokenStream);

            AutoClickerParser.InstructionsContext context = parser.instructions();
            AutoClickerVisitor visitor = new AutoClickerVisitor();

            return (List<Instructions.Instruction>)visitor.Visit(context);
        }
    }
}
