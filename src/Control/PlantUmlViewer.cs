using PlantUmlLanguageService.Services;
using System.Threading.Tasks;

namespace PlantUmlLanguageService.Control
{
    public class PlantUmlViewer : System.Windows.Forms.WebBrowser
    {
        public async Task RenderAsync()
        {
            DocumentText = TemplateService.GetLoadingTemplate();
            if (await DiagramReadyAsync()) {
                DocumentText = TemplateService.GetPreviewTemplate();
                Global.WriteOutput();
            }
        }

        private async Task<bool> DiagramReadyAsync()
        {
            return await DiagramService.ValidateDiagramAsync(Global.DiagramUrl);
        }
    }
}
