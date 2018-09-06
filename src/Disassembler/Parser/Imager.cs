using PlantUmlLanguageService.Disassembler.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace PlantUmlLanguageService.Disassembler.Parser
{
    public class Imager
    {
        /// <summary>
        /// Creates the images.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <param name="relativePath">The relative path.</param>
        /// <returns></returns>
        public static bool CreateImages(string input, string output, string relativePath = null)
        {
            try
            {
                var dllPath = input.Replace(".dll", ".xml");
                if (System.IO.File.Exists(dllPath))
                {
                    DocumentService.Xml = XDocument.Load(input.Replace(".dll", ".xml"));
                }
                Models.Assembly dll = null;
                Core.Root = output;
                if (relativePath == null)
                {
                    relativePath = Core.Root;
                }
                Core.DocumentRoot = relativePath;
                dll = Reflector.ParseAssembly(input);

                List<string> FilesList = new List<string>();

                dll.Namespaces = dll.Namespaces.OrderBy((x) => x.Name).ToList();
                dll.Namespaces.ForEach((n) =>
                {
                    Core.CurrentObject = n;
                    var subdirectory = n.Name.Replace(".", "\\");
                    var directory = $"{output}\\{subdirectory}";
                    System.IO.Directory.CreateDirectory(directory);
                    n.Classes.ForEach((c) =>
                    {
                        try
                        {
                            System.IO.File.Copy(ImageService.CopyTempImageFromUrl(c.DiagramUrl), $"{directory}\\{c.Name}.svg");
                        }
                        catch
                        {
                        }
                    });
                    n.Interfaces.ForEach((i) =>
                    {
                        try
                        {
                            System.IO.File.Copy(ImageService.CopyTempImageFromUrl(i.DiagramUrl), $"{directory}\\{i.Name}.svg");
                        }
                        catch
                        {
                        }
                    });
                    n.Enums.ForEach((e) =>
                    {
                        try
                        {
                            System.IO.File.Copy(ImageService.CopyTempImageFromUrl(e.DiagramUrl), $"{directory}\\{e.Name}.svg");
                        }
                        catch
                        {
                        }
                    });
                });

            }
            catch (Exception ex)
            {
                //TODO: do not obscure exception but handle gracefully
                Debug.WriteLine(ex.Message);
            }

            return true;

        }

    }

}
