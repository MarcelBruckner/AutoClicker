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
    public class Instruction
    {
        /// <summary>
        /// A random number generator
        /// </summary>
        internal static Random random = new Random();

        /// <summary>
        /// The step width in which the delays will be executed
        /// </summary>
        private const int delayStep = 500;

        #region Properties

        /// <summary>
        /// A tuple for the delay after the execution 
        /// </summary>
        private DecimalTuple _delay;
        public DecimalTuple Delay(bool valueNeeded = false)
        {
            if (!valueNeeded)
            {
                return _delay;
            }
            return _delay ?? new DecimalTuple(MainWindow.GlobalDelay, MainWindow.GlobalRandomDelay);
        }
        public void Delay(DecimalTuple delay)
        {
            _delay = delay;
        }

        /// <summary>
        /// A tuple for the repetitions for how often the instruction will be executed
        /// </summary>
        private DecimalTuple _repetitions;
        public DecimalTuple Repetitions
        {
            get
            {
                DecimalTuple repetitions = _repetitions ?? new DecimalTuple(MainWindow.GlobalRepetitions);
                _repetitions = repetitions;
                return _repetitions;
            }
            set => _repetitions = value;
        }

        /// <summary>
        /// A tuple for the speed of the execution
        /// </summary>
        private DecimalTuple _speed;
        public DecimalTuple Speed(bool valueNeeded = false)
        {
            if (!valueNeeded)
            {
                return _speed;
            }

            return _speed ?? new DecimalTuple(MainWindow.GlobalSpeed);
        }
        public void Speed(DecimalTuple speed)
        {
            _speed = speed;
        }

        /// <summary>
        /// Is the shift key pressed during execution
        /// </summary>
        public bool Shift { get; set; }

        /// <summary>
        /// Is the control key pressed during execution
        /// </summary>
        public bool Ctrl { get; set; }

        /// <summary>
        /// Is the alt key pressed during execution
        /// </summary>
        public bool Alt { get; set; }

        /// <summary>
        /// Is the instruction currently running
        /// </summary>
        public bool IsRunning { get; set; } = false;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Instruction"/> class.
        /// </summary>
        /// <param name="delay">The delay.</param>
        /// <param name="repetitions">The repetitions.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="shift">if set to <c>true</c> [shift].</param>
        /// <param name="ctrl">if set to <c>true</c> [control].</param>
        /// <param name="alt">if set to <c>true</c> [alt].</param>
        public Instruction(DecimalTuple delay = null, DecimalTuple repetitions = null, DecimalTuple speed = null,
            bool shift = false, bool ctrl = false, bool alt = false)
        {
            Delay(delay);
            Repetitions = repetitions;
            Speed(speed);

            Shift = shift;
            Ctrl = ctrl;
            Alt = alt;
        }

        /// <summary>
        /// Runs this instruction.
        /// </summary>
        public void Run()
        {
            IsRunning = true;
            DecimalTuple repetitions = Repetitions;

            int save = (int)repetitions.Value;
            int totalRepetitions = (int)repetitions.Value + random.Next((int)(repetitions.Random ?? MainWindow.GlobalRandomRepetitions));
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

        /// <summary>
        /// Instruction specific execution.
        /// </summary>
        internal virtual void SpecificExecute()
        {
            // EMPTY for override
        }

        /// <summary>
        /// Delays the execution flow after one repetition of this instruction has finished
        /// </summary>
        private void DoDelay()
        {
            DecimalTuple delay = Delay(true);
            double save = delay.Value;
            int totalDelay = delay.Get(MainWindow.GlobalRandomDelay);

            delay.Value = 0;

            while (IsRunning && delay.Value < totalDelay)
            {
                int toDelay = (int)Math.Min(totalDelay, Math.Min(delayStep, totalDelay - delay.Value));
                delay.Value += toDelay;
                Thread.Sleep(toDelay);
            }

            delay.Value = save;
        }

        /// <summary>
        /// An array of virtual key codes of the hotkeys that are pressed during execution
        /// </summary>
        /// <seealso cref="Shift"/>
        /// <seealso cref="Ctrl"/>
        /// <seealso cref="Alt"/>
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

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, resembles this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public virtual bool Resembles(object obj)
        {
            return false;
        }

        /// <summary>
        /// Specifies the name of the instruction
        /// </summary>
        /// <returns>The name of the instruction</returns>
        internal virtual string GetName()
        {
            return "";
        }

        /// <summary>
        /// Appends the key value pairs of specific properties of the instruction
        /// </summary>
        /// <param name="builder"></param>
        internal virtual void AppendSpecifics(StringBuilder builder)
        {
            // EMPTY for override
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
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

        /// <summary>
        /// Appends a key value pair to the string builder
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="key"></param>
        /// <param name="tuple"></param>
        protected void Append(StringBuilder builder, string key, DecimalTuple tuple)
        {
            if (tuple == null)
            {
                return;
            }
            Append(builder, key, tuple.ToString());
        }

        /// <summary>
        /// Appends a key value pair to the string builder
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="key"></param>
        /// <param name="tuple"></param>
        protected void Append(StringBuilder builder, string key, DecimalTuple tuple, double globalDefaultValue = 0)
        {
            if (tuple == null)
            {
                return;
            }
            Append(builder, key, tuple.ToString());
        }

        /// <summary>
        /// Appends a key value pair to the string builder
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
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

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = 628206347;
            hashCode = hashCode * -1521134295 + EqualityComparer<DecimalTuple>.Default.GetHashCode(_delay);
            hashCode = hashCode * -1521134295 + EqualityComparer<DecimalTuple>.Default.GetHashCode(_repetitions);
            hashCode = hashCode * -1521134295 + EqualityComparer<DecimalTuple>.Default.GetHashCode(_speed);
            hashCode = hashCode * -1521134295 + Shift.GetHashCode();
            hashCode = hashCode * -1521134295 + Ctrl.GetHashCode();
            hashCode = hashCode * -1521134295 + Alt.GetHashCode();
            return hashCode;
        }
    }
}