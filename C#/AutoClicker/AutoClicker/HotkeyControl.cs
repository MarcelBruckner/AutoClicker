using Gma.UserActivityMonitor;
using System;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace AutoClicker
{
    public class HotkeyControl 
    {
        private Action onHotkey;
        private Keys key;

        public HotkeyControl(Keys key, Action onHotkey)
        {
            this.onHotkey = onHotkey;
            this.key = key;
            HookManager.KeyDown += OnHotkey;
        }

        private void OnHotkey(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;
            if (key == this.key)
            {
                onHotkey();
                e.Handled = true;
            }
        }
    }
}