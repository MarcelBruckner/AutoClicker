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
    public class InstructionTests
    {
        [TestMethod()]
        public void ClickSameTest()
        {


        }

        [TestMethod()]
        public void DistanceTest()
        {
            Instruction a = new Instruction(30, 0, 0, false, false, false);
            Instruction b = new Instruction(30, 0, 10, false, false, false);
            Instruction c = new Instruction(30, 10, 0, false, false, false);
            Instruction d = new Instruction(ButtonType.LEFT, 10, 0, false, false, false);

            Assert.AreEqual(a.Distance(b), 10);
            Assert.AreEqual(a.Distance(c), 10);
            Assert.AreEqual(a.Distance(d), 10);
        }
    }
}