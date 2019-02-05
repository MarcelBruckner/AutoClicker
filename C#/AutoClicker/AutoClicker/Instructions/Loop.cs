
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoClicker.Instructions
{
    class Loop : Instruction
    {
        private bool _isRunning;
        public override bool IsRunning
        {
            get => _isRunning;
            set
            {
                foreach (Instruction instruction in Instructions)
                {
                    instruction.IsRunning = value;
                }
                _isRunning = value;
            }
        }

        public List<Instruction> Instructions { get; private set; } = new List<Instruction>();

        public Loop() : this(1) { }

        public Loop(int repetitions) : this(0, repetitions) { }

        public Loop(int delay, int repetitions) : base(Action.LOOP, delay, repetitions) { }
        
        protected override void SpecificExecute()
        {
            foreach (Instruction instruction in Instructions)
            {
                if (!IsRunning)
                {
                    return;
                }
                instruction.Execute();
            }
        }

        public void Add(Instruction instruction)
        {
            Instructions.Add(instruction);
        }

        public override bool Equals(object obj)
        {
            var loop = obj as Loop;
            return loop != null &&
                   base.Equals(obj) &&
                   IsRunning == loop.IsRunning &&
                   EqualityComparer<List<Instruction>>.Default.Equals(Instructions, loop.Instructions);
        }

        public override int GetHashCode()
        {
            var hashCode = -833470266;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + IsRunning.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Instruction>>.Default.GetHashCode(Instructions);
            return hashCode;
        }
    }
}