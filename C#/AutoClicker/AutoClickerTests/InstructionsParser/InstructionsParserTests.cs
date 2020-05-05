using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Antlr4.Runtime;
using AutoClicker.Instructions;
using System;

namespace AutoClicker.InstructionsParser.Tests
{
    [TestClass()]
    public class InstructionsParserTests
    {
        private Drag testDrag = new Drag(
                new DecimalTuple(546, 22), new DecimalTuple(234, 4), 
                new DecimalTuple(546, 22), new DecimalTuple(234, 4), ButtonType.MIDDLE, MovementType.SPRING,
                new DecimalTuple(789, 43), new DecimalTuple(34, 342), new DecimalTuple(4.2, 3.9),
                true, true, true);

        private Click testClick = new Click(
                new DecimalTuple(546, 22), new DecimalTuple(234, 4), ButtonType.MIDDLE, MovementType.SPRING,
                new DecimalTuple(789, 43), new DecimalTuple(34, 342), new DecimalTuple(4.2, 3.9),
                true, true, true);

        private Hover testHover = new Hover(
                new DecimalTuple(546, 22), new DecimalTuple(234, 4), MovementType.SPRING,
                new DecimalTuple(789, 43), new DecimalTuple(34, 342), new DecimalTuple(4.2, 3.9),
                true, true, true);

        private Keystroke testKeystroke = new Keystroke(VirtualKeyCode.RETURN,
            new DecimalTuple(789, 43), new DecimalTuple(34, 342), new DecimalTuple(4.2, 3.9),
                true, true, true);

        [TestMethod()]
        public void ParseTest()
        {
            string input = testClick.ToString() + "\n";
            input += testHover.ToString() + "\n";
            input += testDrag.ToString() + "\n";
            input += testKeystroke.ToString() + "\n";

            AutoClickerParser parser = SetUp(input.ToString());
            AutoClickerParser.InstructionsContext context = parser.instructions();
            AutoClickerVisitor visitor = new AutoClickerVisitor();
            List<Instructions.Instruction> instructions = (List<Instructions.Instruction>)visitor.Visit(context);

            Assert.IsTrue(instructions.Contains(testClick));
            Assert.IsTrue(instructions.Contains(testHover));
            Assert.IsTrue(instructions.Contains(testDrag));
            Assert.IsTrue(instructions.Contains(testKeystroke));
        }

        public AutoClickerParser SetUp(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);

            AutoClickerLexer lexer = new AutoClickerLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            AutoClickerParser parser = new AutoClickerParser(commonTokenStream);

            return parser;
        }
    }
}