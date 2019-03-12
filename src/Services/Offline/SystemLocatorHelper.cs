using System;
using System.IO;
using System.Linq;

namespace PlantUmlLanguageService.Tests
{
    public class SystemLocatorHelper
    {
        public static string GetJavaPath()
        {
            string javaHome = Environment.GetEnvironmentVariable("JAVA_HOME") ?? string.Empty;
            var javaInJavaHome = Path.Combine(javaHome, "bin", "java.exe");
            if (!string.IsNullOrEmpty(javaHome) && File.Exists(javaInJavaHome))
            {
                return javaInJavaHome;
            }
            return FindInPath("java.exe");
        }

        public static string GetPlantUmlPath()
        {
            string plantUmlJar = Environment.GetEnvironmentVariable("PLANTUML_JAR") 
                                 ?? Environment.GetEnvironmentVariable("PLANTUML")
                                 ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "chocolatey", "lib","plantuml","tools", "plantuml.jar")
                ;
            if (!string.IsNullOrEmpty(plantUmlJar) && File.Exists(plantUmlJar))
            {
                return plantUmlJar;
            }
            return FindInPath("plantuml.jar");
        }

        private static string FindInPath(string fileName)
        {
            var pathEnv = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            return pathEnv.Split(';')
                .Select(path => Path.Combine(path, fileName))
                .FirstOrDefault(File.Exists);
        }
    }
}