using Disassembler.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Disassembler.Parser
{
    public class Imager
    {
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
                Globals.Root = output;
                if (relativePath == null)
                {
                    relativePath = Globals.Root;
                }
                Globals.DocumentRoot = relativePath;
                dll = Reflector.ParseAssembly(input);

                List<string> FilesList = new List<string>();

                dll.Namespaces = dll.Namespaces.OrderBy((x) => x.Name).ToList();
                dll.Namespaces.ForEach((n) =>
                {
                    Globals.CurrentObject = n;
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

            }

            return true;

        }

    }

}
