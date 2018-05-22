using System;
using System.Collections.Generic;
using System.Text;

namespace Disassembler.Models
{
    public class Class : Interface
    {
        /// <value>
        ///   <c>true</c> if abstract; otherwise, <c>false</c>.
        /// </value>
        public bool Abstract { get; set; }

        /// <value>
        ///   <c>true</c> if static; otherwise, <c>false</c>.
        /// </value>
        public bool Static { get; set; }


        /// <value>
        ///   <c>true</c> if module; otherwise, <c>false</c>.
        /// </value>
        public bool Module { get; set; }

        /// <summary>
        /// Gets or sets the implements.
        /// </summary>
        /// <value>
        /// The implements.
        /// </value>
        public Type Implements { get; set; }
        /// <summary>
        /// Gets or sets the inherits.
        /// </summary>
        /// <value>
        /// The inherits.
        /// </value>
        public Type Inherits { get; set; }

    }
}
