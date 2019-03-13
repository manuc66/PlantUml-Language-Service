using System.Threading.Tasks;
using PlantUmlLanguageService.Control;

namespace PlantUmlLanguageService.Services
{
    internal interface IDiagramService
    {
        /// <summary>
        /// Gets the image URL for source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="imageFormat">The image format.</param>
        /// <returns></returns>
        string GetImageUrlForSource(string source, string imageFormat);
        
        /// <summary>
        /// Validates the diagram asynchronous.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        Task<Validator> GetValidator(string uri);
    }
}