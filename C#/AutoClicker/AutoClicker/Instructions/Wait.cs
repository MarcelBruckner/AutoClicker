using Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoClicker.Instructions
{
    /// <summary>
    /// Base class for all instructions
    /// </summary>
    public class Wait : Instruction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Wait"/> class.
        /// </summary>
        /// <param name="delay">The delay.</param>
        /// <param name="repetitions">The repetitions.</param>
        public Wait(DecimalTuple delay = null, DecimalTuple repetitions = null) : base(delay, repetitions) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wait"/> class.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        public Wait(Instruction instruction) : base(instruction) { }

        /// <summary>
        /// Specifies the name of the instruction
        /// </summary>
        /// <returns>The name of the instruction</returns>
        internal override string GetName()
        {
            return "wait";
        }
    }
}