namespace PlantUmlLanguageService.Control
{
    internal class Validator
    {
        public string Description = string.Empty;
        public string Error = string.Empty;
        public string ErrorLine = string.Empty;

        public string ErrorContext = "info";

        public Validator(string description, string error, string line)
        {
            Description = description;
            Error = error;
            if (!string.IsNullOrEmpty(error))
            {
                ErrorLine = $"@ Line(s) {line}";
                ErrorContext = "error";
            }
            else
            {
                ErrorContext = "info";
            }
        }

        public Validator(string description, string[] warnings)
        {
            Description = description;
            Error = "completed with warnings";
            ErrorLine = $"@ Where [{string.Join(",", warnings)}]";
            ErrorContext = "warning";
        }

        public Validator() { }

    }

}
