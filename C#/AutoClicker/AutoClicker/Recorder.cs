using Gma.UserActivityMonitor;
using System;
using System.Windows.Forms;
using System.Windows.Documents;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace AutoClicker
{
    class Recorder : IKeyboardListener
    {
        private const int REPETITION_MAX_DELAY = 200;
        public bool IsRecording { get; set; }

        private MainWindow window;

        private DateTime lastActionTime = DateTime.Now;

        private bool isAltDown = false;
        private bool isCtrlDown = false;
        private bool isShiftDown = false;

        public bool WithDelay { get; set; }

        //private int current = -1;
        private Instruction current = null;
        private Instruction mouseDownPosition;

        #region Init Deinit
        public Recorder(MainWindow window)
        {
            this.window = window;
        }

        public void AddHooks()
        {
            //instructions.(false);
            HookManager.KeyDown += KeyDown;
            HookManager.KeyUp += KeyUp;

            HookManager.MouseDown += MouseDown;
            HookManager.MouseUp += MouseUp;

            HookManager.MouseWheel += MouseWheel;

            IsRecording = true;
        }

        public void RemoveHooks()
        {
            HookManager.KeyDown -= KeyDown;
            HookManager.KeyUp -= KeyUp;

            HookManager.MouseDown -= MouseDown;
            HookManager.MouseUp -= MouseUp;

            HookManager.MouseWheel -= MouseWheel;

            IsRecording = false;
        }
        #endregion

        #region Key Events
        private void KeyDown(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;

            if(key == MainWindow.RECORD_HOTKEY || key == MainWindow.PLAY_HOTKEY)
            {
                return;
            }

            bool hotkeyChanged = SetHotkeys(key + "", true);
            if (hotkeyChanged)
            {
                return;
            }

            Instruction instruction = new Instruction((VirtualKeyCode)(int)key, isShiftDown, isCtrlDown, isAltDown);
            AddOrIncrement(instruction);
        }

        private void KeyUp(object sender, KeyEventArgs e)
        {
            string key = e.KeyCode.ToString();
            bool hotkeyChanged = SetHotkeys(key, false);
        }

        private bool SetHotkeys(string _key, bool direction)
        {
            string key = _key.ToLower();
            if (key.Contains("menu"))
            {
                isAltDown = direction;
                return true;
            }
            else if (key.Contains("shift"))
            {
                isShiftDown = direction;
                return true;
            }
            else if (key.Contains("control"))
            {
                isCtrlDown = direction;
                return true;
            }
            return false;
        }

        #endregion

        #region Mouse Events

        private void MouseDown(object sender, MouseEventArgs e)
        {
            ButtonType button = GetButton(e.Button);
            Point p = Cursor.Point;
            mouseDownPosition = new Instruction(button, p.X, p.Y, isShiftDown, isCtrlDown, isAltDown);
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            ButtonType button = GetButton(e.Button);
            Point p = Cursor.Point;
            Instruction end = new Instruction(button, p.X, p.Y, isShiftDown, isCtrlDown, isAltDown);

            Instruction start = mouseDownPosition as Instruction;
            if (start != null && start.Button == end.Button && end.Distance(start) > Instruction.MAX_UNCERTAINTY)
            {
                AddOrIncrement(new Instruction(start.Button, start.X, start.Y, end.X, end.Y, isShiftDown, isCtrlDown, isAltDown));
            }
            else
            {
                AddOrIncrement(start);
            }
        }

        private void MouseWheel(object sender, MouseEventArgs e)
        {
            Instruction instruction = new Instruction(e.Delta, e.X, e.Y, isShiftDown, isCtrlDown, isAltDown);
            AddOrIncrement(instruction);
        }

        private ButtonType GetButton(MouseButtons button)
        {
            ButtonType _button;
            switch (button)
            {
                case MouseButtons.Middle:
                    _button = ButtonType.MIDDLE;
                    break;
                case MouseButtons.Right:
                    _button = ButtonType.RIGHT;
                    break;
                default:
                    _button = ButtonType.LEFT;
                    break;
            }
            return _button;
        }
        #endregion

        #region Helpers
        public int GetDelay()
        {
            DateTime now = DateTime.Now;
            TimeSpan delta = now - lastActionTime;
            lastActionTime = now;
            return (int)delta.TotalMilliseconds;
        }

        private bool AddOrIncrement(Instruction instruction)
        {
            int delay = GetDelay();

            if(current == null)
            {
                window.AddInstruction(instruction);
                current = instruction;
                return true;
            }

            if (instruction.Equals(current) && delay < REPETITION_MAX_DELAY)
            {
                if (instruction.Type == InstructionType.WHEEL)
                {
                    current.Wheel += instruction.Wheel;
                }
                else
                {
                    current.Repetitions++;
                }
            }
            else
            {
                if (WithDelay)
                {
                    current.Delay = delay;
                }
                window.AddInstruction(instruction);
                current = instruction;
            }

            return true;
        }


        #endregion
    }
}