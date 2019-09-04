﻿using AutoClicker.Instructions;
using Antlr4.Runtime.Misc;
using System.Linq;
using Enums;

namespace AutoClicker.InstructionsParser
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AutoClicker.AutoClickerBaseVisitor{AutoClicker.Instructions.Instruction}" />
    public class AutoClickerInstructionVisitor : AutoClickerBaseVisitor<Instructions.Instruction>
    {
        /// <summary>
        /// The int tuple visitor
        /// </summary>
        private AutoClickerIntTupleVisitor intTupleVisitor = new AutoClickerIntTupleVisitor();

        /// <summary>
        /// The double tuple visitor
        /// </summary>
        private AutoClickerDoubleTupleVisitor doubleTupleVisitor = new AutoClickerDoubleTupleVisitor();

        /// <summary>
        /// The enum visitor
        /// </summary>
        private AutoClickerButtonTypeVisitor buttonTypeVisitor = new AutoClickerButtonTypeVisitor();

        /// <summary>
        /// The movement type visitor
        /// </summary>
        private AutoClickerMovementTypeVisitor movementTypeVisitor = new AutoClickerMovementTypeVisitor();

        /// <summary>
        /// The true false visitor
        /// </summary>
        private AutoClickerTrueFalseVisitor trueFalseVisitor = new AutoClickerTrueFalseVisitor();

        /// <summary>
        /// The key input visitor
        /// </summary>
        private AutoClickerKeyInputVisitor keyInputVisitor = new AutoClickerKeyInputVisitor();

        /// <summary>
        /// The text input visitor
        /// </summary>
        private AutoClickerTextInputVisitor textInputVisitor = new AutoClickerTextInputVisitor();

        /// <summary>
        /// Visit a parse tree produced by <see cref="M:AutoClicker.AutoClickerParser.hover" />.
        /// <para>
        /// The default implementation returns the result of calling <see cref="M:Antlr4.Runtime.Tree.AbstractParseTreeVisitor`1.VisitChildren(Antlr4.Runtime.Tree.IRuleNode)" />
        /// on <paramref name="context" />.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <returns></returns>
        /// <return>The visitor result.</return>
        public override Instructions.Instruction VisitHover([NotNull] AutoClickerParser.HoverContext context)
        {
            IntTuple x = new IntTuple(0);
            IntTuple y = new IntTuple(0);
            MovementType? movement = null;
            Instructions.Instruction commons = Commons(context.commons());

            if (context.xPos() != null && context.xPos().Count() > 0)
            {
                x = intTupleVisitor.Visit(context.xPos(0)) ?? x;
            }
            if (context.yPos() != null && context.yPos().Count() > 0)
            {
                y = intTupleVisitor.Visit(context.yPos(0)) ?? y;
            }
            if (context.movement() != null && context.movement().Count() > 0)
            {
                movement = movementTypeVisitor.Visit(context.movement(0));
            }

            return new Hover(x, y, movement, commons);
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="M:AutoClicker.AutoClickerParser.click" />.
        /// <para>
        /// The default implementation returns the result of calling <see cref="M:Antlr4.Runtime.Tree.AbstractParseTreeVisitor`1.VisitChildren(Antlr4.Runtime.Tree.IRuleNode)" />
        /// on <paramref name="context" />.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <returns></returns>
        /// <return>The visitor result.</return>
        public override Instructions.Instruction VisitClick([NotNull] AutoClickerParser.ClickContext context)
        {
            IntTuple x = new IntTuple(0);
            IntTuple y = new IntTuple(0);
            MovementType? movement = null;
            Instructions.Instruction commons = Commons(context.commons());

            ButtonType? button = null;
            if (context.button() != null && context.button().Count() > 0)
            {
                button = buttonTypeVisitor.Visit(context.button(0));
            }
            if (context.xPos() != null && context.xPos().Count() > 0)
            {
                x = intTupleVisitor.Visit(context.xPos(0)) ?? x;
            }
            if (context.yPos() != null && context.yPos().Count() > 0)
            {
                y = intTupleVisitor.Visit(context.yPos(0)) ?? y;
            }
            if (context.movement() != null && context.movement().Count() > 0)
            {
                movement = movementTypeVisitor.Visit(context.movement(0));
            }

            return new Click(x, y, button, movement, commons);
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="M:AutoClicker.AutoClickerParser.drag" />.
        /// <para>
        /// The default implementation returns the result of calling <see cref="M:Antlr4.Runtime.Tree.AbstractParseTreeVisitor`1.VisitChildren(Antlr4.Runtime.Tree.IRuleNode)" />
        /// on <paramref name="context" />.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <returns></returns>
        /// <return>The visitor result.</return>
        public override Instructions.Instruction VisitDrag([NotNull] AutoClickerParser.DragContext context)
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
                button = buttonTypeVisitor.Visit(context.button(0));
            }
            if (context.xPos() != null && context.xPos().Count() > 0)
            {
                x = intTupleVisitor.Visit(context.xPos(0)) ?? x;
            }
            if (context.yPos() != null && context.yPos().Count() > 0)
            {
                y = intTupleVisitor.Visit(context.yPos(0)) ?? y;
            }
            if (context.endX() != null && context.endX().Count() > 0)
            {
                endX = intTupleVisitor.Visit(context.endX(0)) ?? endX;
            }
            if (context.endY() != null && context.endY().Count() > 0)
            {
                endY = intTupleVisitor.Visit(context.endY(0)) ?? endY;
            }
            if (context.movement() != null && context.movement().Count() > 0)
            {
                movement = movementTypeVisitor.Visit(context.movement(0));
            }

            return new Drag(x, y, endX, endY, button, movement, commons);
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="M:AutoClicker.AutoClickerParser.keystroke" />.
        /// <para>
        /// The default implementation returns the result of calling <see cref="M:Antlr4.Runtime.Tree.AbstractParseTreeVisitor`1.VisitChildren(Antlr4.Runtime.Tree.IRuleNode)" />
        /// on <paramref name="context" />.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <returns></returns>
        /// <return>The visitor result.</return>
        public override Instructions.Instruction VisitKeystroke([NotNull] AutoClickerParser.KeystrokeContext context)
        {
            Instructions.Instruction commons = Commons(context.commons());

            VirtualKeyCode key = VirtualKeyCode.NONE;
            if (context.keyInput() != null && context.keyInput().Count() > 0)
            {
                key = keyInputVisitor.Visit(context.keyInput(0));
            }

            return new Keystroke(key, commons);
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="M:AutoClicker.AutoClickerParser.text" />.
        /// <para>
        /// The default implementation returns the result of calling <see cref="M:Antlr4.Runtime.Tree.AbstractParseTreeVisitor`1.VisitChildren(Antlr4.Runtime.Tree.IRuleNode)" />
        /// on <paramref name="context" />.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <returns></returns>
        /// <return>The visitor result.</return>
        public override Instructions.Instruction VisitText([NotNull] AutoClickerParser.TextContext context)
        {
            Instructions.Instruction commons = Commons(context.commons());

            string text = "";
            if (context.stringInput() != null && context.stringInput().Count() > 0)
            {
                text = textInputVisitor.Visit(context.stringInput(0));
            }

            return new Text(text, commons);
        }

        /// <summary>
        /// Visits the common fields of the instructions.
        /// </summary>
        /// <param name="contexts">The contexts.</param>
        /// <returns></returns>
        private Instructions.Instruction Commons(AutoClickerParser.CommonsContext[] contexts)
        {
            AutoClickerParser.CommonsContext delay = contexts.FirstOrDefault(context => context.delay() != null);
            AutoClickerParser.CommonsContext repetitions = contexts.FirstOrDefault(context => context.repetitions() != null);
            AutoClickerParser.CommonsContext speed = contexts.FirstOrDefault(context => context.speed() != null);
            AutoClickerParser.CommonsContext shift = contexts.FirstOrDefault(context => context.shift() != null);
            AutoClickerParser.CommonsContext ctrl = contexts.FirstOrDefault(context => context.ctrl() != null);
            AutoClickerParser.CommonsContext alt = contexts.FirstOrDefault(context => context.alt() != null);

            return new Instructions.Instruction(
                delay == null ? null : intTupleVisitor.Visit(delay),
                repetitions == null ? null : intTupleVisitor.Visit(repetitions),
                speed == null ? null : doubleTupleVisitor.Visit(speed),
                shift == null ? false : trueFalseVisitor.Visit(shift),
                ctrl == null ? false : trueFalseVisitor.Visit(ctrl),
                alt == null ? false : trueFalseVisitor.Visit(alt));
        }
    }
}