using System.Collections.Generic;

namespace Disassembler.Models
{
    public abstract class Member : Object
    {
        /// <summary>
        /// Gets or sets the inputs.
        /// </summary>
        /// <value>
        /// The inputs.
        /// </value>
        public List<Overload> Inputs { get; set; } = new List<Overload>();
        /// <summary>
        /// Gets or sets the modifier.
        /// </summary>
        /// <value>
        /// The modifier.
        /// </value>
        public ModifierType Modifier { get; set; }
    }
}
