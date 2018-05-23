using PlantUmlLanguageService.Disassembler.Attributes;

namespace PlantUmlLanguageService.Disassembler.Models
{
    public enum RelationshipType
    {
        /// <summary>
        /// The inheritance
        /// </summary>
        [Arrow("-d-|>")]
        Inheritance = 0,
        /// <summary>
        /// The implementation
        /// </summary>
        [Arrow("--()")]
        Implementation = 1,
        /// <summary>
        /// The realization
        /// </summary>
        [Arrow("..|>")]
        Realization = 2,
        /// <summary>
        /// The aggregation
        /// </summary>
        [Arrow("o--")]
        Aggregation = 3,
        /// <summary>
        /// The composition
        /// </summary>
        [Arrow("*--")]
        Composition = 4,
        /// <summary>
        /// The dependency
        /// </summary>
        [Arrow("..>")]
        Dependency = 5,
        /// <summary>
        /// The association
        /// </summary>
        [Arrow("-->")]
        Association = 6
    }
}
