using Gma.UserActivityMonitor;
using System;
using System.Windows.Forms;
using AutoClicker.Instructions;
using System.Windows.Documents;
using System.Drawing;
using WindowsInput.Native;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

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

        private Instruction current = new Instruction(Instruction.InstructionType.LOOP);

        #region Init Deinit
        public Recorder(System.Windows.Controls.RichTextBox target)
        {
            this.target = target;

            target.IsEnabled = false;
            HookManager.KeyDown += KeyDown;
            HookManager.KeyUp += KeyUp;

            HookManager.MouseDown += MouseDown;
            HookManager.MouseUp += MouseUp;
            IsRecording = true;
        }

        public void RemoveAllHooks()
        {
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
            int[] standards = new[] { GetDelay(), 0, 1 };

            bool isNumber = false;
            if(key.ToString().Length > 1)
            {
                isNumber = true;
            }

            if (isNumber || key == Keys.Enter || key == Keys.Tab || key == Keys.Back || key == Keys.Space || isAltDown || isShiftDown || isCtrlDown)
            {
                instruction = new SpecialKeyboard(key + "", isShiftDown, isCtrlDown, isAltDown, standards);
            }
            else
            {
                instruction = new Keyboard(key + "", standards);
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
            current = new MouseClick(button, p.X, p.Y, GetDelay(), 0, 1);
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            int button = GetButton(e.Button);
            Point p = Cursor.Position;
            MouseClick end = new MouseClick(button, p.X, p.Y, GetDelay(), 0, 1);

            MouseClick start = current as MouseClick;
            if (start != null && start.Button == end.Button && end.Distance(start) > MouseClick.MAX_UNCERTAINTY)
            {
                AddOrIncrement(new MouseDrag(start.Button, start.Position.X, start.Position.Y, end.Position.X, end.Position.Y, start.DelayPrevious, end.DelayAfter, 1));
            }
            else {
                AddOrIncrement(start);
            }
            Console.WriteLine("MouseClick: " + e.Location);
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
            if (instruction.Equals(current) && instruction.DelayPrevious < REPETITION_MAX_DELAY)
            {
                RemoveLastLine();
                current.IncrementRepetition();
            }
            else
            {
                current = instruction;
            }

            AddText(current.ToString());

            return true;
        }

        private void RemoveLastLine()
        {
            string all = StringManager.RichTextBoxToString(target);
            var last = all.LastIndexOf("\n", all.Length - 5);

            if (last > 0)
            {
                target.Document.Blocks.Clear();
                AddText(all.Substring(0, last - 1));
            }
        }

        private void AddText(string text)
        {
            target.Document.Blocks.Add(new Paragraph(new Run(text)));
            target.ScrollToEnd();
        }
        #endregion
    }
}
