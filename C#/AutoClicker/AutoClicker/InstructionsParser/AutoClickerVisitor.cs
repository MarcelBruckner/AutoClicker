using AutoClicker.Instructions;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;

namespace AutoClicker.InstructionsParser
{
    public class AutoClickerVisitor : AutoClickerBaseVisitor<object>
    {
        public override object VisitInstructions([NotNull] AutoClickerParser.InstructionsContext context)
        {
            List<Instructions.Instruction> instructions = new List<Instructions.Instruction>();
            foreach (var instruction in context.instruction())
            {
                instructions.Add((Instructions.Instruction) Visit(instruction));
            }

            return instructions;
        }

        public override object VisitHover([NotNull] AutoClickerParser.HoverContext context)
        {
            IntTuple x = new IntTuple(0);
            IntTuple y = new IntTuple(0);
            MovementType? movement = null;
            Instructions.Instruction commons = Commons(context.commons());

            if (context.xPos() != null && context.xPos().Count() > 0)
            {
                x = (IntTuple)(Visit(context.xPos(0)) ?? x);
            }
            if (context.yPos() != null && context.yPos().Count() > 0)
            {
                y = (IntTuple)(Visit(context.yPos(0)) ?? y);
            }
            if (context.movement() != null && context.movement().Count() > 0)
            {
                movement = (MovementType?)Visit(context.movement(0));
            }

            return new Hover(x, y, movement, commons);
        }

        public override object VisitClick([NotNull] AutoClickerParser.ClickContext context)
        {
            IntTuple x = new IntTuple(0);
            IntTuple y = new IntTuple(0);
            MovementType? movement = null;
            Instructions.Instruction commons = Commons(context.commons());

            ButtonType? button = null;
            if (context.button() != null && context.button().Count() > 0)
            {
                button = (ButtonType?)Visit(context.button(0));
            }
            if (context.xPos() != null && context.xPos().Count() > 0)
            {
                x = (IntTuple)(Visit(context.xPos(0)) ?? x);
            }
            if (context.yPos() != null && context.yPos().Count() > 0)
            {
                y = (IntTuple)(Visit(context.yPos(0)) ?? y);
            }
            if (context.movement() != null && context.movement().Count() > 0)
            {
                movement = (MovementType?)Visit(context.movement(0));
            }

            return new Click(x, y, button, movement, commons);
        }

        public override object VisitDrag([NotNull] AutoClickerParser.DragContext context)
        {
            IntTuple x = new IntTuple(0);
            IntTuple y = new IntTuple(0);
            IntTuple endX = new IntTuple(0);
            IntTuple endY = new IntTuple(0);
            MovementType? movement = null;
            Instructions.Instruction commons = Commons(context.commons());

            ButtonType? button = null;
            if (context.button() != null && context.button().Count() > 0)
            {
                button = (ButtonType?)Visit(context.button(0));
            }
            if (context.xPos() != null && context.xPos().Count() > 0)
            {
                x = (IntTuple)(Visit(context.xPos(0)) ?? x);
            }
            if (context.yPos() != null && context.yPos().Count() > 0)
            {
                y = (IntTuple)(Visit(context.yPos(0)) ?? y);
            }
            if (context.endX() != null && context.endX().Count() > 0)
            {
                endX = (IntTuple)(Visit(context.endX(0)) ?? endX);
            }
            if (context.endY() != null && context.endY().Count() > 0)
            {
                endY = (IntTuple)(Visit(context.endY(0)) ?? endY);
            }
            if (context.movement() != null && context.movement().Count() > 0)
            {
                movement = (MovementType?)Visit(context.movement(0));
            }

            return new Drag(x, y, endX, endY, button, movement, commons);
        }

        public override object VisitKeystroke([NotNull] AutoClickerParser.KeystrokeContext context)
        {
            Instructions.Instruction commons = Commons(context.commons());

            VirtualKeyCode key = VirtualKeyCode.NONE;
            if (context.keyInput() != null && context.keyInput().Count() > 0)
            {
                key = (VirtualKeyCode)Visit(context.keyInput(0));
                
            }

            return new Keystroke(key, commons);
        }

        private Instructions.Instruction Commons(AutoClickerParser.CommonsContext[] contexts)
        {
            AutoClickerParser.CommonsContext delay = contexts.FirstOrDefault(context => context.delay() != null);
            AutoClickerParser.CommonsContext repetitions = contexts.FirstOrDefault(context => context.repetitions() != null);
            AutoClickerParser.CommonsContext speed = contexts.FirstOrDefault(context => context.speed() != null);
            AutoClickerParser.CommonsContext shift = contexts.FirstOrDefault(context => context.shift() != null);
            AutoClickerParser.CommonsContext ctrl = contexts.FirstOrDefault(context => context.ctrl() != null);
            AutoClickerParser.CommonsContext alt = contexts.FirstOrDefault(context => context.alt() != null);

            return new Instructions.Instruction(
                delay == null ? null : (IntTuple)Visit(delay),
                repetitions == null ? null : (IntTuple)Visit(repetitions),
                speed == null ? null : (DoubleTuple)Visit(speed),
                shift == null ? false : (bool)Visit(shift),
                ctrl == null ? false : (bool)Visit(ctrl),
                alt == null ? false : (bool)Visit(alt));
        }

        public override object VisitXPos([NotNull] AutoClickerParser.XPosContext context)
        {
            return Visit(context.intTuple());
        }

        public override object VisitYPos([NotNull] AutoClickerParser.YPosContext context)
        {
            return Visit(context.intTuple());
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

        public override object VisitMovement([NotNull] AutoClickerParser.MovementContext context)
        {
            if (context.SINUS() != null)
            {
                return MovementType.SINUS;
            }
            if (context.JUMP() != null)
            {
                return MovementType.JUMP;
            }
            if (context.SPRING() != null)
            {
                return MovementType.SPRING;
            }
            return null;
        }
                
        public override object VisitIntTuple([NotNull] AutoClickerParser.IntTupleContext context)
        {
            if(context.NUMBER(0).Symbol.TokenIndex < 0)
            {
                return null;
            }

            int value = int.Parse(context.NUMBER(0).GetText());
            int? delta = null;

            if (context.NUMBER().Count() > 1)
            {
                delta = int.Parse(context.NUMBER(1).GetText());
            }
            return new IntTuple(value, delta);
        }

        public override object VisitDoubleTuple([NotNull] AutoClickerParser.DoubleTupleContext context)
        {
            if (context.DECIMAL(0).Symbol.TokenIndex < 0)
            {
                return null;
            }

            double value = double.Parse(context.DECIMAL(0).GetText());
            double? delta = null;

            if (context.DECIMAL().Count() > 1)
            {
                delta = double.Parse(context.DECIMAL(1).GetText());
            }
            return new DoubleTuple(value, delta);
        }

        public override object VisitTrueFalse([NotNull] AutoClickerParser.TrueFalseContext context)
        {
            if(context.TRUE() != null)
            {
                return true;
            }
            return false;
        }

        public override object VisitKeyInput([NotNull] AutoClickerParser.KeyInputContext context)
        {
            return VirtualKeyCode.NONE.FromString(context.WORD().GetText());
        }
    }
}
