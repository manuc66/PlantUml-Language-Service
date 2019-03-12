using System.IO;
using NUnit.Framework;

namespace PlantUmlLanguageService.Tests
{
    public class ImageAsserts
    {
        private static readonly byte[] _pngHeader = { 137, 80, 78, 71 }; // PNG file header

        public static void AssertIsAPng(string imageOnDisk)
        {
            using (var imageStream = new FileStream(imageOnDisk, FileMode.Open, FileAccess.Read))
            {
                byte[] header = new byte[_pngHeader.Length];
                imageStream.Read(header, 0, header.Length);

                CollectionAssert.AreEqual(_pngHeader, header);
            }
        }
    }
}