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

        private static Random rnd = new Random(0);

        public static void MouseDrag(ButtonType button, int x, int y, int endX, int endY, params VirtualKeyCode[] hotkeys)
        {
            MouseDrag((int)button, x, y, endX, endY, hotkeys);
        }
        public static void MouseDrag(int button, int x, int y, int endX, int endY, params VirtualKeyCode[] hotkeys)
        {
            MouseDown(button, x, y, hotkeys);
            MouseUp(button, endX, endY, hotkeys);
        }

        public static void MouseDown(int button, int x, int y, params VirtualKeyCode[] hotkeys)
        {
            KeyDown(hotkeys);
            MoveMouse(x,y);

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

        public static void MouseUp(int button, int x, int y, params VirtualKeyCode[] hotkeys)
        {
            MoveMouse(x,y, 0);

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
        
        public static void MouseClick(ButtonType button, int x, int y,  params VirtualKeyCode[] hotkeys)
        {
            MouseClick((int)button, x, y, hotkeys);
        }
        public static void MouseClick(int button, int x, int y,  params VirtualKeyCode[] hotkeys)
        {
            MouseDown(button, x, y, hotkeys);
            MouseUp(button, x, y, hotkeys);
        }

        public static void MoveMouse(Point position, int duration = 50)
        {
            //HumanWindMouse(position, 100, 100, 100, 100, 1);
            //SinusMove(position);
            SpringMove(position);
        }

        public static void MoveMouse(int x, int y, int duration = 50)
        {
            MoveMouse(new Point(x, y), duration);
        }

        public static void JumpMouse(Point position)
        {
            Cursor.Position = position;
        }

        public static void JumpMouse(int x, int y)
        {
            JumpMouse(new Point(x, y));
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

        public static void MouseWheel(int x, int y, int delta)
        {
            MoveMouse(x, y);
            INPUT mouseDownInput = new INPUT
            {
                type = SendInputEventType.InputMouse
            };
            mouseDownInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_WHEEL;
            mouseDownInput.mkhi.mi.mouseData = (uint)delta;
            SendInput(1, ref mouseDownInput, Marshal.SizeOf(new INPUT()));
        }


        #region Own Human
        private static void SpringMove(Point endPoint, double stiffness = 0.5, int uncertainty = 5, double damping = 1.2, int step = 10)
        {
            var end = PointToVector(endPoint);
            
            var current = PointToVector(Cursor.Position);
            double length = (end - current).Length;

            double initialLength = Math.Abs(uncertainty);

            double dt = step / 50.0;
            var velocity = RandomVector(50);

            while (length > initialLength)
            {
                var F = -stiffness * (length) * ((current - end) / length);

                if(length > 200)
                {
                    velocity += RandomVector(25);
                }
                else if (length > 50) // Math.Min(50, initialLength * 5))
                {
                    velocity += RandomVector(10);
                }
                else
                {
                    velocity += RandomVector((int)(initialLength / 2.0));
                }

                current += dt * velocity;
                velocity += dt * (F - damping * velocity);

                Cursor.Position = VectorToPoint(current);
                Thread.Sleep(step);

                length = (end - current).Length;
            }
        }

        private static System.Windows.Vector RandomVector(int maxLength)
        {
            double angle = ConvertToRadians(rnd.Next(360));
            return new System.Windows.Vector(Math.Sin(angle), Math.Cos(angle)) * rnd.Next(0, maxLength);
        }

        private static void PrintVector(string message, System.Windows.Vector v)
        {
            Console.WriteLine(message + ": " + v.X + " - " + v.Y);
        }

        private static void SinusMove(Point end, int duration = 200)
        {
            int amplitude = rnd.Next(5, 50);
            if(rnd.NextDouble() < 0.5)
            {
                amplitude *= -1;
            }
            SinusMove(end, amplitude, duration);
        }

        private static void SinusMove(Point end, int amplitude, int duration = 200)
        {
            System.Windows.Vector origin = PointToVector(Cursor.Position);

            System.Windows.Vector a = new System.Windows.Vector();
            System.Windows.Vector b = PointToVector(end) - origin;

            double steps = Math.Min((a - b).Length, duration);

            if (steps > 0)
            {
                double angle = -System.Windows.Vector.AngleBetween(new System.Windows.Vector(1, 0), b);
                double backAngle = ConvertToRadians(-angle);

                double interval = Math.Cos(ConvertToRadians(angle)) * b.X + -Math.Sin(ConvertToRadians(angle)) * b.Y;

                List<double> coefficients = new List<double>() { 1 };

                int swings = 20;

                for (int i = 0; i < swings; i++)
                {
                    coefficients.Add((rnd.NextDouble() * 2 - 1) / swings);
                }

                for (double x = 0; x < 1.0; x += 1 / steps)
                {
                    double y = 0;
                    for (int i = 0; i < coefficients.Count; i++)
                    {
                        y += coefficients[i] * Math.Sin(x * (i + 2) * Math.PI);
                    }

                    y *= amplitude;
                    System.Windows.Vector current = new System.Windows.Vector(
                        Math.Cos(backAngle) * x * interval + -Math.Sin(backAngle) * y,
                        Math.Sin(backAngle) * x * interval + Math.Cos(backAngle) * y);

                    current += origin;
                    Cursor.Position = new Point((int)current.X, (int)current.Y);
                    Thread.Sleep((int)(duration / steps));
                }
            }

            Cursor.Position = (end);
        }

        private static System.Windows.Vector PointToVector(Point p)
        {
            return new System.Windows.Vector(p.X, p.Y);
        }

        private static Point VectorToPoint(System.Windows.Vector v)
        {
            return new Point((int)v.X, (int)v.Y);
        }

        public static double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
        #endregion


        #region Human Wind Mouse
        private static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public static double Hypot(double x, double y)
        {
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        private static void HumanWindMouse(Point end, double gravity, double wind, double targetArea, float mouseSpeed, float maxUncertainty = 3)
        {
            double veloX = 0,
                veloY = 0,
                windX = 0,
                windY = 0;

            //var msp = _mouseSpeed;
            var msp = mouseSpeed;
            var sqrt2 = Math.Sqrt(2);
            var sqrt3 = Math.Sqrt(3);
            var sqrt5 = Math.Sqrt(5);

            var tDist = Distance(Cursor.Position, end);
            var t = (uint)(Environment.TickCount + 10000);

            while (Distance(Cursor.Position, end) > Math.Max(3, maxUncertainty))
            {
                if (Environment.TickCount > t)
                {
                    break;
                }

                float dist = Distance(Cursor.Position, end);
                wind = Math.Min(wind, dist);

                if (dist < 1)
                {
                    dist = 1;
                }

                var d = (Math.Round(Math.Round(tDist) * 0.3) / 7);

                if (d > 25)
                {
                    d = 25;
                }

                if (d < 5)
                {
                    d = 5;
                }

                double rCnc = rnd.Next(6);

                if (rCnc == 1)
                {
                    d = 2;
                }

                double maxStep;

                if (d <= Math.Round(dist))
                {
                    maxStep = d;
                }
                else
                {
                    maxStep = Math.Round(dist);
                }

                if (dist >= targetArea)
                {
                    windX = windX / sqrt3 + (rnd.Next((int)(Math.Round(wind) * 2 + 1)) - wind) / sqrt5;
                    windY = windY / sqrt3 + (rnd.Next((int)(Math.Round(wind) * 2 + 1)) - wind) / sqrt5;
                }
                else
                {
                    windX = windX / sqrt2;
                    windY = windY / sqrt2;
                }

                veloX = veloX + windX;
                veloY = veloY + windY;
                Console.WriteLine("Velocity: " + veloX + " - " + veloY);
                veloX = veloX + gravity * (end.X - Cursor.Position.X) / dist;
                veloY = veloY + gravity * (end.Y - Cursor.Position.Y) / dist;


                if (Hypot(veloX, veloY) > maxStep)
                {
                    Console.WriteLine("in here");
                    var randomDist = maxStep / 2.0 + rnd.Next((int)(Math.Round(maxStep) / 2));
                    var veloMag = Math.Sqrt(veloX * veloX + veloY * veloY);
                    veloX = (veloX / veloMag) * randomDist;
                    veloY = (veloY / veloMag) * randomDist;
                }

                var lastX = Cursor.Position.X;
                var lastY = Cursor.Position.Y;
                Point current = new Point((int)(Cursor.Position.X + veloX), (int)(Cursor.Position.Y + veloY));

                if (lastX != current.X || (lastY != current.Y))
                {
                    Cursor.Position = current;
                }

                var w = (rnd.Next((int)(Math.Round(100 / msp))) * 6);

                if (w < 5)
                {
                    w = 5;
                }

                w = (int)Math.Round(w * 0.9);
                Console.WriteLine("Remaining distance: " + Distance(Cursor.Position, end));
                Thread.Sleep(w);
            }

            Console.WriteLine("cursor switched at: " + Cursor.Position.X + " - " + Cursor.Position.Y);
        }

        private static float Distance(Point a, Point b)
        {
            return Length(new Point(a.X - b.X, a.Y - b.Y));
        }

        private static float Length(Point a)
        {
            return (float)Math.Sqrt(Math.Pow(a.X, 2) + Math.Pow(a.Y, 2));
        }

        private static Point[] Normal(Point a, Point b)
        {
            Point r = new Point(b.X - a.X, b.Y - a.Y);
            r = Normalize(r);
            return new[] { new Point(-r.Y, r.X), new Point(r.Y - r.X) };
        }

        private static Point Normalize(Point r)
        {
            float length = Length(r);
            return new Point((int)Math.Ceiling(r.X / length), (int)Math.Ceiling(r.Y / length));
        }
        #endregion
    }
}
