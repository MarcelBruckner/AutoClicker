using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoClicker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;

namespace AutoClicker.Tests
{
    [TestClass()]
    public class InputSimulatorTests
    {
        [TestMethod()]
        public void SinusMoveTest()
        {
            Cursor.Position = new Point(100, 100);
            InputSimulator.SinusMove(new Point(101, 101));
        }
    }
}