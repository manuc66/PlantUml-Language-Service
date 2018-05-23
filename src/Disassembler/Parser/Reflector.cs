using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;
using PlantUmlLanguageService.Services;
using PlantUmlLanguageService.Disassembler.Services;

namespace PlantUmlLanguageService.Disassembler.Parser
{
    class Reflector
    {
        /// <summary>
        /// The typing setter
        /// </summary>
        public static string TypingSetter = "(`[0-9])";

        /// <summary>
        /// Gets or sets the namespaces.
        /// </summary>
        /// <value>
        /// The namespaces.
        /// </value>
        private static List<Models.Namespace> Namespaces { get; set; } = new List<Models.Namespace>();

        /// <summary>
        /// Lists the namespaces.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        private static List<Models.Namespace> ListNamespaces(Assembly assembly)
        {
            var names = assembly.GetTypes().Select((t) => t.Namespace).Distinct().ToList();
            names.Where((x) => (x != null) && (!string.IsNullOrEmpty(x)) && (!x.Contains("My.")) && (!x.Contains(".My"))).ToList().ForEach((n) => Namespaces.Add(new Models.Namespace { Name = n }));

            Namespaces.ForEach((n) =>
            {
                Console.WriteLine(n.Name);
                GetTypes(assembly, n);
                n.Topography = AssemblyDiagrammer.MapTopography(n);
                n.TopographyUrl = DiagramService.GetImageUrlForSource(n.Topography, "svg");
                n.Documentation = DocumentService.GetNamespaceDocumentation(n);
            });

            return Namespaces;

        }

        /// <summary>
        /// Gets the types.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="NameSpace">The name space.</param>
        private static void GetTypes(Assembly assembly, Models.Namespace NameSpace)
        {
            Console.WriteLine(NameSpace.Name);
            List<Type> types = new List<Type>();
            List<Type> AllTypes = assembly.GetTypes().ToList();
            types.AddRange(AllTypes.Where((t) => t.Namespace.ToLower() == NameSpace.Name.ToLower()));
            if (types != null)
            {
                types.ForEach((n) =>
                {
                    if (!n.Name.Contains("$"))
                    {
                        if (n.IsInterface) { NameSpace.Interfaces.Add(ConvertToInterface(n)); }
                        if (n.IsClass) { NameSpace.Classes.Add(ConvertToClass(n)); }
                        if (n.IsEnum) { NameSpace.Enums.Add(ConvertToEnum(n)); }
                    }
                });
            }
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static List<Models.Property> GetProperties(Type type)
        {
            List<Models.Property> props = new List<Models.Property>();
            type.GetProperties().ToList().ForEach((n) =>
            {
                var prop = new Models.Property
                {
                    Name = n.Name,
                    ReadOnly = (n.CanWrite ? false : true),
                    TypeName = Regex.Replace(n.PropertyType.Name, TypingSetter, "[]"),
                    Type = n.PropertyType
                };
                prop.Documentation = DocumentService.GetPropertyDocumentation(type.Namespace, type.Name, prop);
                props.Add(prop);
            });
            return props;
        }

        /// <summary>
        /// Gets the voids.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static List<Models.Void> GetVoids(Type type)
        {
            List<Models.Void> voids = new List<Models.Void>();
            type.GetMethods().ToList().Where((x) => x.ReturnType == typeof(void)).ToList().ForEach((n) =>
            {
                if ((!n.Name.Contains("get_")) && (!n.Name.Contains("set_")))
                {
                    var _void = new Models.Void
                    {
                        Name = n.Name,
                        Inputs = GetOverloads(n),
                        Modifier = SetModifier(n)
                    };
                    _void.Documentation = DocumentService.GetVoidDocumentation(type.Namespace, type.Name, _void);
                    voids.Add(_void);
                }
            });
            return voids;
        }

        /// <summary>
        /// Gets the functions.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static List<Models.Function> GetFunctions(Type type)
        {
            List<Models.Function> funcs = new List<Models.Function>();
            type.GetMethods().ToList().Where((x) => x.ReturnType != typeof(void)).ToList().ForEach((n) =>
            {
                if ((!n.Name.Contains("get_")) && (!n.Name.Contains("set_")))
                {
                    if (!Globals.BaseFunctions.Contains(n.Name))
                    {
                        var func = new Models.Function
                        {
                            Name = n.Name,
                            Return = Regex.Replace(n.ReturnType.Name, TypingSetter, "[]"),
                            Inputs = GetOverloads(n),
                            Modifier = SetModifier(n)
                        };
                        func.Documentation = DocumentService.GetFunctionDocumentation(type.Namespace, type.Name, func);
                        funcs.Add(func);
                    }
                }
            });
            return funcs;
        }

        /// <summary>
        /// Converts to interface.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static Models.Interface ConvertToInterface(Type type)
        {
            var @interface = new Models.Interface
            {
                Name = Regex.Replace(type.Name, TypingSetter, ""),
                Properties = GetProperties(type),
                Functions = GetFunctions(type),
                Voids = GetVoids(type)
            };

            @interface.Diagram = string.Format(Templates.PsuedoCode, AssemblyDiagrammer.WriteClassDiagram(@interface));
            @interface.DiagramUrl = DiagramService.GetImageUrlForSource(@interface.Diagram, "svg");
            @interface.Documentation = DocumentService.GetInterfaceDocumentation(type.Namespace, @interface);

            @interface.Relationships = AssemblyDiagrammer.GetRelationships(@interface);

            return @interface;

        }

        /// <summary>
        /// Converts to class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Models.Class ConvertToClass(Type type)
        {
            var @class = new Models.Class
            {
                Name = Regex.Replace(type.Name, TypingSetter, ""),
                Properties = GetProperties(type),
                Functions = GetFunctions(type),
                Voids = GetVoids(type),
                Abstract = type.IsAbstract,
                Static = IsStaticType(type),
                Module = IsModule(type),
                Implements = type.GetInterfaces().FirstOrDefault(),
                Inherits = (type.BaseType ?? null)
            };

            @class.Diagram = string.Format(Templates.PsuedoCode, AssemblyDiagrammer.WriteClassDiagram(@class));
            @class.DiagramUrl = DiagramService.GetImageUrlForSource(@class.Diagram, "svg");
            @class.Documentation = DocumentService.GetClassDocumentation(type.Namespace, @class);

            @class.Relationships = AssemblyDiagrammer.GetRelationships(@class);

            return @class;

        }

        /// <summary>
        /// Converts to enum.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static Models.Enum ConvertToEnum(Type type)
        {
            Dictionary<object, object> Values = new Dictionary<object, object>();

            type.GetFields().ToList().ForEach((e) =>
            {
                if (e.FieldType.IsEnum)
                {
                    Values.Add(e.Name, e.GetRawConstantValue());
                }
            });

            var @enum = new Models.Enum
            {
                Name = Regex.Replace(type.Name, TypingSetter, ""),
                Values = Values
            };

            @enum.Diagram = string.Format(Templates.PsuedoCode, AssemblyDiagrammer.WriteClassDiagram(@enum));
            @enum.DiagramUrl = DiagramService.GetImageUrlForSource(@enum.Diagram, "svg");
            @enum.Documentation = "";

            return @enum;

        }

        /// <summary>
        /// Determines whether [is static type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is static type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsStaticType(Type type)
        {
            return ((type.GetConstructor(Type.EmptyTypes) == null && type.IsAbstract && type.IsSealed) ? true : false);
        }

        /// <summary>
        /// Determines whether the specified type is module.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is module; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsModule(Type type)
        {
            try
            {
                return ((type.GetCustomAttributes(false).Where((a) => a is Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute).FirstOrDefault() != null) ? true : false);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the overloads.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        private static List<Models.Overload> GetOverloads(MethodInfo method)
        {
            List<Models.Overload> inputs = new List<Models.Overload>();
            method.GetParameters().ToList().ForEach((n) =>
            {
                if ((!n.Name.Contains("get_")) && (!n.Name.Contains("set_")))
                {
                    inputs.Add(new Models.Overload
                    {
                        Name = n.Name,
                        TypeName = Regex.Replace(n.ParameterType.Name, TypingSetter, "[]"),
                        Type = n.ParameterType
                    });
                }
            });
            return inputs;

        }

        /// <summary>
        /// Parses the assembly.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static Models.Assembly ParseAssembly(string path)
        {
            var dll = Assembly.LoadFrom(path);
            Console.WriteLine(dll.FullName);
            Console.WriteLine(dll.GetModules().FirstOrDefault().Name);
            return new Models.Assembly
            {
                Name = dll.FullName,
                RootNamespace = dll.GetModules().FirstOrDefault().Name,
                Namespaces = ListNamespaces(dll)
            };

        }

        /// <summary>
        /// Parses the assembly to uml.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="output">The output.</param>
        /// <param name="useIncludes">if set to <c>true</c> [use includes].</param>
        /// <returns></returns>
        public static bool ParseAssemblyToUml(string path, string output, bool useIncludes = false)
        {
            Globals.Root = output;
            try
            {
                var dll = ParseAssembly(path);
                string umlFile = string.Empty;
                string lnkFile = string.Empty;
                dll.Namespaces.ForEach((n) =>
                {
                    var root = System.IO.Path.Combine(output, n.Name);
                    if (!System.IO.Directory.Exists(root))
                    {
                        System.IO.Directory.CreateDirectory(root);
                    }
                    n.Classes.ForEach((c) =>
                    {
                        umlFile = System.IO.Path.Combine(root, c.Name + ".plantuml");
                        System.IO.File.WriteAllText(umlFile, c.Diagram);
                        c.Relationships.ForEach((r) =>
                        {
                            //lnkFile = IO.Path.Combine(root, c.Name + String.Format("{0}.{1}.{2}", c.Name, r.Type.ToString, ".plnk"))
                            //Dim lnk = String.Format("{0} {1} {2}", r.PrincipalObject.Name, r.Type.GetArrow, r.AncillaryObject.Name)
                            //IO.File.WriteAllText(lnkFile, lnk)
                        });
                    });
                    n.Interfaces.ForEach((i) =>
                    {
                        umlFile = System.IO.Path.Combine(root, i.Name + ".plantuml");
                        System.IO.File.WriteAllText(umlFile, i.Diagram);
                        i.Relationships.ForEach((r) =>
                        {
                            //lnkFile = IO.Path.Combine(root, i.Name + String.Format("{0}.{1}.{2}", i.Name, r.Type.ToString, ".plnk"))
                            //Dim lnk = String.Format("{0} {1} {2}", r.PrincipalObject.Name, r.Type.GetArrow, r.AncillaryObject.Name)
                            //IO.File.WriteAllText(lnkFile, lnk)
                        });
                    });
                    n.Enums.ForEach((e) =>
                    {
                        umlFile = System.IO.Path.Combine(root, e.Name + ".plantuml");
                        System.IO.File.WriteAllText(umlFile, e.Diagram);
                    });
                });

            }
            catch (Exception ex)
            {
                Globals.Message += Environment.NewLine + ex.Message;
                if (ex is ReflectionTypeLoadException)
                {
                    var typeLoadException = ex as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions.ToList();
                    loaderExceptions.ForEach((e) => Globals.Message += Environment.NewLine + e.Message);

                }

                return false;
            }

            return true;

        }

        /// <summary>
        /// Parses the object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string ParseObject(Type type)
        {
            if (true == type.IsClass)
            {
                return ConvertToClass(type).Diagram;
            }
            else if (true == type.IsInterface)
            {
                return ConvertToInterface(type).Diagram;
            }

            return null;

        }

        /// <summary>
        /// Parses the object to uml.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        public static bool ParseObjectToUml(Type type, string output)
        {
            Globals.Root = output;
            try
            {
                var uml = ParseObject(type);
                var umlFile = System.IO.Path.Combine(output, Regex.Replace(type.Name, TypingSetter, "") + ".plantuml");
                System.IO.File.WriteAllText(umlFile, uml);

            }
            catch (Exception ex)
            {
                Globals.Message = Environment.NewLine + ex.Message;

                if (ex is ReflectionTypeLoadException)
                {
                    var typeLoadException = ex as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions.ToList();
                    loaderExceptions.ForEach((e) => Globals.Message += Environment.NewLine + e.Message);

                }

                return false;
            }

            return true;

        }

        /// <summary>
        /// Sets the modifier.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        private static Models.ModifierType SetModifier(MethodInfo method)
        {
            if (method.IsPublic) { return Models.ModifierType.Public; }
            if (method.IsPrivate) { return Models.ModifierType.Private; }
            return Models.ModifierType.Internal; 
        }

    }
}
