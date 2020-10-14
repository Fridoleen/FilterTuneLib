using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterTuneWPF
{
    /// <summary>
    /// Contains two strings
    /// </summary>
    public struct StringPair
    {
        public string Name { get; set; }
        public string Value { get; set; }
        /// <summary>
        /// Creates instance of StringPair
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_value"></param>

        public StringPair(string _name, string _value)
        {
            this.Name = _name;
            this.Value = _value;
        }

        /// <summary>
        /// Creates instance of StringPair from single line, if there's no space symbol - StringPair.Value is set to '*'
        /// </summary>
        /// <param name="text"></param>
        public StringPair(string text)
        {
            int spacePosition = text.IndexOf(' ');
            string _name, _value;
            if (spacePosition > 0)
            {
                _name = text.Substring(0, spacePosition);
                _value = text.Substring(spacePosition);
            }
            else
            {
                _name = text;
                _value = "*";
            }

            this.Name = _name;
            this.Value = _value;
        }
    }
}
