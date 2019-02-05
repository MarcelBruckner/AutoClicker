using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoClicker.Instructions
{
    public class Delay : Instruction
    {
        #region Constructors
        public Delay() : this(1) { }

        public Delay(int repetitions) : this(0, repetitions) { }

        public Delay(int delay, int repetitions) : base(Action.DELAY,delay,repetitions)
        {
            Delay = delay;
            Repetitions = repetitions;
        }
        #endregion

    }
}
