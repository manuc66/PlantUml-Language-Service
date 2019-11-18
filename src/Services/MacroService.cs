using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace PlantUmlLanguageService.Services
{
    internal class MacroService
    {
        public static string CheckImports(string diagramtext)
        {
            string[] diagramLines = diagramtext.Split('\n');
            Regex regex = new Regex(@"^(!(import|theme)\s)");

            foreach (string line in diagramLines)
            {
                Match match = regex.Match(line);
                if (match.Success)
                {
                    try
                    {
                        var resource = Regex.Replace(line, @"[^\d\w\s](import|theme)", string.Empty).Trim();
                        if (line.StartsWith("!import"))
                        {
                            diagramtext = diagramtext.Replace(line, GetMacro(resource));
                        }
                        if (line.StartsWith("!theme"))
                        {
                            diagramtext = diagramtext.Replace(line, GetTheme(resource));
                        }
                    }
                    catch
                    {
                        Global.Warnings.Add(line);
                        diagramtext = diagramtext.Replace(line, string.Empty);
                    }
                }
            }

            return diagramtext;
        }

        private static string GetMacro(string name)
        {
            return Global.GetResourceString("Macros", name, "macro");
        }

        private static string GetTheme(string name)
        {
            return Global.GetResourceString("Skins", name, "skin");
        }

    }

    public static class FileSystemExtensions
    {
        public static string IncludeFiles(this string[] lines)
        {
            var idx = 0;
            lines.ToList().ForEach(
                line =>
                {
                    if (line.StartsWith("!url"))
                    {
                        Global.BaseUrl = line.Replace("!url", "").Trim();
                        lines[idx] = string.Empty;
                    }
                    if (line.StartsWith("!include"))
                    {
                        try
                        {
                            var filename = Regex.Replace(line, @"^((!include)\s)", string.Empty).Trim();
                            Debug.WriteLine(filename);
                            if (filename.ToLower().StartsWith("-p"))
                            {
                                filename = Regex.Replace(filename, @"(?i)^((-p)\s)", string.Empty).Trim();
                            }
                            else
                            {
                                var paths = SolutionService.GetFileInfosForActiveSolution().Select(file => file.FullName).ToList();
                                var foundpath = paths.Find(path => System.IO.Path.GetFileName(path) == filename);
                                Debug.WriteLine(foundpath);
                                if (string.IsNullOrEmpty(foundpath) || foundpath == Global.CurrentFilePath)
                                {
                                    line = "!include {SELF}";
                                    filename = string.Empty;
                                }
                                else
                                {
                                    filename = foundpath;
                                }
                            }

                            var included = System.IO.File.ReadAllLines(filename).IncludeFiles();
                            var stripped = Regex.Replace(included, @"(@(start|end)(uml|salt|))", ControlChars.CrLf);
                            lines[idx] = stripped;
                        }
                        catch (System.Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            Global.Warnings.Add(line);
                            lines[idx] = string.Empty;
                        }
                    }
                    idx += 1;
                }
            );
            return string.Join(ControlChars.CrLf, lines.ToArray());
        }
    }

}
