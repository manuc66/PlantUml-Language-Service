using System;
using System.Diagnostics;
using System.Drawing;

namespace PlantUmlLanguageService.Services.Offline
{
    public class PlantUmlRunner
    {
        private readonly string _javaPath;
        private readonly string _jarPath;

        public PlantUmlRunner(string javaPath, string jarPath)
        {
            _javaPath = javaPath;
            _jarPath = jarPath;
        }

        public Image Create(string diagramText, string imageFormat)
        {
            try
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = _javaPath,
                        Arguments = $"-Djava.awt.headless=true -splash:no -jar \"{_jarPath}\" -t{(imageFormat ?? "png").ToLower()} -pipe",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true
                    },
                    EnableRaisingEvents = true
                };

                bool started = process.Start();

                if (started)
                {
                    process.StandardInput.Write(diagramText);
                    process.StandardInput.Close();

                    return Image.FromStream(process.StandardOutput.BaseStream);
                }

                Debug.WriteLine("Failed to invoke plant uml");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to invoke plant uml");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }

            return null;
        }
    }
}