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
    /// <seealso cref="AutoClicker.AutoClickerBaseVisitor{Enums.MovementType?}" />
    public class AutoClickerMovementTypeVisitor : AutoClickerBaseVisitor<MovementType>
    {
        /// <summary>
        /// Visit a parse tree produced by <see cref="M:AutoClicker.AutoClickerParser.movement" />.
        /// <para>
        /// The default implementation returns the result of calling <see cref="M:Antlr4.Runtime.Tree.AbstractParseTreeVisitor`1.VisitChildren(Antlr4.Runtime.Tree.IRuleNode)" />
        /// on <paramref name="context" />.
        /// </para>
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <returns></returns>
        /// <return>The visitor result.</return>
        public override MovementType VisitMovement([NotNull] AutoClickerParser.MovementContext context)
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
            return MovementType.GLOBAL;
        }
    }
}
