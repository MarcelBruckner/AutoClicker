using Antlr4.Runtime;
using System.Collections.Generic;
using AutoClicker;
using Xunit;
using AutoClicker.InstructionsParser;
using AutoClicker.Instructions;

namespace AutoClickerTest
{
    public class AutoClickerParserTest
    {
        public AutoClickerParser SetUp(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);

            AutoClickerLexer lexer = new AutoClickerLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            AutoClickerParser parser = new AutoClickerParser(commonTokenStream);

            return parser;
        }

        [Fact]
        public void TestClicks()
        {
            Click click = new Click(
                new IntTuple(546, 22), new IntTuple(234, 4), ButtonType.MIDDLE, MovementType.SPRING,
                new IntTuple(789, 43), new IntTuple(34, 342), new DoubleTuple(4.2, 3.9),
                true, true, true);

            AutoClickerParser parser = SetUp(click.ToString());

            AutoClickerParser.InstructionsContext context = parser.instructions();
            AutoClickerVisitor visitor = new AutoClickerVisitor();
            Click parsed = (Click)((List <AutoClicker.Instructions.Instruction> )visitor.Visit(context))[0];

            Assert.Equal(click, parsed);
        }
    }
}
