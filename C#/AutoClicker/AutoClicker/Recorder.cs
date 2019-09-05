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
using AutoClicker.Enums;

namespace AutoClicker
{
    /// <summary>
    /// Recorder for the autoclicker insteructions
    /// </summary>
    /// <seealso cref="AutoClicker.IKeyboardListener" />
    class Recorder : IKeyboardListener
    {
        /// <summary>
        /// The mouse repetition maximum delay
        /// </summary>
        private const int MOUSE_REPETITION_MAX_DELAY = 200;

        /// <summary>
        /// The keyboard repetition maximum delay
        /// </summary>
        private const int KEYBOARD_REPETITION_MAX_DELAY = 1000;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is recording.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is recording; otherwise, <c>false</c>.
        /// </value>
        public bool IsRecording { get; set; }

        /// <summary>
        /// The window
        /// </summary>
        private MainWindow window;

        /// <summary>
        /// The last action time
        /// </summary>
        private DateTime lastActionTime = DateTime.Now;

        /// <summary>
        /// The is alt down
        /// </summary>
        private bool isAltDown;

        /// <summary>
        /// The is control down
        /// </summary>
        private bool isCtrlDown;

        /// <summary>
        /// The is shift down
        /// </summary>
        private bool isShiftDown;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is recording with delay.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is recording with delay; otherwise, <c>false</c>.
        /// </value>
        public bool IsRecordingWithDelay { get; set; }

        //private int current = -1;

        /// <summary>
        /// The current instruction
        /// </summary>
        private Instructions.Instruction currentInstruction = null;

        /// <summary>
        /// The current run
        /// </summary>
        private Run currentRun = null;

        /// <summary>
        /// The mouse down position
        /// </summary>
        private Instructions.Instruction mouseDownPosition;

        #region Init Deinit        
        /// <summary>
        /// Initializes a new instance of the <see cref="Recorder"/> class.
        /// </summary>
        /// <param name="window">The window.</param>
        public Recorder(MainWindow window)
        {
            this.window = window;
        }

        /// <summary>
        /// Adds the hooks.
        /// </summary>
        public void AddHooks()
        {
            //instructions.(false);
            HookManager.KeyDown += OnKeyDown;
            HookManager.KeyUp += OnKeyUp;

            HookManager.MouseDown += OnMouseDown;
            HookManager.MouseUp += OnMouseUp;

            HookManager.MouseWheel += OnMouseWheel;

            IsRecording = true;
        }

        /// <summary>
        /// Removes the hooks.
        /// </summary>
        public void RemoveHooks()
        {
            HookManager.KeyDown -= OnKeyDown;
            HookManager.KeyUp -= OnKeyUp;

            HookManager.MouseDown -= OnMouseDown;
            HookManager.MouseUp -= OnMouseUp;

            HookManager.MouseWheel -= OnMouseWheel;

            IsRecording = false;
        }
        #endregion

        #region Key Events

        /// <summary>
        /// Called when key down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Instructions.Instruction newInstruction;

            Keys key = e.KeyCode;
            Console.WriteLine(key);
            if (key == MainWindow.RECORD_HOTKEY || key == MainWindow.PLAY_HOTKEY)
            {
                return;
            }

            string converted = WindowsKeyExtension.ToLiteral(WindowsKeyExtension.KeyCodeToUnicode(key));

            if (isCtrlDown || isAltDown || converted.Length <= 0 || 
                !((converted[0] >= 'A' && converted[0] <= 'Z') ||
                (converted[0] >= 'a' && converted[0] <= 'z') ||
                (converted[0] >= '0' && converted[0] <= '9' && !isShiftDown)))
            {
                VirtualKeyCode virtualKeyCode = (VirtualKeyCode)(int)key;
                newInstruction = new Keystroke(virtualKeyCode, shift: isShiftDown, ctrl: isCtrlDown, alt: isAltDown);
            }
            else
            {
                if (isShiftDown)
                {
                    converted = converted.ToUpperInvariant();
                }
                newInstruction = new Text(converted, ctrl: isCtrlDown, alt: isAltDown);
            }

            bool hotkeyChanged = SetHotkeys(key, true);
            if (hotkeyChanged)
            {
                return;
            }

            AddOrIncrement(newInstruction);
        }

        /// <summary>
        /// Called when key up.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            bool hotkeyChanged = SetHotkeys(e.KeyCode, false);
        }

        /// <summary>
        /// Sets the hotkeys.
        /// </summary>
        /// <param name="_key">The key.</param>
        /// <param name="direction">if set to <c>true</c> [direction].</param>
        /// <returns></returns>
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

        /// <summary>
        /// Called when mouse down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            ButtonType button = GetButton(e.Button);
            Point p = Cursor.Point;
            mouseDownPosition = new Click(p.X, p.Y, button: button, shift: isShiftDown, ctrl: isCtrlDown, alt: isAltDown);
        }

        /// <summary>
        /// Called when mouse up.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            ButtonType button = GetButton(e.Button);
            Point p = Cursor.Point;
            Click end = new Click(p.X, p.Y, button: button, shift: isShiftDown, ctrl: isCtrlDown, alt: isAltDown);

            Click start = mouseDownPosition as Click;
            if (start != null && start.Button == end.Button && end.Distance(start) > Hover.MAX_UNCERTAINTY)
            {
                AddOrIncrement(new Drag(start.X, start.Y, end.X, end.Y, button: button, shift: isShiftDown, ctrl: isCtrlDown, alt: isAltDown));
            }
            else
            {
                AddOrIncrement(start);
            }
        }

        /// <summary>
        /// Called when mouse wheel.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            // TODO Wheel
            //Instruction instruction = new Instruction(e.Delta, e.X, e.Y, isShiftDown, isCtrlDown, isAltDown);
            //AddOrIncrement(instruction);
        }

        /// <summary>
        /// Gets the button.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets the delay.
        /// </summary>
        /// <returns></returns>
        public int GetDelay()
        {
            DateTime now = DateTime.Now;
            TimeSpan delta = now - lastActionTime;
            lastActionTime = now;
            return (int)delta.TotalMilliseconds;
        }

        /// <summary>
        /// Adds the instruction or increments the repetitions.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <returns></returns>
        private bool AddOrIncrement(Instructions.Instruction instruction)
        {
            int delay = GetDelay();

            if (currentInstruction == null || currentRun == null)
            {
                currentInstruction = instruction;
            }
            else if (instruction.Resembles(currentInstruction) &&
                (instruction is Hover && delay < MOUSE_REPETITION_MAX_DELAY ||
                instruction is Keystroke && delay < KEYBOARD_REPETITION_MAX_DELAY))
            {
                currentInstruction.Repetitions.Inc();
            }
            else if (instruction.Resembles(currentInstruction) &&
                instruction is Text text && delay < KEYBOARD_REPETITION_MAX_DELAY)
            {
                ((Text)currentInstruction).Append(text.Input);
            }

            else
            {
                if (IsRecordingWithDelay)
                {
                    currentInstruction.Delay.Value = delay;
                }
                currentInstruction = instruction;
                currentRun = null;
            }

            currentRun = window.AddInstruction(currentInstruction, currentRun);

            return true;
        }
        #endregion
    }
}