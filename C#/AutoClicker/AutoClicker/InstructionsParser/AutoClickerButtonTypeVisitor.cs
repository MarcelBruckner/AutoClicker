using AutoClicker.Instructions;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using Enums;

namespace AutoClicker.InstructionsParser
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AutoClicker.AutoClickerBaseVisitor{Enums.ButtonType?}" />
    public class AutoClickerButtonTypeVisitor : AutoClickerBaseVisitor<ButtonType>
    {
        /// <summary>
        /// Visit a parse tree produced by <see cref="M:AutoClicker.AutoClickerParser.button" />.
        /// <para>
        /// The default implementation returns the result of calling <see cref="M:Antlr4.Runtime.Tree.AbstractParseTreeVisitor`1.VisitChildren(Antlr4.Runtime.Tree.IRuleNode)" />
        /// on <paramref name="context" />.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <returns></returns>
        /// <return>The visitor result.</return>
        public override ButtonType VisitButton([NotNull] AutoClickerParser.ButtonContext context)
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
            return ButtonType.GLOBAL;
        }
    }
}
