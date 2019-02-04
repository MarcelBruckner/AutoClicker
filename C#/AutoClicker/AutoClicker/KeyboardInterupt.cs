using Gma.UserActivityMonitor;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace AutoClicker
{
    class KeyboardInterupt
    {
        public ToggleButton RecordButton { get; private set; }
        public ToggleButton PlayButton { get; private set; }

        public KeyboardInterupt(ToggleButton recordButton, ToggleButton playButton)
        {
            RecordButton = recordButton;
            PlayButton = playButton;
            HookManager.KeyDown += KeyBreak;
        }

        public void RemoveAllHooks()
        {
            HookManager.KeyDown -= KeyBreak;
            RecordButton.Focus();
            PlayButton.Focus();
        }

        private void KeyBreak(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;

            if (key == Keys.F7)
            {
                RecordButton.IsChecked = false;
                PlayButton.IsChecked = false;
                e.Handled = true;
            }
            RemoveAllHooks();
        }
    }
}