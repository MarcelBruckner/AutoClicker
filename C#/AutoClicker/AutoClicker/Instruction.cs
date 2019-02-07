using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace AutoClicker
{
    public class Instruction : INotifyPropertyChanged
    {
        #region Raw Properties
        private long _delay;
        private int _repetitions;
        private bool _shift;
        private bool _ctrl;
        private bool _alt;
        private VirtualKeyCode _key = VirtualKeyCode.NONE;
        private ButtonType _button;
        private int _x;
        private int _y;
        private int _endX;
        private int _endY;
        private bool _isRunning = false;
        private int _wheelDelta;
        internal static readonly int MAX_UNCERTAINTY = 20;
        #endregion

        public InstructionType Type { get; set; }

        public virtual bool IsRunning
        {
            get => _isRunning;
            set {
                _isRunning = value;
                int i = 0;
                while (IsRunning && (Repetitions == -1 || i < Repetitions))
                {
                    SpecificExecute();
                    Thread.Sleep((int)Delay);
                    i++;
                }
            }
        }

        public long Delay { get => _delay; set { _delay = value; OnPropertyChanged("Delay"); } }
        public int Repetitions { get => _repetitions; set { _repetitions = value; OnPropertyChanged("Repetitions"); } }

        public bool Shift { get => _shift; set { _shift = value; OnPropertyChanged("Shift"); } }
        public bool Ctrl { get => _ctrl; set { _ctrl = value; OnPropertyChanged("Ctrl"); } }
        public bool Alt { get => _alt; set { _alt = value; OnPropertyChanged("Alt"); } }

        public VirtualKeyCode Key { get => _key; set { _key = value; OnPropertyChanged("Key"); } }

        public ButtonType Button { get => _button; set { _button = value; OnPropertyChanged("Button"); } }
        public int X { get => _x; set { _x = value; OnPropertyChanged("X"); } }
        public int Y { get => _y; set { _y = value; OnPropertyChanged("Y"); } }

        public int EndX { get => _endX; set { _endX = value; OnPropertyChanged("EndX"); } }
        public int EndY { get => _endY; set { _endY = value; OnPropertyChanged("EndY"); } }

        public int WheelDelta { get => _wheelDelta; set { _wheelDelta = value; OnPropertyChanged("WheelDelta"); } }

        #region Constructors
        public Instruction() : this(InstructionType.DELAY, 0, 1, false, false, false) { }

        public Instruction(InstructionType type) : this(type, 0, 1, false, false, false) { }

        public Instruction(InstructionType type, long delay, int repetitions, bool shift, bool ctrl, bool alt)
        {
            Shift = shift;
            Ctrl = ctrl;
            Alt = alt;
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
            else
            {
                Repetitions = repetitions;
            }
        }

        // Click constructor
        public Instruction(ButtonType button, int x, int y, bool shift, bool ctrl, bool alt) : this(button, x, y, 0L, 1, shift, ctrl, alt) { }
        public Instruction(ButtonType button, int x, int y, long delay, int repetitions, bool shift, bool ctrl, bool alt) : this(InstructionType.CLICK, delay, repetitions, shift, ctrl, alt)
        {
            SetMouse(button, x, y);
        }

        // Drag constructor
        public Instruction(ButtonType button, int x, int y, int endX, int endY, bool shift, bool ctrl, bool alt) : this(button, x, y, endX, endY, 0L, 1, shift, ctrl, alt) { }
        public Instruction(ButtonType button, int x, int y, int endX, int endY, long delay, int repetitions, bool shift, bool ctrl, bool alt) : this(InstructionType.DRAG, delay, repetitions, shift, ctrl, alt)
        {
            SetMouse(button, x, y);

            EndX = endX;
            EndY = endY;
        }

        // Wheel constructor
        public Instruction(int wheelDelta, int x, int y, bool shift, bool ctrl, bool alt) : this(wheelDelta, x, y, 0L, 1, shift, ctrl, alt) { }
        public Instruction(int wheelDelta, int x, int y, long delay, int repetitions, bool shift, bool ctrl, bool alt) : this(InstructionType.WHEEL, delay, repetitions, shift, ctrl, alt)
        {
            WheelDelta = wheelDelta;
            X = x;
            Y = y;
        }

        // Keyboard constructor
        public Instruction(VirtualKeyCode key, bool shift, bool ctrl, bool alt) : this(InstructionType.KEYBOARD, 0, 1, shift, ctrl, alt) { }
        public Instruction(VirtualKeyCode key, int delay, int repetitions, bool shift, bool ctrl, bool alt) : this(InstructionType.KEYBOARD, delay, repetitions, shift, ctrl, alt)
        {
            Key = key;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        
        protected virtual void SpecificExecute()
        {
            switch (Type)
            {
                case InstructionType.CLICK:
                    InputSimulator.MouseClick(Button, X, Y, GetHotkeys());
                    break;
                case InstructionType.DRAG:
                    InputSimulator.MouseDrag(Button, X, Y, EndX, EndY, GetHotkeys());
                    break;
                case InstructionType.KEYBOARD:
                    InputSimulator.KeyPress(Key, GetHotkeys());
                    break;
                case InstructionType.WHEEL:
                    InputSimulator.MouseWheel(X, Y, WheelDelta);
                    break;
                case InstructionType.DELAY:
                case InstructionType.LOOP:
                case InstructionType.END_LOOP:
                    break;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Instruction other))
            {
                return false;
            }

            bool standards = 
                Type == other.Type &&
                Shift == other.Shift &&
                Alt == other.Alt &&
                Ctrl == other.Alt;
            
            switch (Type)
            {
                case InstructionType.WHEEL:
                    standards &= ClickSame(other) && Math.Sign(WheelDelta) == Math.Sign(other.WheelDelta);
                    break;
                case InstructionType.CLICK:
                    standards &= ClickSame(other);
                    break;
                case InstructionType.DRAG:
                    standards &= ClickSame(other) && EndX == other.EndX && EndY == other.EndY;
                    break;
                case InstructionType.KEYBOARD:
                    standards &= Key == other.Key;
                    break;
                case InstructionType.DELAY:
                case InstructionType.LOOP:
                case InstructionType.END_LOOP:
                    break;
            }

            return standards;
        }

        public bool ClickSame(Instruction other)
        {
            int distance = Distance(other);
            return Button == other.Button && Distance(other) < MAX_UNCERTAINTY;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public int Distance(Instruction other)
        {
            return (int)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }

        private void SetMouse(ButtonType button, int x, int y)
        {
            Button = button;
            X = x;
            Y = y;
        }
    }
}