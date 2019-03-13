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
            var javaPath = PlantUmlPathLocator.FindJava();
            var plantUmlPath = PlantUmlPathLocator.FindPlantUml();
            var findGraphVizDot = PlantUmlPathLocator.FindGraphVizDot();
            using (var image = new PlantUmlRunner(javaPath, plantUmlPath, findGraphVizDot).Create(PlantUmlSample.SamplePlantUml, "png"))
            {
                var tempFileName = Path.GetTempFileName();
                image.Save(tempFileName);
                ImageAsserts.AssertIsAPng(tempFileName);
            }
        }
    }
}