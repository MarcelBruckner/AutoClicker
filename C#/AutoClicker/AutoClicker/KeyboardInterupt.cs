using Gma.UserActivityMonitor;
using System;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace AutoClicker
{
    class KeyboardInterupt : IKeyboardListener
    {
        private Action onInterupt;

        public KeyboardInterupt(Action onInterupt)
        {
            this.onInterupt = onInterupt;           
        }

        private void KeyBreak(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;
            if (key == Keys.F7)
            {
                onInterupt();
                e.Handled = true;
            }
        }

        public void AddHooks()
        {
            HookManager.KeyDown += KeyBreak;
        }

        public void RemoveHooks()
        {
            HookManager.KeyDown -= KeyBreak;
        }
    }
}