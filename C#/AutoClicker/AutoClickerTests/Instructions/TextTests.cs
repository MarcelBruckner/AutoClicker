using AutoClicker.Instructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoClicker;

namespace Tests
{
    [TestClass()]
    public class TextTests
    {
        [TestMethod()]
        public void ConvertTextToKeysTest()
        {
            AutoClicker.Instructions.Instruction empty = new AutoClicker.Instructions.Instruction();
            Text text = new Text("Hallo Welt!", empty);
            List<KeyValuePair<VirtualKeyCode, bool>> expected = new List<KeyValuePair<VirtualKeyCode, bool>>
            {
                new KeyValuePair<VirtualKeyCode, bool>(VirtualKeyCode.VK_H, true),
                new KeyValuePair<VirtualKeyCode, bool>(VirtualKeyCode.VK_A, false),
                new KeyValuePair<VirtualKeyCode, bool>(VirtualKeyCode.VK_L, false),
                new KeyValuePair<VirtualKeyCode, bool>(VirtualKeyCode.VK_L, false),
                new KeyValuePair<VirtualKeyCode, bool>(VirtualKeyCode.VK_O, false),
                new KeyValuePair<VirtualKeyCode, bool>(VirtualKeyCode.SPACE, false),
                new KeyValuePair<VirtualKeyCode, bool>(VirtualKeyCode.VK_W, true),
                new KeyValuePair<VirtualKeyCode, bool>(VirtualKeyCode.VK_E, false),
                new KeyValuePair<VirtualKeyCode, bool>(VirtualKeyCode.VK_L, false),
                new KeyValuePair<VirtualKeyCode, bool>(VirtualKeyCode.VK_T, false)
            };
            List<KeyValuePair<VirtualKeyCode, bool>> parsed = text.Converted;

            for(int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(parsed[i], expected[i]);
            }
        }
    }
}