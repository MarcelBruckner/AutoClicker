
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoClicker
{
    public class TickCounter
    {
        private static TickCounter _instance;
        private int lastFrame;

        public static TickCounter Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new TickCounter();
                    _instance.lastFrame = Environment.TickCount;
                }
                return _instance;
            }
        }
        private TickCounter() { }

        public int DeltaTime
        {
            get
            {
                int current = Environment.TickCount;
                int delta = current - lastFrame;
                lastFrame = current;
                return delta;
            }
        }
    }
}