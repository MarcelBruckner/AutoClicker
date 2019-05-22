using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoClicker.Instructions
{
    public abstract class Instruction : INotifyPropertyChanged
    {
        internal Random random = new Random();
        internal static readonly int MAX_UNCERTAINTY = 20;
        private const int delayStep = 500;

        #region Variables
        private bool _isRunning = false;

        private long _delay = -1;
        private int _repetitions = -1;
        private bool _shift = false;
        private bool _ctrl = false;
        private bool _alt = false;

        private double _speed = -1;
        private long _randomDelay = -1;
        private int _randomRepetitions = -1;
        private double _randomSpeed = -1;
        #endregion

        #region Properties
        public long Delay { get => _delay; set { _delay = value; OnPropertyChanged("Delay"); } }
        public long RandomDelay { get => _randomDelay; set { _randomDelay = value; OnPropertyChanged("RandomDelay"); } }

        public int Repetitions { get => _repetitions; set { _repetitions = value; OnPropertyChanged("Repetitions"); } }
        public int RandomRepetitions { get => _randomRepetitions; set { _randomRepetitions = value; OnPropertyChanged("RandomRepetitions"); } }

        public double Speed { get => _speed; set { _speed = value; OnPropertyChanged("Speed"); } }
        public double RandomSpeed { get => _randomSpeed; set { _randomSpeed = value; OnPropertyChanged("RandomSpeed"); } }

        public bool Shift { get => _shift; set { _shift = value; OnPropertyChanged("Shift"); } }
        public bool Ctrl { get => _ctrl; set { _ctrl = value; OnPropertyChanged("Ctrl"); } }
        public bool Alt { get => _alt; set { _alt = value; OnPropertyChanged("Alt"); } }

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged("IsRunning");
            }
        }
        #endregion

        protected Instruction(bool shift, bool ctrl, bool alt)
        {
            Shift = shift;
            Ctrl = ctrl;
            Alt = alt;
        }

        protected Instruction(long delay, long randomDelay, int repetitions, int randomRepetitions, double speed, double randomSpeed, bool shift, bool ctrl, bool alt) 
            : this(shift, ctrl, alt)
        {
            Delay = delay;
            RandomDelay = randomDelay;
            Repetitions = repetitions;
            RandomRepetitions = randomRepetitions;
            Speed = speed;
            RandomSpeed = randomSpeed;
        }

        public void Run()
        {
            IsRunning = true;
            int save = Repetitions;
            int totalRepetitions = Repetitions + random.Next(RandomRepetitions);
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
            long save = Delay;
            long totalDelay = Delay + random.Next((int)RandomDelay);

            Delay = 0;

            while (IsRunning && Delay < totalDelay)
            {
                long toDelay = Math.Min(totalDelay, Math.Min(delayStep, totalDelay - Delay));
                Delay += toDelay;
                Thread.Sleep((int)toDelay);
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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

            Append(builder, "delay", Delay);
            Append(builder, "randomDelay", RandomDelay);

            Append(builder, "repetitions", Repetitions);
            Append(builder, "randomRepetitions", RandomRepetitions);

            Append(builder, "speed", Speed);
            Append(builder, "randomSpeed", RandomSpeed);

            Append(builder, "shift", Shift);
            Append(builder, "ctrl", Ctrl);
            Append(builder, "alt", Alt);

            return builder.ToString();
        }

        private void Append(StringBuilder builder, string key, int value)
        {
            if (value >= 0)
            {
                Append(builder, key, (object)value);
            }
        }

        internal void Append(StringBuilder builder, string key, double value)
        {
            if (value >= 0)
            {
                Append(builder, key, (object)value);
            }
        }

        internal void Append(StringBuilder builder, string key, bool value)
        {
            if (value)
            {
                Append(builder, key, (object)value);
            }
        }

        internal void Append(StringBuilder builder, string key, object value)
        {
            builder.Append("\t");
            builder.Append(key);
            builder.Append("=");
            builder.Append(value);
        }
    }
}
