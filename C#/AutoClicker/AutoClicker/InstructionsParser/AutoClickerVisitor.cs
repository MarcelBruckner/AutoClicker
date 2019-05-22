using AutoClicker.Instructions;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.InstructionsParser
{
    public class AutoClickerVisitor : AutoClickerBaseVisitor<object>
    {
        public override object VisitInstructions([NotNull] AutoClickerParser.InstructionsContext context)
        {
            List<Instructions.Instruction> instructions = new List<Instructions.Instruction>();

            foreach(var instruction in context.instruction())
            {
                instructions.Add((Instructions.Instruction)Visit(instruction));
            }

            return instructions;
        }

        public override object VisitClick([NotNull] AutoClickerParser.ClickContext context)
        {
            int x = -1;
            int y = -1;
            ButtonType? button = null;

            if (context.xPos() != null && context.xPos().Count() > 0)
            {
                x = (int)Visit(context.xPos(0));
            }
            if (context.yPos() != null && context.yPos().Count() > 0)
            {
                y = (int)Visit(context.yPos(0));
            }
            if(context.button() != null && context.button().Count() > 0)
            {
                button = (ButtonType?)Visit(context.button(0));
            }

            // TODO click creation
            return new Click(x, y, button: button);
        }

        public override object VisitXPos([NotNull] AutoClickerParser.XPosContext context)
        {
            return int.Parse(context.NUMBER().GetText());
        }

        public override object VisitYPos([NotNull] AutoClickerParser.YPosContext context)
        {
            return int.Parse(context.NUMBER().GetText());
        }

        public override object VisitButton([NotNull] AutoClickerParser.ButtonContext context)
        {
            if (context.RIGHT() != null)
            {
                return ButtonType.RIGHT;
            }
            if (context.MIDDLE() != null)
            {
                return ButtonType.MIDDLE;
            }
            if (context.LEFT() != null)
            {
                return ButtonType.LEFT;
            }
            return null;
        }
    }
}
