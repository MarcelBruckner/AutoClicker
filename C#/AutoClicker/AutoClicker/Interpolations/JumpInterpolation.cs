using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AutoClicker
{
    public class JumpInterpolation : ICursorInterpolation
    {
        public JumpInterpolation(System.Drawing.Point end, double speed = 1, int targetRadius = 5) : base(end, speed) { }

        public override void Interpolate()
        {
            if (Finished)
            {
                return;
            }
            Vector toMove = End;
            Cursor.Vector = toMove;
        }
    }
}
