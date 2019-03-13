using System;
using System.Diagnostics;
using System.Drawing;

namespace PlantUmlLanguageService.Services.Offline
{
    public class PlantUmlRunner
    {
        private readonly string _javaPath;
        private readonly string _jarPath;
        private readonly string _graphVizDot;

        public PlantUmlRunner(string javaPath, string jarPath, string graphVizDot)
        {
            _javaPath = javaPath;
            _jarPath = jarPath;
            _graphVizDot = graphVizDot;
        }

        public Image Create(string diagramText, string imageFormat)
        {

            if (string.IsNullOrEmpty(_javaPath))
            {
                Trace.TraceError("Could not find java.exe");
                return null;
            }

            if (string.IsNullOrEmpty(_jarPath))
            {
                Trace.TraceError("Could not find plantuml.jar");
                return null;
            }

            if (string.IsNullOrEmpty(_graphVizDot))
            {
                Trace.TraceError("Could not find Graphviz dot.exe");
                return null;
            }

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
                        RedirectStandardOutput = true,

                    },
                    EnableRaisingEvents = true
                };

                process.StartInfo.EnvironmentVariables["GRAPHVIZ_DOT"] = _graphVizDot;

                bool started = process.Start();

                if (!started)
                {
                    Trace.TraceError("Failed to invoke plant uml");
                    return null;
                }

                process.StandardInput.Write(diagramText);
                process.StandardInput.Close();

                return Image.FromStream(process.StandardOutput.BaseStream);

            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed to invoke plant uml: {0}", ex);
                return null;
            }
        }
    }
}