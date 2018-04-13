namespace PlantUmlLanguageService.Services
{
    internal class TemplateService
    {
        /// <summary>
        /// Gets the preview template.
        /// </summary>
        /// <returns></returns>
        public static string GetPreviewTemplate()
        {
            string hiddenDetail = string.Empty;
            if (Global.Validator.ErrorContext == "error" || Global.Validator.ErrorContext == "none") { hiddenDetail = " hidden"; }
            return string.Format(
                GetResourceTemplate("preview"),
                Global.DiagramUrl,
                Global.DiagramUrl.Replace("/svg/", "/png/"),
                GetValidationTemplate(),
                GetResourceTemplate("preview", "css"),
                Global.Validator.ErrorContext,
                hiddenDetail

            );
        }

        /// <summary>
        /// Gets the loading template.
        /// </summary>
        /// <returns></returns>
        public static string GetLoadingTemplate()
        {
            return GetResourceTemplate("loading");
        }

        public static string GetValidationTemplate()
        {
            return string.Format(
                GetResourceTemplate("feedback"),
                Global.Validator.Description,
                Global.Validator.Error,
                Global.Validator.ErrorLine,
                Global.Validator.ErrorContext
            );
        }

        private static string GetResourceTemplate(string name, string extension = "html")
        {
            return Global.GetResourceString("Templates", name, extension);
        }

    }
}
