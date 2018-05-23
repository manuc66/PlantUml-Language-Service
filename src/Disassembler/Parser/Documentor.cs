using PlantUmlLanguageService.Disassembler.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PlantUmlLanguageService.Disassembler.Parser
{
    /// <summary>
    /// 
    /// </summary>
    public class Documentor
    {
        /// <summary>
        /// The document link
        /// </summary>
        private const string DocumentLink = " + [{0}]({1})";

        /// <summary>
        /// Creates the documentation.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <param name="relativePath">The relative path.</param>
        /// <returns></returns>
        public static bool CreateDocumentation(string input, string output, string relativePath = null)
        {
            try
            {
                var dllPath = input.Replace(".dll", ".xml");
                if (System.IO.File.Exists(dllPath))
                {
                    DocumentService.Xml = XDocument.Load(input.Replace(".dll", ".xml"));
                }

                Models.Assembly dll = new Models.Assembly();
                Globals.Root = output;

                if (relativePath == null)
                {
                    relativePath = Globals.Root;
                }

                Globals.DocumentRoot = relativePath;

                if (Reflector.ParseAssemblyToUml(input, output))
                {
                    dll = Reflector.ParseAssembly(input);
                    Console.WriteLine("Reading " + dll.RootNamespace);

                    var PrimaryIndex = System.IO.Path.Combine(output, "reference_" + dll.RootNamespace.Replace(".dll", ".md"));
                    System.IO.File.WriteAllText(PrimaryIndex, "#" + dll.Name + Environment.NewLine);

                    List<string> FilesList = new List<string>();

                    dll.Namespaces = dll.Namespaces.OrderBy((x) => x.Name).ToList();
                    dll.Namespaces.ForEach((n) =>
                    {
                        Globals.CurrentObject = n;
                        var mdFile = System.IO.Path.Combine(output, n.Name + ".md");
                        System.IO.File.WriteAllText(mdFile, n.Documentation);
                        if (n.Name.StartsWith(dll.RootNamespace.Replace(".dll", "")))
                        {
                            FilesList.Add(string.Format(DocumentLink, n.Name, mdFile.Replace(output, relativePath)).Replace("\\", "/"));
                        }
                    });
                    System.IO.File.AppendAllText(PrimaryIndex, string.Join(Environment.NewLine, FilesList.Distinct().ToArray()));
                }
                else
                {
                    Globals.Message += Environment.NewLine + "Unable to Parse " + input;
                    return false;
                }

            }
            catch (Exception ex)
            {

                if (ex is NullReferenceException)
                {
                   Globals.Message += Environment.NewLine + "Documentor - Null Reference | " + string.Join(";",ex.Data.Values);
                   Globals.Message += Environment.NewLine + ex.Message;
                   //Globals.Message += Environment.NewLine + ex.InnerException.Message;
                }

                if (Globals.CurrentObject != null)
                {
                    Globals.Message += Environment.NewLine + "Documentor - " + Globals.CurrentObject.Name + " | " + ex.Message;
                }
                else
                {
                    Globals.Message += Environment.NewLine + "Documentor - Null Reference | " + ex.Message;
                }

                if (ex is System.Reflection.ReflectionTypeLoadException)
                {
                    var typeLoadException = ex as System.Reflection.ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions.ToList();
                    loaderExceptions.ForEach((e) => Globals.Message += Environment.NewLine + e.Message);

                }

                return false;

            }

            return true;

        }


    }

}
