using System;
using System.Collections.Generic;
using System.Text;

namespace PlantUmlLanguageService.Disassembler.Models
{
    public class Assembly :  Object
    {
        /// <summary>
        /// Gets or sets the root namespace.
        /// </summary>
        /// <value>
        /// The root namespace.
        /// </value>
        public string RootNamespace { get; set; }

        /// <summary>
        /// Gets or sets the namespaces.
        /// </summary>
        /// <value>
        /// The namespaces.
        /// </value>
        public List<Namespace> Namespaces { get; set; } = new List<Namespace>();

    }
}
