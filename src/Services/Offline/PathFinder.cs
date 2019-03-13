using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlantUmlLanguageService.Services.Offline
{
    internal static class PathFinder
    {
        public static FindContext LookupFile(string fileName)
        {
            return new FindContext(fileName);
        }

        public class FindContext
        {
            private readonly string _fileName;

            public FindContext(string fileName)
            {
                if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(fileName));
                _fileName = fileName;
            }

            public string InPath()
            {
                if (File.Exists(_fileName))
                {
                    return Path.GetFullPath(_fileName);
                }

                var values = Environment.GetEnvironmentVariable("PATH");

                return values?.Split(';')
                    .Select(path => Path.Combine(path, _fileName))
                    .Where(System.IO.File.Exists)
                    .DefaultIfEmpty(string.Empty)
                    .First();

            }

            public string BeneathEnv(string envVar)
            {
                var path = Environment.GetEnvironmentVariable(envVar);
                if (string.IsNullOrEmpty(path))
                {
                    return string.Empty;
                }

                return SafeGetFiles(path, _fileName)
                    .DefaultIfEmpty(string.Empty)
                    .First();
            }

            public string BeneathProgramFiles(string pattern)
            {
                var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                var programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

                return new[] {programFiles, programFilesX86}
                    .Where(path => !string.IsNullOrEmpty(path))
                    .Select(path => FindIn(path, pattern, _fileName))
                    .DefaultIfEmpty(string.Empty)
                    .First();
            }

            private static string FindIn(string rootPath, string pathPattern, string fileName)
            {
                foreach (var dir in SafeGetDirectories(rootPath, pathPattern))
                {
                    var files = SafeGetFiles(fileName, dir, SearchOption.AllDirectories);

                    var findResult = files.FirstOrDefault();
                    if (!string.IsNullOrEmpty(findResult))
                    {
                        return findResult;
                    }
                }

                return string.Empty;
            }

            private static IEnumerable<string> SafeGetFiles(string fileName, string dir, SearchOption searchOption = SearchOption.TopDirectoryOnly)
            {
                try
                {
                    return Directory.GetFiles(dir, fileName, searchOption);
                }
                catch (Exception e)
                    when (e is ArgumentException
                          || e is UnauthorizedAccessException
                          || e is IOException
                    )
                {
                   return Enumerable.Empty<string>();
                }
            }

            private static IEnumerable<string> SafeGetDirectories(string rootPath, string pathPattern)
            {
                try
                {
                    return Directory.GetDirectories(rootPath, pathPattern);
                }
                catch (Exception e)
                    when (e is ArgumentException
                          || e is UnauthorizedAccessException
                          || e is IOException
                    )
                {
                    return Enumerable.Empty<string>();
                }
            }
        }
    }
}