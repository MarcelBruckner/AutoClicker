using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoClicker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.Tests
{
    [TestClass()]
    public class VirtualKeyCodeExtensionTests
    {
        [TestMethod()]
        public void FromStringTest()
        {
            string input = "A";
            Assert.IsTrue(VirtualKeyCode.NONAME.IsUpper(input));
            Assert.AreEqual(VirtualKeyCode.VK_A, VirtualKeyCode.NONAME.FromString(input));

            input = "RETURN";
            Assert.IsFalse(VirtualKeyCode.NONAME.IsUpper(input));
            Assert.AreEqual(VirtualKeyCode.RETURN, VirtualKeyCode.NONAME.FromString(input));

            input = "VK_1";
            Assert.IsFalse(VirtualKeyCode.NONAME.IsUpper(input));
            Assert.AreEqual(VirtualKeyCode.VK_1, VirtualKeyCode.NONAME.FromString(input));

            input = "1";
            Assert.IsFalse(VirtualKeyCode.NONAME.IsUpper(input));
            Assert.AreEqual(VirtualKeyCode.VK_1, VirtualKeyCode.NONAME.FromString(input));

            Assert.AreEqual(VirtualKeyCode.NONAME.FromString("1"), VirtualKeyCode.NONAME.FromString("VK_1"));
        }
    }
}