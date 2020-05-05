using System;
using System.Collections.Generic;

namespace AutoClicker.Instructions
{
    public class DecimalTuple
    {
        /// <summary>
        /// The random
        /// </summary>
        private static readonly Random random = new Random();

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the random.
        /// </summary>
        /// <value>
        /// The random.
        /// </value>
        public double? Random { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalTuple"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="delta">The delta.</param>
        public DecimalTuple(double value , double? delta = null)
        {
            Value = value;
            Random = delta;
        }

        /// <summary>
        /// Incs the specified i.
        /// </summary>
        /// <param name="i">The i.</param>
        public void Inc(double i = 1)
        {
            Value += i;
        }

        /// <summary>
        /// Gets the specified fallback random.
        /// </summary>
        /// <param name="fallbackRandom">The fallback random.</param>
        /// <returns></returns>
        public double Get(double fallbackRandom=0.0)
        {
            return Value + random.NextDouble() * (Random ?? fallbackRandom);
        }

        /// <summary>
        /// Gets the specified fallback random.
        /// </summary>
        /// <param name="fallbackRandom">The fallback random.</param>
        /// <returns></returns>
        public int Get(int fallbackRandom=0)
        {
            return (int)(Value + random.NextDouble() * (Random ?? fallbackRandom));
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is DecimalTuple tuple &&
                   Value == tuple.Value &&
                   EqualityComparer<double?>.Default.Equals(Random, tuple.Random);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = -914021701;
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Random);
            return hashCode;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
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
