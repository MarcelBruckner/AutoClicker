using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoClicker.Instructions
{
    public class Instruction
    {
        internal Random random = new Random();
        internal static readonly int MAX_UNCERTAINTY = 20;
        private const int delayStep = 500;

        #region Properties
        public IntTuple Delay { get; set; }
        public IntTuple Repetitions { get; set; }
        public DoubleTuple Speed { get; set; }

        public bool Shift { get; set; } 
        public bool Ctrl { get; set; } 
        public bool Alt { get; set; } 

        public bool IsRunning { get; set; } = false;
        #endregion
        
        public Instruction(IntTuple delay = null, IntTuple repetitions = null, DoubleTuple speed = null, 
            bool shift = false, bool ctrl = false, bool alt = false) 
        {
            Delay = delay;
            Repetitions = repetitions;
            Speed = speed;

            Shift = shift;
            Ctrl = ctrl;
            Alt = alt;
        }

        public void Run()
        {
            IsRunning = true;
            IntTuple repetitions = Repetitions ?? new IntTuple(MainWindow.GlobalRandomRepetitions);

            int save = repetitions.Value;
            int totalRepetitions = repetitions.Value + random.Next(repetitions.Random ?? MainWindow.GlobalRandomRepetitions);
            repetitions.Value = 1;
            while (IsRunning && repetitions.Value <= totalRepetitions)
            {
                SpecificExecute();
                DoDelay();
                Repetitions.Value++;
            }
            IsRunning = false;
            repetitions.Value = save;
        }

        internal virtual void SpecificExecute()
        {
            // EMPTY for override
        }

        private void DoDelay()
        {
            IntTuple delay = Delay ?? new IntTuple(MainWindow.GlobalDelay);

            int save = delay.Value;
            int totalDelay = delay.Value + random.Next(delay.Random ?? MainWindow.GlobalRandomDelay);

            delay.Value = 0;

            while (IsRunning && delay.Value < totalDelay)
            {
                int toDelay = Math.Min(totalDelay, Math.Min(delayStep, totalDelay - delay.Value));
                delay.Value += toDelay;
                Thread.Sleep(toDelay);
            }

            delay.Value = save;
        }

        protected VirtualKeyCode[] GetHotkeys()
        {
            List<VirtualKeyCode> hotkeys = new List<VirtualKeyCode>();
            if (Shift)
            {
                hotkeys.Add(VirtualKeyCode.SHIFT);
            }
            if (Alt)
            {
                hotkeys.Add(VirtualKeyCode.MENU);
            }
            if (Ctrl)
            {
                hotkeys.Add(VirtualKeyCode.CONTROL);
            }
            return hotkeys.ToArray();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Instructions.Instruction other))
            {
                return false;
            }

            return Shift == other.Shift &&
                Alt == other.Alt &&
                Ctrl == other.Alt;
        }

        internal int Randomize(int value, int range)
        {
            return value + random.Next(-range, range);
        }
        
        public override int GetHashCode()
        {
            var hashCode = -2077448662;
            hashCode = hashCode * -1521134295 + (Delay ?? new IntTuple(MainWindow.GlobalDelay)).GetHashCode();
            hashCode = hashCode * -1521134295 + (Repetitions ?? new IntTuple(MainWindow.GlobalRepetitions)).GetHashCode();
            hashCode = hashCode * -1521134295 + (Speed ?? new DoubleTuple(MainWindow.GlobalSpeed)).GetHashCode();
            hashCode = hashCode * -1521134295 + Shift.GetHashCode();
            hashCode = hashCode * -1521134295 + Ctrl.GetHashCode();
            hashCode = hashCode * -1521134295 + Alt.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("\t");

            Append(builder, "delay", Delay);
            Append(builder, "repetitions", Repetitions);
            Append(builder, "speed", Speed);

            if (Shift != MainWindow.GlobalShift)
            {
                Append(builder, "shift", Shift);
            }
            if (Ctrl != MainWindow.GlobalCtrl)
            {
                Append(builder, "ctrl", Ctrl);
            }
            if (Alt != MainWindow.GlobalAlt)
            {
                Append(builder, "alt", Alt);
            }
            return builder.ToString();
        }

        protected void Append(StringBuilder builder, string key, IntTuple tuple)
        {
            if(tuple == null)
            {
                return;
            }
            Append(builder, key, tuple.ToString());
        }

        protected void Append(StringBuilder builder, string key, DoubleTuple tuple)
        {
            if (tuple == null)
            {
                return;
            }
            Append(builder, key, tuple.ToString());
        }

        internal void Append(StringBuilder builder, string key, object value)
        {
            if (value == null)
            {
                return;
            }
            builder.Append("\t");
            builder.Append(key);
            builder.Append("=");
            builder.Append(value);
        }
    }
}
