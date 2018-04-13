using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace PlantUmlLanguageService.Intellisense
{
    // we have to export the class in order
    // for it to replace the VS default functionality
    [Export(typeof(IIntellisensePresenterProvider))]
    [ContentType(Constants.ContentType)]
    [Order(Before = "default")] //it will be picked up before the VS default
    [Name("Plantuml Intellisense Extension")]
    public class PlantUmlIntellisenseProvider :
        // should implement IIntellisensePresenterProvider
        IIntellisensePresenterProvider
    {
        public IIntellisensePresenter
            TryCreateIntellisensePresenter(IIntellisenseSession session)
        {

            // returning null will 
            // trigger the default Intellisense provider
            return null;
        }
    }
}
