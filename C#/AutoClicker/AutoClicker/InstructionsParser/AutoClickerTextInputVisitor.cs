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
    /// <seealso cref="AutoClicker.AutoClickerBaseVisitor{System.String}" />
    public class AutoClickerTextInputVisitor : AutoClickerBaseVisitor<string>
    {
        /// <summary>
        /// Visit a parse tree produced by <see cref="M:AutoClicker.AutoClickerParser.stringInput" />.
        /// <para>
        /// The default implementation returns the result of calling <see cref="M:Antlr4.Runtime.Tree.AbstractParseTreeVisitor`1.VisitChildren(Antlr4.Runtime.Tree.IRuleNode)" />
        /// on <paramref name="context" />.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <returns></returns>
        /// <return>The visitor result.</return>
        public override string VisitStringInput([NotNull] AutoClickerParser.StringInputContext context)
        {
            string text = "";
            if (context.STRING() != null)
            {
                text = context.STRING().GetText();
                text = text.Remove(0, 1);
                text = text.Remove(text.Length - 1, 1);
            }
            return text;
        }
    }
}
