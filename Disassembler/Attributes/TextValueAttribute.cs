using System;

namespace Disassembler.Attributes
{
    public class TextValueAttribute : Attribute
    {
        /// <param name="StringValue">The string value.</param>
        public TextValueAttribute(string StringValue)
        {
            Value = StringValue;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

    }
}
