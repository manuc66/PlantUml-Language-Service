using System;
using System.Collections.Generic;
using System.Text;

namespace Disassembler.Models
{
    public class Namespace : Object
    {
        /// <summary>
        /// Gets or sets the classes.
        /// </summary>
        /// <value>
        /// The classes.
        /// </value>
        public List<Class> Classes { get; set; } = new List<Class>();

        /// <summary>
        /// Gets or sets the enums.
        /// </summary>
        /// <value>
        /// The enums.
        /// </value>
        public List<Enum> Enums { get; set; } = new List<Enum>();

        /// <summary>
        /// Gets or sets the interfaces.
        /// </summary>
        /// <value>
        /// The interfaces.
        /// </value>
        public List<Interface> Interfaces { get; set; } = new List<Interface>();

        /// <summary>
        /// Gets or sets the topography.
        /// </summary>
        /// <value>
        /// The topography.
        /// </value>
        public string Topography { get; set; }

        /// <summary>
        /// Gets or sets the topography URL.
        /// </summary>
        /// <value>
        /// The topography URL.
        /// </value>
        public string TopographyUrl { get; set; }

    }

}
