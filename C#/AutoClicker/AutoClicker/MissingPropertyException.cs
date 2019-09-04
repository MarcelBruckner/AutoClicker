using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker
{
    public class MissingPropertyException : Exception
    {
        Dictionary<string, string> missingProperties = new Dictionary<string, string>();

        public MissingPropertyException()
        {
        }

        public MissingPropertyException(string key, string value)
        {
            AddMissingProperty(key, value);
        }

        public void AddMissingProperty(string key, string value)
        {
            missingProperties.Add(key, value);
        }

        public override string Message
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                foreach (KeyValuePair<string, string> pair in missingProperties)
                {
                    builder.Append(pair.Key + "=" + pair.Value + " ");
                }

                return builder.ToString();
            }
        }
    }
}
