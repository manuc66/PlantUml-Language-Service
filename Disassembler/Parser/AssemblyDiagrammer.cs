using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Disassembler.Parser
{
    internal class AssemblyDiagrammer
    {

        /// <summary>
        /// The include
        /// </summary>
        private const string Include = "!include {0}.plantuml";
        /// <summary>
        /// The stereo type
        /// </summary>
        private const string StereoType = "<< ({0},{1}) {2} >> ";
        /// <summary>
        /// The stereo type element
        /// </summary>
        private const string StereoTypeElement = "[$stereotype]";
        /// <summary>
        /// The proto type element
        /// </summary>
        private const string ProtoTypeElement = "[$prototype]";
        /// <summary>
        /// The type element
        /// </summary>
        private const string TypeElement = "[$type]";
        /// <summary>
        /// The name element
        /// </summary>
        private const string NameElement = "[$name]";
        /// <summary>
        /// The body element
        /// </summary>
        private const string BodyElement = "[$body]";
        /// <summary>
        /// The property notation
        /// </summary>
        private const string Property_Notation = "{0}{1}:{2}";
        /// <summary>
        /// The method notation
        /// </summary>
        private const string Method_Notation = "{0}{1}({2}):{3}";
        /// <summary>
        /// The overload notation
        /// </summary>
        private const string Overload_Notation = "{0}: {1}";

        /// <summary>
        /// Gets or sets the aggregations.
        /// </summary>
        /// <value>
        /// The aggregations.
        /// </value>
        public static List<string> Aggregations { get; set; } = new List<string>();
        /// <summary>
        /// Gets or sets the compositions.
        /// </summary>
        /// <value>
        /// The compositions.
        /// </value>
        public static List<string> Compositions { get; set; } = new List<string>();
        /// <summary>
        /// Gets or sets the providers.
        /// </summary>
        /// <value>
        /// The providers.
        /// </value>
        public static List<string> Providers { get; set; } = new List<string>();
        /// <summary>
        /// Gets or sets the consumers.
        /// </summary>
        /// <value>
        /// The consumers.
        /// </value>
        public static List<string> Consumers { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the relationships.
        /// </summary>
        /// <value>
        /// The relationships.
        /// </value>
        public static List<Models.Relationship> Relationships { get; set; } = new List<Models.Relationship>();

        /// <summary>
        /// Maps the topography.
        /// </summary>
        /// <param name="Namespace">The namespace.</param>
        /// <returns></returns>
        internal static string MapTopography(Models.Namespace Namespace)
        {
            string markup = Templates.NamespaceDescriptor;
            markup = markup.Replace(NameElement, Namespace.Name);
            List<string> body = new List<string>();
            Namespace.Classes.ForEach((c) =>
            {
                string item = Templates.ClassDescriptor;
                if (c.Module) { item = item.Replace(ProtoTypeElement, "").Replace(StereoTypeElement, string.Format(StereoType, "M", "application", "module ")); }
                if (c.Static) { item = item.Replace(ProtoTypeElement, "").Replace(StereoTypeElement, string.Format(StereoType, "S", "orchid", "static ")); }
                if (c.Abstract) { item = item.Replace(ProtoTypeElement, "abstract ").Replace(StereoTypeElement, ""); }
                item = item.Replace(TypeElement, "class ").Replace(NameElement, c.Name);
                body.Add(item);
            });
            Namespace.Interfaces.ForEach((i) =>
            {
                string item = Templates.ClassDescriptor;
                item = item.Replace(TypeElement, "interface ").Replace(NameElement, i.Name).Replace(ProtoTypeElement, "").Replace(StereoTypeElement, "");
                body.Add(item);
            });
            Namespace.Enums.ForEach((e) =>
            {
                string item = Templates.ClassDescriptor;
                item = item.Replace(TypeElement, "enum ").Replace(NameElement, e.Name).Replace(ProtoTypeElement, "").Replace(StereoTypeElement, "");
                body.Add(item);
            });
            markup = markup.Replace(BodyElement, string.Join(Environment.NewLine, body.ToArray()));
            return markup;
        }

        /// <summary>
        /// Writes the class diagram.
        /// </summary>
        /// <param name="class">The class.</param>
        /// <param name="useIncludes">if set to <c>true</c> [use includes].</param>
        /// <returns></returns>
        internal static string WriteClassDiagram(Models.Class @class, bool useIncludes = false)
        {
            string markup = Templates.Descriptor;

            if (@class.Module)
            {
                markup = markup.Replace(ProtoTypeElement, "").Replace(StereoTypeElement, string.Format(StereoType, "M", "application", "module "));
            }

            else if (@class.Static && @class.Abstract)
            {
                markup = markup.Replace(ProtoTypeElement, "").Replace(StereoTypeElement, string.Format(StereoType, "S", "orchid", "static "));
            }

            else if (@class.Abstract)
            {
                markup = markup.Replace(ProtoTypeElement, "abstract ").Replace(StereoTypeElement, "");
            }

            else
            {
                markup = markup.Replace(ProtoTypeElement, "").Replace(StereoTypeElement, "");
            }

            markup = markup.Replace(TypeElement, "class ").Replace(NameElement, @class.Name);

            markup = markup.Replace(BodyElement, WriteBody(@class));

            if (@class.Implements != null)
            {
                markup += Lollipop(@class.Name, @class.Implements.Name);
            }
            if (@class.Inherits != null)
            {
                markup += Extend(@class.Name, @class.Inherits.Name);
            }

            var aggregates = Aggregations.Distinct().ToList();
            Aggregations.Clear();
            var composites = Compositions.Distinct().ToList();
            Compositions.Clear();
            var consumption = Consumers.Distinct().ToList();
            Consumers.Clear();
            var provision = Providers.Distinct().ToList();
            Providers.Clear();

            aggregates.ForEach((n) =>
            {
                if (useIncludes)
                {
                    markup = ((IncludeDiagram(n).Length > 0) ? IncludeDiagram(n) : "") + markup;
                }
                markup += Aggregate(@class.Name, n);
                Relationships.Add(new Models.Relationship
                {
                    PrincipalObject = @class,
                    AncillaryObject = new Models.Generic { Name = n },
                    Type = Models.RelationshipType.Aggregation
                });
            });

            composites.ForEach((n) =>
            {
                if (useIncludes)
                {
                    markup = ((IncludeDiagram(n).Length > 0) ? IncludeDiagram(n) : "") + markup;
                }
                markup += Composite(@class.Name, n);
                Relationships.Add(new Models.Relationship
                {
                    PrincipalObject = @class,
                    AncillaryObject = new Models.Generic { Name = n },
                    Type = Models.RelationshipType.Composition
                });
            });

            consumption.ForEach((n) =>
            {
                if (useIncludes)
                {
                    markup = ((IncludeDiagram(n).Length > 0) ? IncludeDiagram(n) : "") + markup;
                }
                markup += Consumes(@class.Name, n);
                Relationships.Add(new Models.Relationship
                {
                    PrincipalObject = @class,
                    AncillaryObject = new Models.Generic { Name = n },
                    Type = Models.RelationshipType.Dependency
                });
            });

            Providers.ForEach((n) =>
            {
                if (useIncludes)
                {
                    markup = ((IncludeDiagram(n).Length > 0) ? IncludeDiagram(n) : "") + markup;
                }
                markup += Provides(@class.Name, n);
                Relationships.Add(new Models.Relationship
                {
                    PrincipalObject = @class,
                    AncillaryObject = new Models.Generic { Name = n },
                    Type = Models.RelationshipType.Association
                });
            });

            return markup;

        }

        /// <summary>
        /// Writes the class diagram.
        /// </summary>
        /// <param name="interface">The interface.</param>
        /// <param name="useIncludes">if set to <c>true</c> [use includes].</param>
        /// <returns></returns>
        internal static string WriteClassDiagram(Models.Interface @interface, bool useIncludes = false)
        {
            string markup = Templates.Descriptor;
            markup = markup.Replace(TypeElement, "interface ").Replace(NameElement, @interface.Name).Replace(ProtoTypeElement, "").Replace(StereoTypeElement, "");

            markup = markup.Replace(BodyElement, WriteBody(@interface));

            var aggregates = Aggregations.Distinct().ToList();
            Aggregations.Clear();
            var composites = Compositions.Distinct().ToList();
            Compositions.Clear();

            aggregates.ForEach((n) => markup += Aggregate(@interface.Name, n));
            composites.ForEach((n) => markup += Composite(@interface.Name, n));

            return markup;

        }

        /// <summary>
        /// Writes the class diagram.
        /// </summary>
        /// <param name="enum">The enum.</param>
        /// <returns></returns>
        internal static string WriteClassDiagram(Models.Enum @enum)
        {
            string markup = Templates.Descriptor;
            markup = markup.Replace(TypeElement, "enum ").Replace(NameElement, @enum.Name).Replace(ProtoTypeElement, "").Replace(StereoTypeElement, "");
            string Body = string.Empty;

            foreach (var num in @enum.Values)
            {
                Body += (num.Key + ":" + num.Value.ToString()) + Environment.NewLine;
            }

            markup = markup.Replace(BodyElement, Body);

            return markup;

        }

        /// <summary>
        /// Writes the body.
        /// </summary>
        /// <param name="class">The class.</param>
        /// <returns></returns>
        private static string WriteBody(Models.Class @class)
        {
            string Body = string.Empty;

            WriteProperties(@class.Properties).ForEach((n) => Body += (n + '\n'));
            WriteVoids(@class.Voids).ForEach((n) => Body += (n + Environment.NewLine));
            WriteFunctions(@class.Functions).ForEach((n) => Body += (n + Environment.NewLine));

            return Body;

        }

        /// <summary>
        /// Writes the body.
        /// </summary>
        /// <param name="interface">The interface.</param>
        /// <returns></returns>
        private static string WriteBody(Models.Interface @interface)
        {
            string Body = string.Empty;

            WriteProperties(@interface.Properties).ForEach((n) => Body += (n + '\n'));
            WriteVoids(@interface.Voids).ForEach((n) => Body += (n + Environment.NewLine));
            WriteFunctions(@interface.Functions).ForEach((n) => Body += (n + Environment.NewLine));

            return Body;

        }

        /// <summary>
        /// Lollipops the specified class.
        /// </summary>
        /// <param name="class">The class.</param>
        /// <param name="interface">The interface.</param>
        /// <returns></returns>
        private static string Lollipop(string @class, string @interface)
        {
            Relationships.Add(new Models.Relationship
            {
                AncillaryObject = new Models.Generic { Name = Regex.Replace(@interface, Reflector.TypingSetter, "") },
                Type = Models.RelationshipType.Realization
            });
            return (@class + " --() " + Regex.Replace(@interface, Reflector.TypingSetter, "")) + Environment.NewLine;

        }

        /// <summary>
        /// Extends the specified class.
        /// </summary>
        /// <param name="class">The class.</param>
        /// <param name="interface">The interface.</param>
        /// <returns></returns>
        private static string Extend(string @class, string @interface)
        {
            if (!(@interface == "Object"))
            {
                Relationships.Add(new Models.Relationship
                {
                    AncillaryObject = new Models.Generic { Name = Regex.Replace(@interface, Reflector.TypingSetter, "") },
                    Type = Models.RelationshipType.Inheritance
                });
                return (@class + " -d-|> " + Regex.Replace(@interface, Reflector.TypingSetter, "") + " : Inherits > ") + Environment.NewLine;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Aggregates the specified class.
        /// </summary>
        /// <param name="class">The class.</param>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        private static string Aggregate(string @class, string @object)
        {
            if (!Globals.BaseTypes.Contains(@object.Replace("[]", "")))
            {
                return (@class + " o.. " + (@object.EndsWith("[]") ? "\" Many \"" + @object.Replace("[]", "") : Regex.Replace(@object, Reflector.TypingSetter, "")) + Environment.NewLine);
            }
            return string.Empty;
        }

        /// <summary>
        /// Composites the specified class.
        /// </summary>
        /// <param name="class">The class.</param>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        private static string Composite(string @class, string @object)
        {
            if (!Globals.BaseTypes.Contains(@object.Replace("[]", "")))
            {
                return (@class + " *-- " + (@object.EndsWith("[]") ? "\" Many \"" + @object.Replace("[]", "") : Regex.Replace(@object, Reflector.TypingSetter, "")) + Environment.NewLine);
            }
            return string.Empty;
        }

        /// <summary>
        /// Consumeses the specified class.
        /// </summary>
        /// <param name="class">The class.</param>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        private static string Consumes(string @class, string @object)
        {
            if (!Globals.BaseTypes.Contains(@object.Replace("[]", "")))
            {
                return (@class + " <.. " + (@object.EndsWith("[]") ? "\" Many \"" + @object.Replace("[]", "") : Regex.Replace(@object, Reflector.TypingSetter, "")) + Environment.NewLine);
            }
            return string.Empty;
        }

        /// <summary>
        /// Provideses the specified class.
        /// </summary>
        /// <param name="class">The class.</param>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        private static string Provides(string @class, string @object)
        {
            if (!Globals.BaseTypes.Contains(@object.Replace("[]", "")))
            {
                return (@class + " -r-> " + (@object.EndsWith("[]") ? "\" Many \"" + @object.Replace("[]", "") : Regex.Replace(@object, Reflector.TypingSetter, "")) + Environment.NewLine);
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        private static List<string> WriteProperties(List<Models.Property> properties)
        {
            List<string> _props = new List<string>();
            properties.ForEach((n) =>
            {
                if ((!Globals.DataTypes.Contains(n.Type)) && (!Globals.BaseTypes.Contains(Regex.Replace(n.Type.Name, Reflector.TypingSetter, ""))))
                {
                    Compositions.Add(Regex.Replace(n.Type.Name, Reflector.TypingSetter, ""));
                }
                _props.Add('\t' + string.Format(Property_Notation, (n.ReadOnly ? "#" : "+"), n.Name, Regex.Replace(n.Type.Name, Reflector.TypingSetter, "[]")));
            });

            return _props;

        }

        /// <summary>
        /// Writes the voids.
        /// </summary>
        /// <param name="voids">The voids.</param>
        /// <returns></returns>
        private static List<string> WriteVoids(List<Models.Void> voids)
        {
            List<string> _voids = new List<string>();
            voids.ForEach((n) =>
            {
                string inputs = string.Empty;
                n.Inputs.ForEach((x) => inputs += (string.Format(Overload_Notation, x.Name, Regex.Replace(x.Type.Name, Reflector.TypingSetter, "[]")) + ","));
                _voids.Add('\t' + string.Format(Method_Notation, "+", n.Name, ((inputs.Length > 0) ? inputs.TrimEnd(',') : ""), "void"));
            });

            return _voids;

        }

        /// <summary>
        /// Writes the functions.
        /// </summary>
        /// <param name="funcs">The funcs.</param>
        /// <returns></returns>
        private static List<string> WriteFunctions(List<Models.Function> funcs)
        {
            List<string> _funcs = new List<string>();
            funcs.ForEach((n) =>
            {
                string inputs = string.Empty;
                if (!Globals.BaseTypes.Contains(n.Return))
                {
                    Providers.Add(n.Return.Replace("[]", ""));
                }
                n.Inputs.ForEach((x) =>
                {
                    if ((!Globals.DataTypes.Contains(x.Type)) && (!Globals.BaseTypes.Contains(Regex.Replace(x.Type.Name, Reflector.TypingSetter, ""))))
                    {
                        Consumers.Add(Regex.Replace(x.Type.Name, Reflector.TypingSetter, ""));
                    }
                    inputs += (string.Format(Overload_Notation, x.Name, Regex.Replace(x.Type.Name, Reflector.TypingSetter, "[]")) + ",");
                });
                _funcs.Add('\t' + string.Format(Method_Notation, "+", n.Name, ((inputs.Length > 0) ? inputs.TrimEnd(',') : ""), n.Return.Replace("[]", "")));
            });

            return _funcs;
        }

        public static List<Models.Relationship> GetRelationships(Models.Object type)
        {
            var Related = Relationships.Distinct().ToList();
            Relationships.Clear();
            Related.ForEach((r) =>
            {
                if (r.PrincipalObject == null)
                {
                    r.PrincipalObject = type;
                }
                Console.WriteLine(r.PrincipalObject.Name + r.Type.GetArrow() + r.AncillaryObject.Name);
            });

            return Related;
        }

        /// <summary>
        /// Includes the diagram.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static string IncludeDiagram(string name)
        {
            var directories = (new System.IO.DirectoryInfo(Globals.Root)).GetDirectories().ToList();
            string IncludePath = string.Empty;
            directories.ForEach((dir) =>
            {
                var filePath = FindFile(dir, name);
                if (!string.IsNullOrEmpty(filePath))
                {
                    IncludePath = filePath;
                }
                //'Exit Sub
            });

            var fullpath = System.IO.Path.Combine(Globals.Root, System.IO.Path.Combine(IncludePath, name));

            if (System.IO.File.Exists(fullpath + ".plantuml"))
            {
                return string.Format(Include, fullpath);
            }
            return string.Empty;

        }

        /// <summary>
        /// Finds the file.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static string FindFile(System.IO.DirectoryInfo dir, string name)
        {
            var _files = dir.GetFiles().ToList();
            string foldername = string.Empty;
            _files.ForEach((f) =>
            {
                if (System.IO.Path.GetFileNameWithoutExtension(f.Name) == name)
                {
                    foldername = dir.Name;
                }
            });

            return foldername;

        }

    }
}