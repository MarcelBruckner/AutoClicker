using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace AutoClicker
{
    public class Instruction : INotifyPropertyChanged
    {
        Random random = new Random();

        #region Raw Properties
        private long _delay = MainWindow.GlobalDelay;
        private int _repetitions = MainWindow.GlobalRepetitions;
        private bool _shift = MainWindow.GlobalShift;
        private bool _ctrl = MainWindow.GlobalCtrl;
        private bool _alt = MainWindow.GlobalAlt;
        private VirtualKeyCode _key = VirtualKeyCode.NONE;
        private ButtonType _button = ButtonType.LEFT;
        private int _x = 0;
        private int _y = 0;
        private int _endX = 0;
        private int _endY = 0;
        private bool _isRunning = false;
        private int _wheelDelta = MainWindow.GlobalWheel;
        private InstructionType _type;
        private MovementType _movement = MainWindow.GlobalMovement;
        private double _speed = MainWindow.GlobalSpeed;
        private long _randomDelay = MainWindow.GlobalRandomDelay;
        private int _randomRepetitions = MainWindow.GlobalRandomRepetitions;
        private double _randomSpeed = MainWindow.GlobalRandomSpeed;
        private int _randomX = MainWindow.GlobalRandomX;
        private int _randomY = MainWindow.GlobalRandomY;
        private int _randomEndY = MainWindow.GlobalRandomDragX;
        private int _randomEndX = MainWindow.GlobalRandomDragY;
        private int _randomWheelDelta = MainWindow.GlobalRandomWheel;
        internal static readonly int MAX_UNCERTAINTY = 20;
        #endregion

        private const int delayStep = 500;

        #region Properties
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged("IsRunning");
            }
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

        public InstructionType Type { get => _type; set { _type = value; OnPropertyChanged("Type"); } }

        public ButtonType Button { get => _button; set { _button = value; OnPropertyChanged("Button"); } }

        public MovementType Movement { get => _movement; set { _movement = value; OnPropertyChanged("Movement"); } }

        public long Delay { get => _delay; set { _delay = value; OnPropertyChanged("Delay"); } }
        public long RandomDelay { get => _randomDelay; set { _randomDelay = value; OnPropertyChanged("RandomDelay"); } }

        public int Repetitions { get => _repetitions; set { _repetitions = value; OnPropertyChanged("Repetitions"); } }
        public int RandomRepetitions { get => _randomRepetitions; set { _randomRepetitions = value; OnPropertyChanged("RandomRepetitions"); } }
        
        public double Speed { get => _speed; set { _speed = value; OnPropertyChanged("Speed"); } }
        public double RandomSpeed { get => _randomSpeed; set { _randomSpeed = value; OnPropertyChanged("RandomSpeed"); } }

        public int X { get => _x; set { _x = value; OnPropertyChanged("X"); } }
        public int RandomX { get => _randomX; set { _randomX = value; OnPropertyChanged("RandomX"); } }
        public int Y { get => _y; set { _y = value; OnPropertyChanged("Y"); } }
        public int RandomY { get => _randomY; set { _randomY = value; OnPropertyChanged("RandomY"); } }
        private Vector Randomized { get; set; } = new Vector(-100, -100);

        public int EndX { get => _endX; set { _endX = value; OnPropertyChanged("EndX"); } }
        public int RandomEndX { get => _randomEndX; set { _randomEndX = value; OnPropertyChanged("RandomEndX"); } }
        public int EndY { get => _endY; set { _endY = value; OnPropertyChanged("EndY"); } }
        public int RandomEndY { get => _randomEndY; set { _randomEndY = value; OnPropertyChanged("RandomEndY"); } }
        
        public int Wheel { get => _wheelDelta; set { _wheelDelta = value; OnPropertyChanged("Wheel"); } }
        public int RandomWheel { get => _randomWheelDelta; set { _randomWheelDelta = value; OnPropertyChanged("RandomWheel"); } }

        public bool Shift { get => _shift; set { _shift = value; OnPropertyChanged("Shift"); } }
        public bool Ctrl { get => _ctrl; set { _ctrl = value; OnPropertyChanged("Ctrl"); } }
        public bool Alt { get => _alt; set { _alt = value; OnPropertyChanged("Alt"); } }

        public VirtualKeyCode Key { get => _key; set { _key = value; OnPropertyChanged("Key"); } }
        #endregion

        #region Constructors
        public Instruction() : this(InstructionType.DELAY) { }

        public Instruction(InstructionType type) {
            Type = type;
        }

        public Instruction(Instruction other)
        {
            Delay = other.Delay;
            Repetitions = other.Repetitions;
            X = other.X;
            Y = other.Y;
            EndX = other.EndX;
            EndY = other.EndY;
            Ctrl = other.Ctrl;
            Shift = other.Shift;
            Alt = other.Alt;
            Button = other.Button;
            Key = other.Key;
            Wheel = other.Wheel;
            Type = other.Type;
        }
        
        public Instruction(InstructionType type, bool shift, bool ctrl, bool alt)
        {
            Shift = shift;
            Ctrl = ctrl;
            Alt = alt;
            Type = type;
        }

        // Click constructor
        public Instruction(ButtonType button, int x, int y, bool isShiftDown, bool isCtrlDown, bool isAltDown) : this(InstructionType.M_CLICK, isShiftDown, isCtrlDown, isAltDown)
        {
            SetMouse(button, x, y);
        }

        // Drag constructor
        public Instruction(ButtonType button, int x, int y, int endX, int endY, bool shift, bool ctrl, bool alt) : this(InstructionType.DRAG, shift, ctrl, alt)
        {
            SetMouse(button, x, y);

            EndX = endX;
            EndY = endY;
        }

        // Wheel constructor
        public Instruction(int wheelDelta, int x, int y, bool shift, bool ctrl, bool alt) : this(InstructionType.WHEEL, shift, ctrl, alt)
        {
            Wheel = wheelDelta;
            X = x;
            Y = y;
        }

        // Keyboard constructor
        public Instruction(VirtualKeyCode key, bool shift, bool ctrl, bool alt) : this(InstructionType.KEYBOARD, shift, ctrl, alt) {
            Key = key;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

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

        protected virtual void SpecificExecute()
        {
            if (!AlreadyThere)
            {
                Randomized = new Vector(Randomize(X, RandomX), Randomize(Y, RandomY));
            }

            switch (Type)
            {
                case InstructionType.CLICK:
                    InputSimulator.MouseClick(Button, GetHotkeys());
                    break;
                case InstructionType.M_CLICK:
                    InputSimulator.MouseClick(Movement, Button, Randomized, Randomize(Speed, RandomSpeed), GetHotkeys());
                    break;
                case InstructionType.DRAG:
                    InputSimulator.MouseDrag(Movement, Button, Randomized, Randomize(EndX, RandomEndX), Randomize(EndY, RandomEndY), Randomize(Speed, RandomSpeed), GetHotkeys());
                    break;
                case InstructionType.WHEEL:
                    InputSimulator.MouseWheel(Movement, Randomized, Randomize(Wheel, RandomWheel), Randomize(Speed, RandomSpeed), GetHotkeys());
                    break;
                case InstructionType.KEYBOARD:
                    InputSimulator.KeyPress(Key, GetHotkeys());
                    break;
                case InstructionType.DELAY:
                case InstructionType.LOOP:
                case InstructionType.END_LOOP:
                    break;
                default:
                    throw new ArgumentException("The instruction type " + Type + " is not supported.");
            }
        }

        private bool AlreadyThere
        {
            get
            {
                int dx = (int)Math.Abs(Randomized.X - Cursor.Vector.X);
                int dy = (int)Math.Abs(Randomized.Y - Cursor.Vector.Y);

                return dx < RandomX && dy < RandomY;
            }
        }

        private int Randomize(int value, int range)
        {
            return value + random.Next(-range, range);
        }

        private double Randomize(double value, double range)
        {
            return value + (2 * random.NextDouble() - 1) * range;
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
                    standards &= ClickSame(other) && Math.Sign(Wheel) == Math.Sign(other.Wheel);
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