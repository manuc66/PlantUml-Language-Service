using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace PlantUmlLanguageService.Services
{
    public static class SolutionService
    {
        public static DTE2 GetActiveIDE()
        {
            // Get an instance of currently running Visual Studio IDE.
            DTE2 dte2 = Package.GetGlobalService(typeof(DTE)) as DTE2;
            return dte2;
        }

        public static Solution  GetActiveSolution() {
            return GetActiveIDE().Solution;
        }


        public static DirectoryInfo GetActiveSolutionInfo()
        {
            return new DirectoryInfo(Path.GetDirectoryName(GetActiveSolution().FullName));
        }


        public static List<FileInfo> GetFileInfosForActiveSolution()
        {
            List<FileInfo> Files = new List<FileInfo>();
            Files.AddRange(GetFilesFrom(GetActiveSolutionInfo()));
            Files.ForEach(file => Debug.WriteLine(file.FullName));
            return Files;
        }

        private static List<FileInfo> GetFilesFrom(DirectoryInfo directory)
        {
            List<FileInfo> Files = new List<FileInfo>();
            Files.AddRange(directory.GetFiles());
            foreach (DirectoryInfo subDirectory in directory.GetDirectories()) {
                Files.AddRange(GetFilesFrom(subDirectory));

            } 
            return Files;
        }

        public static IList<Project> Projects()
        {
            Projects projects = GetActiveSolution().Projects;
            List<Project> list = new List<Project>();
            var item = projects.GetEnumerator();
            while (item.MoveNext())
            {
                var project = item.Current as Project;
                if (project == null)
                {
                    continue;
                }

                if (project.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")
                {
                    list.AddRange(GetSolutionFolderProjects(project));
                }
                else
                {
                    list.Add(project);
                }
            }

            return list;
        }

        private static IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder)
        {
            List<Project> list = new List<Project>();
            for (var i = 1; i <= solutionFolder.ProjectItems.Count; i++)
            {
                var subProject = solutionFolder.ProjectItems.Item(i).SubProject;
                if (subProject == null)
                {
                    continue;
                }

                // If this is another solution folder, do a recursive call, otherwise add
                if (subProject.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")
                {
                    list.AddRange(GetSolutionFolderProjects(subProject));
                }
                else
                {
                    list.Add(subProject);
                }
            }
            return list;
        }
    }
}
