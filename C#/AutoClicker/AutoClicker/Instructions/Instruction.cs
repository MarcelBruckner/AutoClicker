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

        public Action Type { get; protected set; }

        public virtual bool IsRunning { get; set; }
        public int Delay { get; protected set; }
        public int Repetitions { get; protected set; }

        #region Constructors
        public Instruction(Action type) : this(type, 1) { }

        public Instruction(Action type, int repetitions) : this(type, 0, repetitions) { }

        public Instruction(Action type, int delay, int repetitions)
        {
            Type = type;
            Delay = delay;
            Repetitions = repetitions;
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
            string s = Type + " " +
                Property.AFTER + "=" + Delay + " " +
                Property.REPETITIONS + "=" + Repetitions;

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