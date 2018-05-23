using System;
using System.Collections.Generic;
using System.Text;

namespace PlantUmlLanguageService.Disassembler.Models
{
    public class Interface : Object
    {
        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public List<Property> Properties { get; set; } = new List<Property>();
        /// <summary>
        /// Gets or sets the functions.
        /// </summary>
        /// <value>
        /// The functions.
        /// </value>
        public List<Function> Functions { get; set; } = new List<Function>();
        /// <summary>
        /// Gets or sets the voids.
        /// </summary>
        /// <value>
        /// The voids.
        /// </value>
        public List<Void> Voids { get; set; } = new List<Void>();

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
        /// Gets or sets the relationships.
        /// </summary>
        /// <value>
        /// The relationships.
        /// </value>
        public List<Relationship> Relationships { get; set; }

    }

}
