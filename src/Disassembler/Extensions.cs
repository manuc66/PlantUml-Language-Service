using PlantUmlLanguageService.Disassembler.Attributes;
using PlantUmlLanguageService.Disassembler.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace PlantUmlLanguageService.Disassembler
{
    internal static class Extensions
    {
        /// <summary>
        /// To the bytes.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <returns></returns>
        public static byte[] ToBytes(this Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Bmp);
            return ms.ToArray();
        }

        /// <summary>
        /// To the image.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static Image ToImage(this byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            return Image.FromStream(ms);
        }

        /// <summary>
        /// Renames the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="newFile">The new file.</param>
        /// <returns></returns>
        public static bool Rename(this FileInfo filePath, string newFile)
        {
            try
            {
                if (File.Exists(newFile))
                {
                    File.Delete(newFile);
                }
                File.Move(filePath.FullName, newFile);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetText(this System.Enum value)
        {
            var attributeValue = (TextValueAttribute)(value.GetType().GetField(value.ToString()).GetCustomAttributes(false).Where((a) => a is TextValueAttribute).FirstOrDefault());
            return ((attributeValue != null) ? attributeValue.Value : value.ToString());
        }

        /// <summary>
        /// Gets the modifier.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetModifier(this ModifierType value)
        {
            var attributeValue = (ModifierAttribute)(value.GetType().GetField(value.ToString()).GetCustomAttributes(false).Where((a) => a is ModifierAttribute).FirstOrDefault());
            return ((attributeValue != null) ? attributeValue.Value : value.ToString());
        }

        /// <summary>
        /// Gets the arrow.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetArrow(this RelationshipType value)
        {
            var attributeValue = (ArrowAttribute)(value.GetType().GetField(value.ToString()).GetCustomAttributes(false).Where((a) => a is ArrowAttribute).FirstOrDefault());
            return ((attributeValue != null) ? attributeValue.Value : value.ToString());
        }

        /// <summary>
        /// Sanitizes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string Sanitize(this string input)
        {
            return input.Replace("{", "<").Replace("}", ">");
        }

    }
}
