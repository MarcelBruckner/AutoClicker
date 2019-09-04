using AutoClicker.Instructions;
using Antlr4.Runtime.Misc;
using System.Linq;

namespace AutoClicker.InstructionsParser
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AutoClicker.AutoClickerBaseVisitor{AutoClicker.Instructions.IntTuple}" />
    public class AutoClickerIntTupleVisitor : AutoClickerBaseVisitor<IntTuple>
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
        public override IntTuple VisitXPos([NotNull] AutoClickerParser.XPosContext context)
        {
            return Visit(context.intTuple());
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
        public override IntTuple VisitYPos([NotNull] AutoClickerParser.YPosContext context)
        {
            return Visit(context.intTuple());
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
        public override IntTuple VisitIntTuple([NotNull] AutoClickerParser.IntTupleContext context)
        {
            if (context.NUMBER(0).Symbol.TokenIndex < 0)
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
    }
}
