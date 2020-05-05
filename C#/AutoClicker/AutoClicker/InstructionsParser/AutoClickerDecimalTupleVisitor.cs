using AutoClicker.Instructions;
using Antlr4.Runtime.Misc;
using System.Linq;

namespace AutoClicker.InstructionsParser
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AutoClicker.AutoClickerBaseVisitor{AutoClicker.Instructions.DecimalTuple}" />
    public class AutoClickerDecimalTupleVisitor : AutoClickerBaseVisitor<DecimalTuple>
    {
        /// <summary>
        /// Visit a parse tree produced by <see cref="M:AutoClicker.AutoClickerParser.xPos" />.
        /// <para>
        /// The default implementation returns the result of calling <see cref="M:Antlr4.Runtime.Tree.AbstractParseTreeVisitor`1.VisitChildren(Antlr4.Runtime.Tree.IRuleNode)" />
        /// on <paramref name="context" />.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <returns></returns>
        /// <return>The visitor result.</return>
        public override DecimalTuple VisitXPos([NotNull] AutoClickerParser.XPosContext context)
        {
            return Visit(context.decimalTuple());
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="M:AutoClicker.AutoClickerParser.yPos" />.
        /// <para>
        /// The default implementation returns the result of calling <see cref="M:Antlr4.Runtime.Tree.AbstractParseTreeVisitor`1.VisitChildren(Antlr4.Runtime.Tree.IRuleNode)" />
        /// on <paramref name="context" />.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <returns></returns>
        /// <return>The visitor result.</return>
        public override DecimalTuple VisitYPos([NotNull] AutoClickerParser.YPosContext context)
        {
            return Visit(context.decimalTuple());
        }

        /// <summary>
        /// Visit a parse tree produced by <see cref="M:AutoClicker.AutoClickerParser.intTuple" />.
        /// <para>
        /// The default implementation returns the result of calling <see cref="M:Antlr4.Runtime.Tree.AbstractParseTreeVisitor`1.VisitChildren(Antlr4.Runtime.Tree.IRuleNode)" />
        /// on <paramref name="context" />.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <returns></returns>
        /// <return>The visitor result.</return>
        public override DecimalTuple VisitDecimalTuple([NotNull] AutoClickerParser.DecimalTupleContext context)
        {
            if (context.DECIMAL(0).Symbol.TokenIndex < 0)
            {
                return null;
            }

            int value = int.Parse(context.DECIMAL(0).GetText());
            int? delta = null;

            if (context.DECIMAL().Count() > 1)
            {
                delta = int.Parse(context.DECIMAL(1).GetText());
            }
            return new DecimalTuple(value, delta);
        }
    }
}
