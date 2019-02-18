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
        public JumpInterpolation(System.Drawing.Point end, double speed = 1, int targetRadius = 5) : base(end, speed, targetRadius) { }

        public override void Interpolate()
        {
            Vector toMove = End + RandomVector(TargetRadius);
            Cursor = toMove;
            Thread.Sleep((int)(20 * Speed));
            Cursor = toMove;
        }
    }
}
