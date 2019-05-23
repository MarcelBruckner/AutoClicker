using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoClicker.Instructions
{
    /** Base class for instructions */
    public class Instruction
    {
        /** A random number generator */
        internal Random random = new Random();

        /** The step width in which the delays will be executed */
        private const int delayStep = 500;

        #region Properties
        /** A tuple for the delay after the execution */
        public IntTuple _delay;
        public IntTuple Delay { get => _delay ?? new IntTuple(MainWindow.GlobalDelay, MainWindow.GlobalRandomDelay); set => _delay = value; }

        /** A tuple for the repetitions for how often the instruction will be executed */
        public IntTuple _repetitions;
        public IntTuple Repetitions { get => _repetitions ?? new IntTuple(MainWindow.GlobalRepetitions, MainWindow.GlobalRandomRepetitions); set => _repetitions = value; }

        /** A tuple for the speed of the execution */
        public DoubleTuple _speed;
        public DoubleTuple Speed { get => _speed ?? new DoubleTuple(MainWindow.GlobalSpeed, MainWindow.GlobalRandomSpeed); set => _speed = value; }

        /** Is the shift key pressed during execution */
        public bool Shift { get; set; }

        /** Is the control key pressed during execution */
        public bool Ctrl { get; set; }
        
        /** Is the alt key pressed during execution */
        public bool Alt { get; set; }

        /** Is the instruction currently running */
        public bool IsRunning { get; set; } = false;
        #endregion

        /** constructor */
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

        /** The method to execute the instruction */
        public void Run()
        {
            IsRunning = true;
            IntTuple repetitions = Repetitions;

            int save = repetitions.Value;
            int totalRepetitions = repetitions.Value + random.Next(repetitions.Random ?? MainWindow.GlobalRandomRepetitions);
            repetitions.Value = 1;
            while (IsRunning && repetitions.Value <= totalRepetitions)
            {
                SpecificExecute();
                DoDelay();
                repetitions.Value++;
            }
            IsRunning = false;
            repetitions.Value = save;
        }

        /** Override this method to implement the specific execution steps of the instructions */
        internal virtual void SpecificExecute()
        {
            // EMPTY for override
        }

        /** Delays the execution flow after one repetition of this instruction has finished */
        private void DoDelay()
        {
            IntTuple delay = Delay;

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

        /** An array of virtual key codes of the hotkeys that are pressed during execution */
        protected VirtualKeyCode[] Hotkeys
        {
            get
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
        }

        /** equals */
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

        /** Randomizes a tuple based on its value and random value */
        internal double Randomize(IntTuple tuple)
        {
            return Randomize(tuple.Value, tuple.Random ?? 0);
        }

        /** Randomizes a tuple based on its value and random value */
        internal double Randomize(DoubleTuple tuple)
        {
            return Randomize(tuple.Value, tuple.Random ?? 0.0);
        }

        /** Randomizes a value in the interval [value - range, value + range] */
        internal double Randomize(double value, double range)
        {
            return value + ((2 * random.NextDouble()) - 1) * range;
        }
               
        /** Override to specify the name of the instruction */
        internal virtual string GetName()
        {
            return "";
        }

        /** Override to append the key value pairs of specific properties of the instructions */
        internal virtual void AppendSpecifics(StringBuilder builder) { }

        /** toString */
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(GetName() + ":");

            AppendSpecifics(builder);

            Append(builder, "delay", _delay);
            Append(builder, "repetitions", _repetitions);
            Append(builder, "speed", _speed);

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

        /** Appends a key value pair to the string builder */
        protected void Append(StringBuilder builder, string key, IntTuple tuple)
        {
            if (tuple == null)
            {
                return;
            }
            Append(builder, key, tuple.ToString());
        }

        /** Appends a key value pair to the string builder */
        protected void Append(StringBuilder builder, string key, DoubleTuple tuple)
        {
            if (tuple == null)
            {
                return;
            }
            Append(builder, key, tuple.ToString());
        }

        /** Appends a key value pair to the string builder */
        internal void Append(StringBuilder builder, string key, object value)
        {
            if (value == null)
            {
                return;
            }
            builder.Append(" \t");
            builder.Append(key);
            builder.Append("=");
            builder.Append(value);
        }

        /** hash code */
        public override int GetHashCode()
        {
            var hashCode = 628206347;
            hashCode = hashCode * -1521134295 + EqualityComparer<IntTuple>.Default.GetHashCode(_delay);
            hashCode = hashCode * -1521134295 + EqualityComparer<IntTuple>.Default.GetHashCode(_repetitions);
            hashCode = hashCode * -1521134295 + EqualityComparer<DoubleTuple>.Default.GetHashCode(_speed);
            hashCode = hashCode * -1521134295 + Shift.GetHashCode();
            hashCode = hashCode * -1521134295 + Ctrl.GetHashCode();
            hashCode = hashCode * -1521134295 + Alt.GetHashCode();
            return hashCode;
        }
    }
}
