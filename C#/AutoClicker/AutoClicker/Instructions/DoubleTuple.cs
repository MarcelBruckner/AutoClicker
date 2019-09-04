using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.Instructions
{
    public class DoubleTuple
    {
        public double Value { get; set; }
        public double? Random { get; set; }

        public DoubleTuple(double value , double? delta = null)
        {
            Value = value;
            Random = delta;
        }

        public override bool Equals(object obj)
        {
            return obj is DoubleTuple tuple &&
                   Value == tuple.Value &&
                   EqualityComparer<double?>.Default.Equals(Random, tuple.Random);
        }

        public override int GetHashCode()
        {
            var hashCode = -914021701;
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Random);
            return hashCode;
        }

        public override string ToString()
        {
            string s = Value + "";
            if (Random is double d)
            {
                s += "/" + d;
            }
            return s;
        }
    }
}
