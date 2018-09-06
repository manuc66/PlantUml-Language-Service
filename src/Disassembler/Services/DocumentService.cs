using PlantUmlLanguageService.Disassembler.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PlantUmlLanguageService.Disassembler.Services
{
    public class DocumentService
    {

        public static XDocument Xml { get; set; }

        private const string Title = "#{0}";

        private const string ClassLink = (" + [{0}](#{1})" + "\r\n");

        private static string GetXmlDocumentation(string Namespace, string @class, string member, List<Models.Overload> inputs)
        {
            return Xml.XPathEvaluate(string.Format("string(/doc/members/member[@name='{0}']/summary)", GetMemberElementName(Namespace, @class, member, inputs))).ToString().Trim();
        }

        private static string GetXmlDocumentation(string Namespace, string @class)
        {
            return Xml.XPathEvaluate(string.Format("string(/doc/members/member[@name='{0}']/summary)", GetTypeElementName(Namespace, @class))).ToString().Trim();
        }

        private static string GetXmlDocumentation(string Namespace, string @class, string member)
        {
            return Xml.XPathEvaluate(string.Format("string(/doc/members/member[@name='{0}']/summary)", GetPropertyElementName(Namespace, @class, member))).ToString().Trim();
        }

        private static string GetMemberElementName(string Namespace, string @class, string method, List<Models.Overload> inputs)
        {
            string @params = string.Join(",", inputs.Select((x) => x.Type.FullName).ToArray());
            string FullPathFormat = string.Format("{0}.{1}.{2}({3})", Namespace, @class, method, @params);
            return string.Format("M:{0}", FullPathFormat);
        }

        private static string GetTypeElementName(string Namespace, string @class)
        {
            string FullPathFormat = string.Format("{0}.{1}", Namespace, @class);
            return string.Format("T:{0}", FullPathFormat);
        }

        private static string GetPropertyElementName(string Namespace, string @class, string method)
        {
            string FullPathFormat = string.Format("{0}.{1}.{2}", Namespace, @class, method);
            return string.Format("P:{0}", FullPathFormat);
        }

        internal static string GetClassDocumentation(string @namespace, Models.Class @class)
        {
            string docDetail = string.Empty;
            var classSummary = GetXmlDocumentation(@namespace, @class.Name);
            Console.WriteLine($"Loading documentation {@class.Name}");
            if (classSummary.Length > 0)
            {
                docDetail += (" + " + classSummary + Environment.NewLine);
            }
            if (@class.Properties.Count > 0)
            {
                docDetail += ("###Properties" + Environment.NewLine);
            }
            @class.Properties.ForEach((p) =>
            {
                Core.CurrentObject = p;
                docDetail += p.Documentation;
            });
            if (@class.Functions.Count > 0)
            {
                docDetail += ("###Functions" + Environment.NewLine);
            }
            @class.Functions.ForEach((f) =>
            {
                Core.CurrentObject = f;
                docDetail += f.Documentation;
            });
            if (@class.Voids.Count > 0)
            {
                docDetail += ("###Actions" + Environment.NewLine);
            }
            @class.Voids.ForEach((v) =>
            {
                Core.CurrentObject = v;
                docDetail += v.Documentation;
            });
            //if (@class.ExtraDetail.Length > 0)
            //{
            //    docDetail += ("###Design" + Environment.NewLine);
            //    docDetail += string.Format(Templates.DiagramTemplate, "Design", @class.ExtraDetail);
            //}

            return string.Format(Template(Templates.ClassTemplate, @namespace, @class.Name), @class.Name, docDetail, string.Format(Templates.DiagramTemplate, @class.Name, @class.DiagramUrl), string.Format(Title, @namespace.ToLower()));

        }

        internal static string GetInterfaceDocumentation(string @namespace, Models.Interface @interface)
        {

            string docDetail = string.Empty;
            if (@interface.Properties.Count > 0)
            {
                docDetail += ("###Properties" + Environment.NewLine);
            }
            @interface.Properties.ForEach((p) =>
            {
                docDetail += p.Documentation;
            });
            if (@interface.Functions.Count > 0)
            {
                docDetail += ("###Functions" + Environment.NewLine);
            }
            @interface.Functions.ForEach((f) =>
            {
                docDetail += f.Documentation;
            });
            if (@interface.Voids.Count > 0)
            {
                docDetail += ("###Actions" + Environment.NewLine);
            }
            @interface.Voids.ForEach((v) =>
            {
                docDetail += v.Documentation;
            });

            return string.Format(Template(Templates.ClassTemplate, @namespace, @interface.Name), @interface.Name, docDetail, string.Format(Templates.DiagramTemplate, @interface.Name, @interface.DiagramUrl), string.Format(Title, @namespace.ToLower()));

        }

        internal static string GetFunctionDocumentation(string @namespace, string @class, Models.Function function)
        {
            string inputs = "";
            string returnable = function.Return;
            function.Inputs.ForEach((x) =>
            {
                inputs += "[" + string.Format("{0}: {1}", x.Name, Regex.Replace(x.Type.Name, Reflector.TypingSetter, "[]")) + "]";
            });
            return string.Format(Templates.FunctionTemplate, function.Name, (string.IsNullOrEmpty(inputs) ? "N/A" : inputs), (string.IsNullOrEmpty(returnable) ? "Void" : returnable), GetXmlDocumentation(@namespace, @class, function.Name, function.Inputs).Sanitize());

        }

        internal static string GetPropertyDocumentation(string @namespace, string @class, Models.Property property)
        {
            string returnable = property.TypeName;
            return string.Format(Templates.PropertyTemplate, property.Name, returnable, GetXmlDocumentation(@namespace, @class, property.Name).Sanitize());
        }

        internal static string GetVoidDocumentation(string @namespace, string @class, Models.Void @void)
        {
            string inputs = "";
            @void.Inputs.ForEach((x) =>
            {
                inputs += "[" + string.Format("{0}: {1}", x.Name, Regex.Replace(x.Type.Name, Reflector.TypingSetter, "[]")) + "]";
            });
            return string.Format(Templates.VoidTemplate, @void.Name, (string.IsNullOrEmpty(inputs) ? "N/A" : inputs), GetXmlDocumentation(@namespace, @class, @void.Name, @void.Inputs).Sanitize());
        }

        internal static string GetNamespaceDocumentation(Models.Namespace @namespace)
        {
            Console.WriteLine("Reading " + @namespace.Name);
            var docTitle = string.Format(Title, @namespace.Name);
            string docIndex = string.Empty;
            string docBody = string.Empty;

            @namespace.Classes.ForEach((c) =>
            {
                Console.WriteLine("Reading " + c.Name);
                docIndex += string.Format(ClassLink, c.Name, c.Name.ToLower());
                Core.CurrentObject = c;
                docBody += c.Documentation;
            });

            @namespace.Interfaces.ForEach((i) =>
            {
                Console.WriteLine("Reading " + i.Name);
                docIndex += string.Format(ClassLink, i.Name, i.Name.ToLower());
                Core.CurrentObject = i;
                docBody += i.Documentation;
            });

            @namespace.Enums.ForEach((e) =>
            {
                Console.WriteLine("Reading " + e.Name);
                docIndex += string.Format(ClassLink, e.Name, e.Name.ToLower());
                Core.CurrentObject = e;
                docBody += string.Format(Templates.DiagramTemplate, e.Name, e.DiagramUrl);
            });

            var topography = string.Format(Templates.DiagramTemplate, @namespace.Name, @namespace.TopographyUrl);
            return string.Format(
                Templates.MainTemplate.
                Replace("[$Topography]", topography).
                Replace("[$PseudoNamespace]", $"[Pseudo Namespace]({Core.DocumentRoot}/{docTitle.Replace("#", "")})").
                Replace("[$NavigationUp]", $"[{GetNavigationUpLink(docTitle).Replace("#", "")}]({Core.DocumentRoot}/{GetNavigationUpLink(docTitle).Replace("#", "")}.md)"),
                docTitle,
                docIndex,
                docBody,
                Core.DocumentRoot
            );

        }

        internal static string GetNavigationUpLink(string input)
        {
            int index = input.LastIndexOf(".");
            if (index > 0)
            {
                input = input.Substring(0, index);
            }

            return input;

        }

        internal static string Template(string input, string @namespace, string @class)
        {
            return input.Replace("[$PseudoCode]", $"[Pseudo Code]({Core.DocumentRoot}/{@namespace.Replace("#", "")}/{@class.Replace("#", "")}.plantuml)");
        }

    }

}
