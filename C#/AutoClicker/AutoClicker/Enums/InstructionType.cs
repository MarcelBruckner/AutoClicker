using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker
{
    public enum InstructionType
    {
        CLICK,
        DRAG,
        WHEEL,
        KEYBOARD,
        LOOP,
        END_LOOP,
        DELAY,
    }
}
