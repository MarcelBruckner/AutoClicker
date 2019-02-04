using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoClicker.Instructions
{
    public class Instruction
    {
        public enum InstructionType
        {
            KEYBOARD,
            SPECIAL_KEYBOARD,
            CLICK,
            DELAY,
            LOOP,
            DRAG
        }

        public enum InstructionProperty
        {
            TYPE,
            PREVIOUS,
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
            TEXT
        }

        private bool _isRunning;

        public int DelayPrevious { get; protected set; }
        public int DelayAfter { get; protected set; }
        public int Repetitions { get; protected set; }

        public InstructionType Type { get; protected set; }

        public List<Instruction> Instructions { get; private set; } = new List<Instruction>();

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                foreach(Instruction instruction in Instructions)
                {
                    instruction.IsRunning = value;
                }
                _isRunning = value;
            }
        }
        #region Constructors
        public Instruction(InstructionType type) : this(type, 1) { }

        public Instruction(InstructionType type, int repetitions) : this(type, 0, 0, repetitions) { }

        public Instruction(InstructionType type, int delayPrevious, int delayAfter, int repetitions)
        {
            Type = type;
            DelayPrevious = delayPrevious;
            DelayAfter = delayAfter;
            Repetitions = repetitions;
        }

        public Instruction(Dictionary<string, string> raw)
        {
            Parse(raw);
        }
        #endregion

        #region Parse Helpers
        private int ParsePreviousDelay(Dictionary<string, string> raw)
        {
            if (raw.ContainsKey(InstructionProperty.PREVIOUS.ToString()))
            {
                return int.Parse(raw[InstructionProperty.PREVIOUS.ToString()]);
            }
            return 0;
        }

        private int ParseAfterDelay(Dictionary<string, string> raw)
        {
            if (raw.ContainsKey(InstructionProperty.AFTER.ToString()))
            {
                return int.Parse(raw[InstructionProperty.AFTER.ToString()]);
            }
            return 0;
        }

        private int ParseRepetitions(Dictionary<string, string> raw)
        {
            if (raw.ContainsKey(InstructionProperty.REPETITIONS.ToString()))
            {
                return int.Parse(raw[InstructionProperty.REPETITIONS.ToString()]);
            }
            return 1;
        }
        #endregion

        protected virtual void Parse(Dictionary<string, string> raw)
        {
            DelayPrevious = ParsePreviousDelay(raw);
            DelayAfter = ParseAfterDelay(raw);
            Repetitions = ParseRepetitions(raw);
            Type = (InstructionType)Enum.Parse(typeof(InstructionType), raw[InstructionProperty.TYPE.ToString().ToLower()]);
        }

        public void Execute()
        {
            IsRunning = true;
            int i = 0;
            while (IsRunning && (Repetitions == -1 || i < Repetitions))
            {
                Thread.Sleep(DelayPrevious);
                SpecificExecute();
                Thread.Sleep(DelayAfter);
                i++;
            }
        }

        protected virtual void SpecificExecute()
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

        public void IncrementRepetition()
        {
            Repetitions++;
        }

        public override string ToString()
        {
            string s = InstructionProperty.TYPE + "=" + Type + " " + 
                InstructionProperty.PREVIOUS + "=" + DelayPrevious + " " + 
                InstructionProperty.AFTER + "=" + DelayAfter + " " + 
                InstructionProperty.REPETITIONS + "=" + Repetitions;

            if(Type == InstructionType.LOOP)
            {
                s += "\n";
                s += StringManager.END_LOOP;
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
