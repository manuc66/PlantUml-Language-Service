using System.IO;
using NUnit.Framework;
using PlantUmlLanguageService.Services.Offline;

namespace PlantUmlLanguageService.Tests
{
    public class PlantUmlRunnerTests
    {
        [Test]
        public void ItCanGeneratePngFromCommandLine()
        {
            var javaPath = SystemLocatorHelper.GetJavaPath();
            var plantUmlPath = SystemLocatorHelper.GetPlantUmlPath();
            using (var image = new PlantUmlRunner(javaPath, plantUmlPath).Create(PlantUmlSample.SamplePlantUml, "png"))
            {
                var tempFileName = Path.GetTempFileName();
                image.Save(tempFileName);
                ImageAsserts.AssertIsAPng(tempFileName);
            }
        }
    }
}