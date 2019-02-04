using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AutoClicker
{
    public static class MouseSimulator
    {
        #region Win Stuff
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public SendInputEventType type;
            public MouseKeybdhardwareInputUnion mkhi;
        }
        [StructLayout(LayoutKind.Explicit)]
        struct MouseKeybdhardwareInputUnion
        {
            [FieldOffset(0)]
            public MouseInputData mi;

            [FieldOffset(0)]
            public KEYBDINPUT ki;

            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }
        struct MouseInputData
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public MouseEventFlags dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        [Flags]
        enum MouseEventFlags : uint
        {
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }
        enum SendInputEventType : int
        {
            InputMouse,
            InputKeyboard,
            InputHardware
        }
        #endregion

        public static void MouseDown(Point position, int button)
        {
            MoveMouse(position);

            INPUT mouseDownInput = new INPUT();
            mouseDownInput.type = SendInputEventType.InputMouse;

            MouseEventFlags flags;
            switch (button)
            {
                case 1:
                    flags = MouseEventFlags.MOUSEEVENTF_RIGHTDOWN;
                    break;
                case 2:
                    flags = MouseEventFlags.MOUSEEVENTF_MIDDLEDOWN;
                    break;
                default:
                    flags = MouseEventFlags.MOUSEEVENTF_LEFTDOWN;
                    break;
            }

            mouseDownInput.mkhi.mi.dwFlags = flags;
            SendInput(1, ref mouseDownInput, Marshal.SizeOf(new INPUT()));
        }

        public static void MouseUp(Point position, int button)
        {
            MoveMouse(position);

            INPUT mouseDownInput = new INPUT();
            mouseDownInput.type = SendInputEventType.InputMouse;

            MouseEventFlags flags;
            switch (button)
            {
                case 1:
                    flags = MouseEventFlags.MOUSEEVENTF_RIGHTUP;
                    break;
                case 2:
                    flags = MouseEventFlags.MOUSEEVENTF_MIDDLEUP;
                    break;
                default:
                    flags = MouseEventFlags.MOUSEEVENTF_LEFTUP;
                    break;
            }

            mouseDownInput.mkhi.mi.dwFlags = flags;
            SendInput(1, ref mouseDownInput, Marshal.SizeOf(new INPUT()));
        }

        public static void MouseClick(Point position, int button)
        {
            MouseDown(position, button);
            MouseUp(position, button);
        }

        public static void MoveMouse(Point position)
        {
            Cursor.Position = position;
            Thread.Sleep(50);
        }
    }
}
