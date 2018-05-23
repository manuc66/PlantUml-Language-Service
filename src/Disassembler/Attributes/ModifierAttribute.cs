using System;

namespace PlantUmlLanguageService.Disassembler.Attributes
{
    public class ModifierAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModifierAttribute"/> class.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public ModifierAttribute(string symbol)
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
