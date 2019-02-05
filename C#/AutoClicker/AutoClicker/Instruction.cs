using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace AutoClicker.Instructions
{
    public class Instruction : INotifyPropertyChanged
    {
        #region Raw Properties
        private int _delay;
        private int _repetitions;
        private bool _shift;
        private bool _ctrl;
        private bool _alt;
        private VirtualKeyCode _key = VirtualKeyCode.NONE;
        private int _button;
        private int _x;
        private int _y;
        private int _endX;
        private int _endY;
        #endregion

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

        public virtual bool IsRunning { get; set; } = false;

        public int Delay { get => _delay; set { _delay = value; OnPropertyChanged("Delay"); } }
        public int Repetitions { get => _repetitions; set { _repetitions = value; OnPropertyChanged("Repetitions"); } }

        public bool Shift { get => _shift; set { _shift = value; OnPropertyChanged("Shift"); } }
        public bool Ctrl { get => _ctrl; set { _ctrl = value; OnPropertyChanged("Ctrl"); } }
        public bool Alt { get => _alt; set { _alt = value; OnPropertyChanged("Alt"); } }

        public VirtualKeyCode Key { get => _key; set { _key = value; OnPropertyChanged("Key"); } }

        public int Button { get => _button; set { _button = value; OnPropertyChanged("Button"); } }
        public int X { get => _x; set { _x = value; OnPropertyChanged("X"); } }
        public int Y { get => _y; set { _y = value; OnPropertyChanged("Y"); } }

        public int EndX { get => _endX; set { _endX = value; OnPropertyChanged("EndX"); } }
        public int EndY { get => _endY; set { _endY = value; OnPropertyChanged("EndY"); } }


        #region Constructors
        public Instruction() : this(Action.CLICK, 0, 1, false, false, false) { }

        public Instruction(Action type) : this(type, 0, 1, false, false, false) { }

        public Instruction(Action type, int delay, int repetitions, bool shift, bool ctrl, bool alt)
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

        public event PropertyChangedEventHandler PropertyChanged;
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
            IsRunning = false;
        }

        protected virtual void SpecificExecute()
        {
            switch (Type)
            {
                case Action.CLICK:
                    InputSimulator.MouseClick(Button, X, Y, GetHotkeys());
                    break;
                case Action.DRAG:
                    InputSimulator.MouseDrag(Button, X, Y, EndX, EndY, GetHotkeys());
                    break;
                case Action.KEYBOARD:
                    InputSimulator.KeyPress(Key, GetHotkeys());
                    break;
                case Action.DELAY:
                case Action.LOOP:
                case Action.END_LOOP:
                    break;
            }
        }

        public override string ToString()
        {
            string s = Type + " ";

            if (Delay > 50)
            {
                s += Property.AFTER + "=" + Delay + " ";
            }
            if (Repetitions > 1)
            {
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
    }
}