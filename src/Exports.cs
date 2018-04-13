using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace PlantUmlLanguageService
{

    public static class ExportDefinitions
    {

        //[Export]
        //[DisplayName("Plant Uml")]
        //[Name(Constants.ContentType)]
        //[BaseDefinition("csharp")]
        //internal static ContentTypeDefinition PlantUmlContentType;

        [Export]
        [FileExtension(Constants.PlantUmlFile)]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition PlantUmlExtensionDefinition;

        [Export]
        [FileExtension(Constants.PumlFile)]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition PUmlExtensionDefinition;

        [Export]
        [FileExtension(Constants.PlantFile)]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition PlantExtensionDefinition;

        [Export]
        [FileExtension(Constants.PuFile)]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition PuExtensionDefinition;

        [Export]
        [FileExtension(Constants.IumlFile)]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition IumlExtensionDefinition;

        [Export]
        [FileExtension(Constants.UmlFile)]
        [ContentType(Constants.ContentType)]
        internal static FileExtensionToContentTypeDefinition UmlExtensionDefinition;

    }

}
