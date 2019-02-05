using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoClicker.Instructions
{
    class MouseDrag : MouseClick
    {

        public MouseDrag() : this(0, 0, 0, 0, 0, 0, 1, false, false, false) { }

        public MouseDrag(int button, int x, int y, int endX, int endY, int delay, int repetitions, bool shift, bool ctrl, bool alt) : base(button, x, y, delay, repetitions, shift, ctrl, alt)
        {
            Type = Action.DRAG;
            EndX = endX;
            EndY = endY;
        }



        public override string ToString()
        {
            return base.ToString() + " " +
                Property.END_X + "=" + EndX + " " +
                Property.END_Y + "=" + EndY;
        }

        public override bool Equals(object obj)
        {
            var drag = obj as MouseDrag;
            return drag != null &&
                   base.Equals(obj) &&
                   EndX == drag.EndX &&
                   EndY == drag.EndY;
        }

        public override int GetHashCode()
        {
            var hashCode = 986550709;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EndX.GetHashCode();
            hashCode = hashCode * -1521134295 + EndY.GetHashCode();
            return hashCode;
        }
    }
}