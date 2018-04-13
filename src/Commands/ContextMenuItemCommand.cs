using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;

namespace PlantUmlLanguageService.Commands
{
    internal sealed class ContextMenuItemCommand
    {
        public const int FileContextMenuCommandId = 0x0100;

        public static readonly Guid CommandSet = new Guid("b394839a-d886-44d2-94c9-ffeeb48d97d5");

        private readonly Package package;

        private ContextMenuItemCommand(Package package)
        {
            this.package = package ?? throw new ArgumentNullException("package");

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, FileContextMenuCommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        public static ContextMenuItemCommand Instance
        {
            get;
            private set;
        }

        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        public static void Initialize(Package package)
        {
            Instance = new ContextMenuItemCommand(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            EnvDTE.DTE dte;
            EnvDTE.SelectedItems selectedItems;
            EnvDTE.ProjectItem projectItem;
            EnvDTE.Solution solution;
            dte = (EnvDTE.DTE)ServiceProvider.GetService(typeof(EnvDTE.DTE));
            selectedItems = dte.SelectedItems;
            solution = dte.Solution;

            if (selectedItems != null)
            {
                foreach (EnvDTE.SelectedItem selectedItem in selectedItems)
                {
                    projectItem = selectedItem.ProjectItem as EnvDTE.ProjectItem;

                    if (projectItem != null)
                    {
                        projectItem.Save();
                        ServiceProvider.PreviewFileContent(projectItem.FileNames[1]);
                    }
                }
            }

        }

    }
}