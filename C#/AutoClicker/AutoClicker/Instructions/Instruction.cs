using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoClicker.Instructions
{
    public abstract class Instruction
    {
        internal Random random = new Random();
        internal static readonly int MAX_UNCERTAINTY = 20;
        private const int delayStep = 500;

        #region Properties
        public int? Delay { get; set; }
        public int? RandomDelay { get; set; }

        public int? Repetitions { get; set; } 
        public int? RandomRepetitions { get; set; } 

        public double? Speed { get; set; } 
        public double? RandomSpeed { get; set; }

        public bool Shift { get; set; } 
        public bool Ctrl { get; set; } 
        public bool Alt { get; set; } 

        public bool IsRunning { get; set; } = false;
        #endregion
        
        protected Instruction(int? delay = null, int? randomDelay = null, 
            int? repetitions = null, int? randomRepetitions = null, 
            double? speed = null, double? randomSpeed = null,
            bool shift = false, bool ctrl = false, bool alt = false) 
        {
            Delay = delay;
            RandomDelay = randomDelay;

            Repetitions = repetitions;
            RandomRepetitions = randomRepetitions;

            Speed = speed;
            RandomSpeed = randomSpeed;

            Shift = shift;
            Ctrl = ctrl;
            Alt = alt;
        }

        public void Run()
        {
            IsRunning = true;
            int save = Repetitions ?? MainWindow.GlobalRepetitions;
            int totalRepetitions = Repetitions ?? MainWindow.GlobalRepetitions + random.Next(RandomRepetitions ?? MainWindow.GlobalRandomRepetitions);
            Repetitions = 1;
            while (IsRunning && Repetitions <= totalRepetitions)
            {
                SpecificExecute();
                DoDelay();
                Repetitions++;
            }
            IsRunning = false;
            Repetitions = save;
        }

        internal abstract void SpecificExecute();

        private void DoDelay()
        {
            int save = Delay ?? MainWindow.GlobalDelay;
            int totalDelay = Delay ?? MainWindow.GlobalDelay + random.Next(RandomDelay ?? MainWindow.GlobalRandomDelay);

            Delay = 0;

            while (IsRunning && Delay < totalDelay)
            {
                int toDelay = Math.Min(totalDelay, Math.Min(delayStep, totalDelay - Delay ?? MainWindow.GlobalDelay));
                Delay += toDelay;
                Thread.Sleep(toDelay);
            }

            Delay = totalDelay;
            Delay = save;
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
            hashCode = hashCode * -1521134295 + Delay.GetHashCode();
            hashCode = hashCode * -1521134295 + RandomDelay.GetHashCode();
            hashCode = hashCode * -1521134295 + Repetitions.GetHashCode();
            hashCode = hashCode * -1521134295 + RandomRepetitions.GetHashCode();
            hashCode = hashCode * -1521134295 + Speed.GetHashCode();
            hashCode = hashCode * -1521134295 + RandomSpeed.GetHashCode();
            hashCode = hashCode * -1521134295 + Shift.GetHashCode();
            hashCode = hashCode * -1521134295 + Ctrl.GetHashCode();
            hashCode = hashCode * -1521134295 + Alt.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("\t");

            Append(builder, "delay", Delay, RandomDelay);
            Append(builder, "repetitions", Repetitions, RandomRepetitions);
            Append(builder, "speed", Speed, RandomSpeed);

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

        internal void Append(StringBuilder builder, string key, object value, object delta = null)
        {
            if (value == null)
            {
                return;
            }
            builder.Append("\t");
            builder.Append(key);
            builder.Append("=");
            builder.Append(value);

            if (delta == null)
            {
                return;
            }
            builder.Append("/");
            builder.Append(delta);
        }
    }
}
