using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker
{
    interface IKeyboardListener
    {
        void AddHooks();
        void RemoveHooks();
    }
}
