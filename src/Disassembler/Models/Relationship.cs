namespace PlantUmlLanguageService.Disassembler.Models
{
    public class Relationship
    {
        /// <summary>
        /// Gets or sets the principal object.
        /// </summary>
        /// <value>
        /// The principal object.
        /// </value>
        public Object PrincipalObject { get; set; }
        /// <summary>
        /// Gets or sets the ancillary object.
        /// </summary>
        /// <value>
        /// The ancillary object.
        /// </value>
        public Object AncillaryObject { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public RelationshipType Type { get; set; }

    }
}
