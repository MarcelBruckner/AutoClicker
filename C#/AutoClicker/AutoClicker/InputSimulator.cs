using Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AutoClicker
{
    /// <summary>
    /// Class to simulate mouse and keyboard input.
    /// </summary>
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

        /// <summary>
        /// Drags the mouse.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="button">The button.</param>
        /// <param name="v">The v.</param>
        /// <param name="endX">The end x.</param>
        /// <param name="endY">The end y.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void MouseDrag(MovementType move, ButtonType button, System.Windows.Vector v, int endX, int endY, double speed, params VirtualKeyCode[] hotkeys)
        {
            MouseDrag(move, button, (int)v.X, (int)v.Y, endX, endY, speed, hotkeys);
        }

        /// <summary>
        /// Drags the mouse.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="button">The button.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="endX">The end x.</param>
        /// <param name="endY">The end y.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void MouseDrag(MovementType move, ButtonType button, int x, int y, int endX, int endY, double speed, params VirtualKeyCode[] hotkeys)
        {
            MouseDrag(move, (int)button, x, y, endX, endY, speed, hotkeys);
        }

        /// <summary>
        /// Drags the mouse.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="button">The button.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="endX">The end x.</param>
        /// <param name="endY">The end y.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void MouseDrag(MovementType move, int button, int x, int y, int endX, int endY, double speed, params VirtualKeyCode[] hotkeys)
        {
            MouseDown(move, button, x, y, speed, hotkeys);
            MouseUp(move, button, endX, endY, speed, hotkeys);
        }

        /// <summary>
        /// Moves the mouse to the position and sets the button and the hotkeys to the down state.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="button">The button.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void MouseDown(MovementType move, int button, int x, int y, double speed, params VirtualKeyCode[] hotkeys)
        {
            MouseMove(move, x, y, speed);
            MouseDown(button, hotkeys);
        }

        /// <summary>
        /// Sets the button and the hotkeys to the down state.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void MouseDown(int button, params VirtualKeyCode[] hotkeys)
        {
            KeyDown(hotkeys);
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

        /// <summary>
        /// Sets the button and the hotkeys to the up state.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void MouseUp(int button, params VirtualKeyCode[] hotkeys)
        {
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

        /// <summary>
        /// Moves the mouse to the position and sets the button and the hotkeys to the up state.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="button">The button.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void MouseUp(MovementType move, int button, int x, int y, double speed, params VirtualKeyCode[] hotkeys)
        {

            MouseMove(move, x, y, speed);

            MouseUp(button, hotkeys);
        }

        /// <summary>
        /// Clicks the specified mouse button at the given position.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="button">The button.</param>
        /// <param name="position">The position.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void MouseClick(MovementType move, ButtonType button, System.Windows.Vector position, double speed, params VirtualKeyCode[] hotkeys)
        {
            MouseClick(move, button, (int)position.X, (int)position.Y, speed, hotkeys);
        }

        /// <summary>
        /// Clicks the specified mouse button at the given position.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="button">The button.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void MouseClick(MovementType move, ButtonType button, int x, int y, double speed, params VirtualKeyCode[] hotkeys)
        {
            MouseClick(move, (int)button, x, y, speed, hotkeys);
        }

        /// <summary>
        /// Clicks the specified mouse button at the given position.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="button">The button.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void MouseClick(MovementType move, int button, int x, int y, double speed, params VirtualKeyCode[] hotkeys)
        {
            MouseDown(move, button, x, y, speed, hotkeys);
            MouseUp(move, button, x, y, speed, hotkeys);
        }

        /// <summary>
        /// Clicks the mouse at the current position.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void MouseClick(int button, params VirtualKeyCode[] hotkeys)
        {
            MouseDown(button, hotkeys);
            MouseUp(button, hotkeys);
        }

        /// <summary>
        /// Clicks the mouse at the current position.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void MouseClick(ButtonType button, params VirtualKeyCode[] hotkeys)
        {
            MouseClick((int)button, hotkeys);
        }

        /// <summary>
        /// Moves the mouse.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="end">The end.</param>
        /// <param name="speed">The speed.</param>
        public static void MouseMove(MovementType move, System.Windows.Vector end, double speed)
        {
            MouseMove(move, new Point((int)end.X, (int)end.Y), speed);
        }

        /// <summary>
        /// Moves the mouse.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="end">The end.</param>
        /// <param name="speed">The speed.</param>
        public static void MouseMove(MovementType move, Point end, double speed)
        {
            ICursorInterpolation interpolation;

            switch (move)
            {
                case MovementType.SINUS:
                    interpolation = new SinusInterpolation(end, speed);
                    break;
                case MovementType.SPRING:
                    interpolation = new MassSpringInterpolation(end, speed);
                    break;
                default:
                    interpolation = new JumpInterpolation(end, speed);
                    break;
            }


            interpolation.Interpolate();
        }

        /// <summary>
        /// Moves the mouse.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="speed">The speed.</param>
        public static void MouseMove(MovementType move, int x, int y, double speed)
        {
            MouseMove(move, new Point(x, y), speed);
        }

        /// <summary>
        /// Sets the hotkeys down.
        /// </summary>
        /// <param name="keys">The keys.</param>
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
    
        /// <summary>
        /// Sets the hotkeys up.
        /// </summary>
        /// <param name="keys">The keys.</param>
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
        
        /// <summary>
        /// Presses the key while the hotkeys are pressed.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void KeyPress(VirtualKeyCode key, params VirtualKeyCode[] hotkeys)
        {
            KeyDown(hotkeys);
            KeyDown(key);
            Thread.Sleep(55);
            KeyUp(key);
            KeyUp(hotkeys);
        }

        /// <summary>
        /// Inputs the specified text inputs.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void Text(List<KeyValuePair<VirtualKeyCode, bool>> inputs, params VirtualKeyCode[] hotkeys)
        {
            foreach (var input in inputs)
            {
                if (input.Value)
                {
                    KeyDown(VirtualKeyCode.SHIFT);
                }
                KeyPress(input.Key, hotkeys);
                if (input.Value)
                {
                    KeyUp(VirtualKeyCode.SHIFT);
                }
            }
        }


        /// <summary>
        /// Inputs the specified text inputs.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void Text(string inputs, params VirtualKeyCode[] hotkeys)
        {
            KeyDown(hotkeys);
            WindowsInput.InputSimulator simulator = new WindowsInput.InputSimulator();
            simulator.Keyboard.TextEntry(inputs);
            KeyUp(hotkeys);
        }

        /// <summary>
        /// Moves the mousewheel.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="v">The v.</param>
        /// <param name="delta">The delta.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void MouseWheel(MovementType move, System.Windows.Vector v, int delta, double speed, params VirtualKeyCode[] hotkeys)
        {
            MouseWheel(move, (int)v.X, (int)v.Y, delta, speed, hotkeys);
        }

        /// <summary>
        /// Moves the mousewheel.
        /// </summary>
        /// <param name="move">The move.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="delta">The delta.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="hotkeys">The hotkeys.</param>
        public static void MouseWheel(MovementType move, int x, int y, int delta, double speed, params VirtualKeyCode[] hotkeys)
        {
            MouseMove(move, x, y, speed);
            KeyDown(hotkeys);
            INPUT mouseDownInput = new INPUT
            {
                type = SendInputEventType.InputMouse
            };
            mouseDownInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_WHEEL;
            mouseDownInput.mkhi.mi.mouseData = (uint)delta;
            SendInput(1, ref mouseDownInput, Marshal.SizeOf(new INPUT()));
            KeyUp(hotkeys);
        }
    }
}