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
    class Recorder
    {
        private const int REPETITION_MAX_DELAY = 200;
        public bool IsRecording { get; set; }

        private System.Windows.Controls.RichTextBox target;

        private DateTime lastActionTime = DateTime.Now;

        private bool isAltDown = false;
        private bool isCtrlDown = false;
        private bool isShiftDown = false;

        private Instruction current = null;
        private MouseClick mouseDownPosition;
        private bool isRepeated = false;

        #region Init Deinit
        public Recorder(System.Windows.Controls.RichTextBox target)
        {
            this.target = target;
            Console.WriteLine("init");

            target.IsEnabled = false;
            HookManager.KeyDown += KeyDown;
            HookManager.KeyUp += KeyUp;

            HookManager.MouseDown += MouseDown;
            HookManager.MouseUp += MouseUp;
            IsRecording = true;
        }

        public void RemoveAllHooks()
        {
            Console.WriteLine("remove all");

            HookManager.KeyDown -= KeyDown;
            HookManager.KeyUp -= KeyUp;

            HookManager.MouseDown -= MouseDown;
            HookManager.MouseUp -= MouseUp;
            target.IsEnabled = true;
            IsRecording = false;
        }
        #endregion


        #region Key Events
        private void KeyDown(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;
            
            if(key == Keys.F7)
            {
                return;
            }
            
            bool hotkeyChanged = SetHotkeys(key + "", true);
            if (hotkeyChanged)
            {
                return;
            }

            Instruction instruction;
            int delay = GetDelay();

            bool isNumber = false;
            if(key.ToString().Length > 1)
            {
                isNumber = true;
            }

            if (isNumber || key == Keys.Enter || key == Keys.Tab || key == Keys.Back || key == Keys.Space || isAltDown || isShiftDown || isCtrlDown)
            {
                instruction = new SpecialKeyboard((VirtualKeyCode)(int)key, isShiftDown, isCtrlDown, isAltDown, delay,1);
            }
            else
            {
                instruction = new Keyboard(key + "", delay,1);
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
            mouseDownPosition = new MouseClick(button, p.X, p.Y, GetDelay(), 1);
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            int button = GetButton(e.Button);
            Point p = Cursor.Position;
            MouseClick end = new MouseClick(button, p.X, p.Y, GetDelay(), 1);

            MouseClick start = mouseDownPosition as MouseClick;
            if (start != null && start.Button == end.Button && end.Distance(start) > MouseClick.MAX_UNCERTAINTY)
            {
                AddOrIncrement(new MouseDrag(start.Button, start.Position.X, start.Position.Y, end.Position.X, end.Position.Y, GetDelay(), 1));
            }
            else {
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
            Console.WriteLine(delta.TotalMilliseconds);
            return (int) delta.TotalMilliseconds;
        }

        private bool AddOrIncrement(Instruction instruction)
        {
            if (instruction.Equals(current) && instruction.Delay < REPETITION_MAX_DELAY)
            {
                current.IncrementRepetition();
                UpdateLast(current.ToString());
                isRepeated = true;
            }
            else
            {
                if (isRepeated && current != null)
                {
                    isRepeated = false;
                    AddText(instruction.ToString());
                }
                else
                {
                        AddText(instruction.ToString());
                    }
                current = instruction;
            }

            return true;
        }

        private void AddText(string text)
        {
            target.Document.Blocks.Add(new Paragraph(new Run(text)));
            target.ScrollToEnd();
        }

        private void UpdateLast(string text)
        {
            target.Document.Blocks.Remove(target.Document.Blocks.LastBlock);
            AddText(text);
        }
        #endregion
    }
}
