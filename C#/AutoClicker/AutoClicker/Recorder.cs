using Gma.UserActivityMonitor;
using System;
using System.Windows.Forms;
using AutoClicker.Instructions;
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
        private const int REPETITION_MAX_DELAY = 200000;
        public bool IsRecording { get; set; }

        private MainWindow window;

        private DateTime lastActionTime = DateTime.Now;

        private bool isAltDown = false;
        private bool isCtrlDown = false;
        private bool isShiftDown = false;

        public bool WithDelay { get; set; }

        //private int current = -1;
        private Instruction current = null;
        private MouseClick mouseDownPosition;

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
            IsRecording = true;
        }

        public void RemoveHooks()
        {
            HookManager.KeyDown -= KeyDown;
            HookManager.KeyUp -= KeyUp;

            HookManager.MouseDown -= MouseDown;
            HookManager.MouseUp -= MouseUp;
            //instructions.EnableTextBox(true);
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

            Instruction instruction = new Keyboard((VirtualKeyCode)(int)key, 0, 1, isShiftDown, isCtrlDown, isAltDown);
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
            mouseDownPosition = new MouseClick(button, p.X, p.Y, 0, 1, isShiftDown, isCtrlDown, isAltDown);
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            int button = GetButton(e.Button);
            Point p = Cursor.Position;
            MouseClick end = new MouseClick(button, p.X, p.Y, 0, 1, isShiftDown, isCtrlDown, isAltDown);

            MouseClick start = mouseDownPosition as MouseClick;
            if (start != null && start.Button == end.Button && end.Distance(start) > MouseClick.MAX_UNCERTAINTY)
            {
                AddOrIncrement(new MouseDrag(start.Button, start.X, start.Y, end.X, end.Y, 0, 1, isShiftDown, isCtrlDown, isAltDown));
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

            if(current == null)
            {
                window.AddInstruction(instruction);
                current = instruction;
                return true;
            }

            if (instruction.Equals(current) && delay < REPETITION_MAX_DELAY)
            {
                current.Repetitions++;
                //instructions.UpdateLast(current);
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