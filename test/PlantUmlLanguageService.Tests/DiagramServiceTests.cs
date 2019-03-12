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
            var imageUrl = DiagramService.GetImageUrlForSource(PlantUmlSample.SamplePlantUml, "png");
            _imageOnDisk = ImageService.SaveImagefromUrlToDisk(imageUrl, Core.ImageRequestFormat.Png, Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));

            ImageAsserts.AssertIsAPng(_imageOnDisk);
        }

        [Test]
        public void ItGetHttpsImageUrlForSource()
        {
            var imageUrl = DiagramService.GetImageUrlForSource(PlantUmlSample.SamplePlantUml, "png");
        
            StringAssert.StartsWith("https", imageUrl);
        }
    }
}