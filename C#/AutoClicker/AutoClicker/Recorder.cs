using Gma.UserActivityMonitor;
using System;
using System.Windows.Forms;
using AutoClicker.Instructions;
using System.Windows.Documents;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Threading;

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

        private Instruction current = null;
        private MouseClick mouseDownPosition;

        #region Init Deinit
        public Recorder(MainWindow window)
        {
            this.window = window;
        }

        public void AddHooks()
        {
            window.EnableTextBox(false);
            HookManager.KeyDown += KeyDown;
            HookManager.KeyUp += KeyUp;

            HookManager.MouseDown += MouseDown;
            HookManager.MouseUp += MouseUp;
            IsRecording = true;
        }

        public void RemoveHooks()
        {
            HookManager.KeyDown -= KeyDown;
            HookManager.KeyUp -= KeyUp;

            HookManager.MouseDown -= MouseDown;
            HookManager.MouseUp -= MouseUp;
            window.EnableTextBox(true);
            IsRecording = false;
        }
        #endregion

        #region Key Events
        private void KeyDown(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;

            if (key == Keys.F7)
            {
                return;
            }

            bool hotkeyChanged = SetHotkeys(key + "", true);
            if (hotkeyChanged)
            {
                return;
            }

            Instruction instruction;
            if (key == Keys.Enter || key == Keys.Tab || key == Keys.Back || key == Keys.Space || isAltDown || isShiftDown || isCtrlDown)
            {
                instruction = new SpecialKeyboard((VirtualKeyCode)(int)key, isShiftDown, isCtrlDown, isAltDown, 0, 1);
            }
            else
            {
                instruction = new Keyboard((char)key + "", 0, 1);
            }
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
            int button = GetButton(e.Button);
            Point p = Cursor.Position;
            mouseDownPosition = new MouseClick(button, p.X, p.Y, 0, 1);
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            int button = GetButton(e.Button);
            Point p = Cursor.Position;
            MouseClick end = new MouseClick(button, p.X, p.Y, 0, 1);

            MouseClick start = mouseDownPosition as MouseClick;
            if (start != null && start.Button == end.Button && end.Distance(start) > MouseClick.MAX_UNCERTAINTY)
            {
                AddOrIncrement(new MouseDrag(start.Button, start.Position.X, start.Position.Y, end.Position.X, end.Position.Y, 0, 1));
            }
            else
            {
                AddOrIncrement(start);
            }
        }

        private int GetButton(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Middle:
                    return 2;
                case MouseButtons.Right:
                    return 1;
                default:
                    return 0;
            }
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
            if (instruction.Equals(current) && delay < REPETITION_MAX_DELAY)
            {
                current.IncrementRepetition();
                window.UpdateLast(current);
            }
            else
            {
                if (current != null)
                {
                    if (WithDelay)
                    {
                        current.Delay = delay;
                    }
                    window.UpdateLast(current);
                }
                window.AddInstruction(instruction);
                current = instruction;
            }

            return true;
        }


        #endregion
    }
}