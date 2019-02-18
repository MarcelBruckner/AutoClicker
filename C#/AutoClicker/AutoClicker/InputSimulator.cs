using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AutoClicker
{
    public static class InputSimulator
    {
        #region Win Stuff
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        //[DllImport("user32.dll")]
        //static extern int MapVirtualKey(uint uCode, uint uMapType);

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
        [Flags]
        enum KeyboardEventFlags : uint
        {
            KEYEVENTF_EXTENDEDKEY = 0x0001,
            KEYEVENTF_KEYUP = 0x0002,
            KEYEVENTF_UNICODE = 0x0004,
            KEYEVENTF_SCANCODE = 0x0008
        }
        [Flags]
        enum HardwareEventFlags : uint
        {
            KEYEVENTF_EXTENDEDKEY = 0x0001,
            KEYEVENTF_KEYUP = 0x0002,
            KEYEVENTF_UNICODE = 0x0004,
            KEYEVENTF_SCANCODE = 0x0008
        }
        enum SendInputEventType : int
        {
            InputMouse,
            InputKeyboard,
            InputHardware
        }
        #endregion

        public static void MouseDrag(MovementType move, ButtonType button, int x, int y, int endX, int endY, params VirtualKeyCode[] hotkeys)
        {
            MouseDrag(move, (int)button, x, y, endX, endY, hotkeys);
        }
        public static void MouseDrag(MovementType move, int button, int x, int y, int endX, int endY, params VirtualKeyCode[] hotkeys)
        {
            MouseDown(move, button, x, y, hotkeys);
            MouseUp(move, button, endX, endY, hotkeys);
        }

        public static void MouseDown(MovementType move, int button, int x, int y, params VirtualKeyCode[] hotkeys)
        {
            KeyDown(hotkeys);
            MoveMouse(move, x,y);

            INPUT mouseDownInput = new INPUT
            {
                type = SendInputEventType.InputMouse
            };

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

        public static void MouseUp(MovementType move, int button, int x, int y, params VirtualKeyCode[] hotkeys)
        {
            MoveMouse(move, x,y, 0);

            INPUT mouseDownInput = new INPUT
            {
                type = SendInputEventType.InputMouse
            };

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
            KeyUp(hotkeys);
        }
        
        public static void MouseClick(MovementType move, ButtonType button, int x, int y,  params VirtualKeyCode[] hotkeys)
        {
            MouseClick(move, (int)button, x, y, hotkeys);
        }
        public static void MouseClick(MovementType move, int button, int x, int y,  params VirtualKeyCode[] hotkeys)
        {
            MouseDown(move, button, x, y, hotkeys);
            MouseUp(move, button, x, y, hotkeys);
        }

        public static void MoveMouse(MovementType move, Point end, int speed = 1, int targetRadius = 5)
        {
            ICursorInterpolation interpolation;

            switch (move)
            {
                case MovementType.SINUS:
                    interpolation = new SinusInterpolation(end, speed, 100);
                    break;
                case MovementType.SPRING:
                    interpolation = new MassSpringInterpolation(end, speed, targetRadius);
                    break;
                default:
                    interpolation = new JumpInterpolation(end, speed);
                    break;
            }


            interpolation.Interpolate();
        }

        public static void MoveMouse(MovementType move, int x, int y, int speed = 1, int targetRadius = 5)
        {
            MoveMouse(move, new Point(x, y), speed, targetRadius);
        }

        public static void KeyDown(params VirtualKeyCode[] keys)
        {
            foreach (VirtualKeyCode key in keys)
            {
                INPUT keyDownInput = new INPUT
                {
                    type = SendInputEventType.InputKeyboard
                };

                keyDownInput.mkhi.ki.wVk = (ushort)key;
                //keyDownInput.mkhi.ki.wVk = 0;
                //keyDownInput.mkhi.ki.dwFlags = (uint)KeyboardEventFlags.KEYEVENTF_SCANCODE;

                //ushort mapped = (ushort)MapVirtualKey((uint)key, 0);
                //keyDownInput.mkhi.ki.wScan = mapped;

                SendInput(1, ref keyDownInput, Marshal.SizeOf(new INPUT()));
            }
        }

        public static void KeyUp(params VirtualKeyCode[] keys)
        {
            foreach (VirtualKeyCode key in keys)
            {
                INPUT keyUpInput = new INPUT
                {
                    type = SendInputEventType.InputKeyboard
                };

                keyUpInput.mkhi.ki.wVk = (ushort)key;
                keyUpInput.mkhi.ki.dwFlags = (uint)KeyboardEventFlags.KEYEVENTF_KEYUP;
                //keyUpInput.mkhi.ki.wVk = 0;
                //keyUpInput.mkhi.ki.dwFlags = (uint)KeyboardEventFlags.KEYEVENTF_SCANCODE | (uint)KeyboardEventFlags.KEYEVENTF_KEYUP;

                //ushort mapped = (ushort)MapVirtualKey((uint)key, 0);
                //keyUpInput.mkhi.ki.wScan = mapped;

                SendInput(1, ref keyUpInput, Marshal.SizeOf(new INPUT()));
            }
        }

        public static void KeyPress(VirtualKeyCode key)
        {
            KeyPress(key);
        }

        public static void KeyPress(VirtualKeyCode key, params VirtualKeyCode[] hotkeys)
        {
            KeyDown(hotkeys);
            KeyDown(key);
            KeyUp(key);
            KeyUp(hotkeys);
        }

        public static void MouseWheel(MovementType move, int x, int y, int delta)
        {
            MoveMouse(move, x, y);
            INPUT mouseDownInput = new INPUT
            {
                type = SendInputEventType.InputMouse
            };
            mouseDownInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_WHEEL;
            mouseDownInput.mkhi.mi.mouseData = (uint)delta;
            SendInput(1, ref mouseDownInput, Marshal.SizeOf(new INPUT()));
        }
    }
}
