using Disassembler.Attributes;

namespace Disassembler.Models
{
    public enum ModifierType
    {
        /// <summary>
        /// The public
        /// </summary>
        [Modifier("+")]
        Public = 0,
        /// <summary>
        /// The private
        /// </summary>
        [Modifier("-")]
        Private = 1,
        /// <summary>
        /// The internal
        /// </summary>
        [Modifier("~")]
        Internal = 2,
        /// <summary>
        /// The protected
        /// </summary>
        [Modifier("#")]
        Protected = 3
    }

}
