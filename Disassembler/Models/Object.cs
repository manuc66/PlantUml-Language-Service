
namespace Disassembler.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Object
    {

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the extra detail.
        /// </summary>
        /// <value>
        /// The extra detail.
        /// </value>
        public string ExtraDetail { get; set; }

        /// <summary>
        /// Gets or sets the documentation.
        /// </summary>
        /// <value>
        /// The documentation.
        /// </value>
        public string Documentation { get; set; }

    }
}
