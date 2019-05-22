using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.Instructions
{
    public class IntTuple
    {
        public int Value { get; set; }
        public int? Random { get; set; }

        public IntTuple(int value, int? delta = null)
        {
            Value = value;
            Random = delta;
        }

        public override bool Equals(object obj)
        {
            return obj is IntTuple tuple &&
                   Value == tuple.Value &&
                   EqualityComparer<int?>.Default.Equals(Random, tuple.Random);
        }

        public override int GetHashCode()
        {
            var hashCode = -914021701;
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(Random);
            return hashCode;
        }

        public override string ToString()
        {
            string s = Value + "";
            if(Random is int d)
            {
                s += "/" + d;
            }
            return s;
        }
    }
}
