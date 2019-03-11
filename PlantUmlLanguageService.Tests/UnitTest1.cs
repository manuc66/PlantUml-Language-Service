using System;
using System.IO;
using NUnit.Framework;
using PlantUmlLanguageService.Disassembler;
using PlantUmlLanguageService.Disassembler.Services;
using PlantUmlLanguageService.Services;

namespace PlantUmlLanguageService.Tests
{
    public class DiagramServiceTests
    {
        private const string SamplePlantUml = "@startuml\r\n\r\nobject user {\r\n  name = \"Dummy\"\r\n  id = 123\r\n}\r\n\r\n@enduml";
        private readonly byte[] _pngHeader = { 137, 80, 78, 71 }; // PNG file header
        private string _imageOnDisk;

        [TearDown]
        public void Cleanup()
        {
            if (_imageOnDisk != null)
            {
                File.Delete(_imageOnDisk);
                _imageOnDisk = null;
            }
        }

        [Test]
        public void ItGetValidPngImageUrlForSource()
        {
            var imageUrl = DiagramService.GetImageUrlForSource(SamplePlantUml, "png");
            _imageOnDisk = ImageService.SaveImagefromUrlToDisk(imageUrl, Core.ImageRequestFormat.Png, Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));

            AssertIsAPng(_imageOnDisk);
        }

        [Test]
        public void ItGetHttpsImageUrlForSource()
        {
            var imageUrl = DiagramService.GetImageUrlForSource(SamplePlantUml, "png");
        
            StringAssert.StartsWith("https", imageUrl);
        }

        private void AssertIsAPng(string imageOnDisk)
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