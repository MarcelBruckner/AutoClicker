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
using AutoClicker.Instructions;
using Enums;

namespace AutoClicker
{
    class Recorder : IKeyboardListener
    {
        private const int REPETITION_MAX_DELAY = 200;
        public bool IsRecording { get; set; }

        private MainWindow window;

        private DateTime lastActionTime = DateTime.Now;

        private bool isAltDown;
        private bool isCtrlDown;
        private bool isShiftDown;

        public bool WithDelay { get; set; }

        //private int current = -1;
        private Instructions.Instruction current = null;
        private Instructions.Instruction mouseDownPosition;

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

            bool hotkeyChanged = SetHotkeys(key, true);
            if (hotkeyChanged)
            {
                return;
            }

            //TODO Keyboard
            Keystroke instruction = new Keystroke((VirtualKeyCode)(int)key, shift: isShiftDown, ctrl: isCtrlDown, alt: isAltDown);
            AddOrIncrement(instruction);
        }

        private void KeyUp(object sender, KeyEventArgs e)
        {
            bool hotkeyChanged = SetHotkeys(e.KeyCode, false);
        }

        private bool SetHotkeys(Keys _key, bool direction)
        {
            string key = _key.ToString().ToLower();
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
            mouseDownPosition = new Click(p.X, p.Y, button: button, shift: isShiftDown, ctrl: isCtrlDown, alt: isAltDown);
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            ButtonType button = GetButton(e.Button);
            Point p = Cursor.Point;
            Click end = new Click(p.X, p.Y, button: button, shift: isShiftDown, ctrl: isCtrlDown, alt: isAltDown);

            Click start = mouseDownPosition as Click;
            if (start != null && start.Button == end.Button && end.Distance(start) > Instructions.Hover.MAX_UNCERTAINTY)
            {
                //TODO Drag ad or increment
                AddOrIncrement(new Drag(start._x, start._y, end._x, end._y, button: button, shift: isShiftDown, ctrl: isCtrlDown, alt: isAltDown));
            }
            else
            {
                AddOrIncrement(start);
            }
        }

        private void MouseWheel(object sender, MouseEventArgs e)
        {
            // TODO Wheel
            //Instruction instruction = new Instruction(e.Delta, e.X, e.Y, isShiftDown, isCtrlDown, isAltDown);
            //AddOrIncrement(instruction);
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

        private bool AddOrIncrement(Instructions.Instruction instruction)
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
                // TODO Wheel increment
                //if (instruction.Type == InstructionType.WHEEL)
                //{
                //    current.Wheel += instruction.Wheel;
                //}
                //else
                {
                    current.Repetitions.Value++;
                }
            }
            else
            {
                if (WithDelay)
                {
                    current.Delay.Value = delay;
                }
                window.AddInstruction(instruction);
                current = instruction;
            }

            return true;
        }


        #endregion
    }
}