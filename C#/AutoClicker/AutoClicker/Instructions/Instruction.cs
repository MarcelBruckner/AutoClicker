using System;
using System.Threading;

namespace AutoClicker.Instructions
{
    public class Instruction
    {
        public enum Action
        {
            KEYBOARD,
            SPECIAL_KEYBOARD,
            CLICK,
            DELAY,
            LOOP,
            DRAG,
            END_LOOP,
            EMPTY
        }

        public enum Property
        {
            TYPE,
            AFTER,
            REPETITIONS,
            X,
            Y,
            BUTTON,
            END_X,
            END_Y,
            CONTROL,
            SHIFT,
            ALT,
            TEXT,
            KEY
        }

        public Action Type { get; set; }

        public virtual bool IsRunning { get; set; }
        public int Delay { get; set; }
        public int Repetitions { get; protected set; }

        #region Constructors
        public Instruction() : this(Action.CLICK) { }

        public Instruction(Action type) : this(type, 1) { }

        public Instruction(Action type, int repetitions) : this(type, 0, repetitions) { }

        public Instruction(Action type, int delay, int repetitions)
        {
            Type = type;
            if (delay < 50)
            {
                Delay = 50;
            }
            else
            {
                Delay = delay;
            }
            if (repetitions < 1)
            {
                Repetitions = 1;
            }
            else { 
                Repetitions = repetitions;
            }
        }
        #endregion
        
        public void Execute()
        {
            IsRunning = true;
            int i = 0;
            while (IsRunning && (Repetitions == -1 || i < Repetitions))
            {
                SpecificExecute();
                Thread.Sleep(Delay);
                i++;
            }
        }

        protected virtual void SpecificExecute() { }

        public void IncrementRepetition()
        {
            Repetitions++;
        }

        public override string ToString()
        {
            string s = Type + " ";

            if (Delay > 50) {
                s += Property.AFTER + "=" + Delay + " ";
            }
            if (Repetitions > 1) {
                s += Property.REPETITIONS + "=" + Repetitions;
            }

            return s;
        }

        public override bool Equals(object obj)
        {
            var instruction = obj as Instruction;
            return instruction != null &&
                   instruction.Type == Type;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}