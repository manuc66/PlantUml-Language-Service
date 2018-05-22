using System;

namespace Disassembler.Attributes
{
    public class ArrowAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrowAttribute"/> class.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public ArrowAttribute(string symbol)
        {
            Value = symbol;
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
