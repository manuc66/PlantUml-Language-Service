using System;

namespace PlantUmlLanguageService.Services.Offline
{
    public class PlantUmlPathLocator
    {
        public static string FindGraphVizDot()
        {
            var dotFindContext = PathFinder.LookupFile("dot.exe");

            var dotPath = dotFindContext.InPath();
            
            if (string.IsNullOrEmpty(dotPath))
                dotPath = dotFindContext.BeneathProgramFiles("Graphviz*");


            if (string.IsNullOrEmpty(dotPath))
                dotPath = dotFindContext.BeneathEnv("ChocolateyInstall");

            return dotPath;
        }

        public static string FindJava()
        {
            var javaFindContext = PathFinder.LookupFile("java.exe");
            var javaExePath = javaFindContext.InPath();
            if (string.IsNullOrEmpty(javaExePath))
            {
                javaExePath = javaFindContext.BeneathEnv("JAVA_HOME");
            }

            return javaExePath;
        }

        public static string FindPlantUml()
        {
            return Environment.GetEnvironmentVariable("PLANTUML_JAR")
                   ?? Environment.GetEnvironmentVariable("PLANTUML")
                   ?? PathFinder.LookupFile("plantuml.jar").BeneathEnv("ChocolateyInstall")
                   ?? PathFinder.LookupFile("plantuml.jar").InPath();
        }
    }
}