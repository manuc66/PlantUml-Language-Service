using Disassembler.Attributes;
using System;
using System.Reflection;
using PlantUmlLanguageService.Services;

namespace Disassembler
{
    /// <summary>
    /// 
    /// </summary>
    public static class Globals
    {

        /// <summary>
        /// The current object
        /// </summary>
        public static Models.Object CurrentObject;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public static string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the root.
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        public static string Root { get; set; } = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        /// <summary>
        /// Gets or sets the document root.
        /// </summary>
        /// <value>
        /// The document root.
        /// </value>
        public static object DocumentRoot { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the base functions.
        /// </summary>
        /// <value>
        /// The base functions.
        /// </value>
        public static string[] BaseFunctions { get; set; } = new[] { "ToString", "Equals", "GetHashCode", "GetType" };

        /// <summary>
        /// Gets or sets the data types.
        /// </summary>
        /// <value>
        /// The data types.
        /// </value>
        public static Type[] DataTypes { get; set; } = new[] { typeof(string), typeof(Int16), typeof(Int32), typeof(Int64), typeof(UInt16), typeof(UInt32), typeof(UInt64), typeof(bool), typeof(DateTime), typeof(object), typeof(Type), typeof(float), typeof(double), typeof(decimal), typeof(byte), typeof(Guid) };

        /// <summary>
        /// Gets or sets the base types.
        /// </summary>
        /// <value>
        /// The base types.
        /// </value>
        public static string[] BaseTypes { get; set; } = new[] { "String", "Int16", "Int32", "Int64", "UInt16", "UInt32", "UInt64", "Boolean", "Date", "Object", "Type", "Single", "Double", "Decimal", "Byte", "Byte[]", "Guid" };

        /// <summary>
        /// Gets or sets the diagram URL.
        /// </summary>
        /// <value>
        /// The diagram URL.
        /// </value>
        public static string DiagramUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public enum ImageRequestFormat
        {
            /// <summary>
            /// The PNG
            /// </summary>
            [TextValue("png")]
            Png = 1,
            /// <summary>
            /// The SVG
            /// </summary>
            [TextValue("svg")]
            Svg = 2,
            /// <summary>
            /// The JPG
            /// </summary>
            [TextValue("jpg")]
            Jpg = 3
        }

        /// <summary>
        /// Gets the diagram.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="imageformat">The imageformat.</param>
        /// <returns></returns>
        private static string GetDiagram(string source, string imageformat)
        {
            return DiagramService.GetImageUrlForSource(source, imageformat);
        }

        /// <summary>
        /// Renders the diagram.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="imageformat">The imageformat.</param>
        public static void RenderDiagram(string source, Globals.ImageRequestFormat imageformat)
        {
            DiagramUrl = GetDiagram(source, imageformat.GetText());
        }

    }

}