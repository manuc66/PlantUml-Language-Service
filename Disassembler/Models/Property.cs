using System;
using System.Collections.Generic;
using System.Text;

namespace Disassembler.Models
{
    public class Property : Overload
    {
        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [read only]; otherwise, <c>false</c>.
        /// </value>
        public bool ReadOnly { get; set; }

    }
}
