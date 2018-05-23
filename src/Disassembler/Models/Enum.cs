using System;
using System.Collections.Generic;
using System.Text;

namespace PlantUmlLanguageService.Disassembler.Models
{
    public class Enum : Object
    {
        /// <summary>
        /// Gets or sets the diagram.
        /// </summary>
        /// <value>
        /// The diagram.
        /// </value>
        public string Diagram { get; set; }
        /// <summary>
        /// Gets or sets the diagram URL.
        /// </summary>
        /// <value>
        /// The diagram URL.
        /// </value>
        public string DiagramUrl { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public Dictionary<object, object> Values { get; set; } = new Dictionary<object, object>();

    }

}
