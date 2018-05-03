using System;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.VisualStudio.Shell;

namespace PlantUmlLanguageService.Commands
{
    internal sealed class ContextMenuItemCommand
    {
        public const int FileContextMenuCommandId = 0x0100;

        public static readonly Guid CommandSet = new Guid("b394839a-d886-44d2-94c9-ffeeb48d97d5");

        private readonly Package package;

        private OleMenuCommand OlemenuItem;

        private ContextMenuItemCommand(Package package)
        {
            this.package = package ?? throw new ArgumentNullException("package");

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, FileContextMenuCommandId);
                OlemenuItem = new OleMenuCommand(this.MenuItemCallback, menuCommandID)
                {
                    Visible = false
                };
                OlemenuItem.BeforeQueryStatus += EnableForPlantUmlMenuItems;
                commandService.AddCommand(OlemenuItem);
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

        private void EnableForPlantUmlMenuItems(object sender, EventArgs e)
        {
            EnvDTE.DTE dte = (EnvDTE.DTE)ServiceProvider.GetService(typeof(EnvDTE.DTE));
            EnvDTE.SelectedItems selectedItems = dte.SelectedItems;

            if (selectedItems != null)
            {
                foreach (EnvDTE.SelectedItem selectedItem in selectedItems)
                {
                    EnvDTE.ProjectItem projectItem = selectedItem.ProjectItem as EnvDTE.ProjectItem;

                    if (projectItem != null && Constants.FileTypes.Contains($".{projectItem.FileNames[1].Split('.').Last()}"))
                    {
                        OlemenuItem.Visible = true;
                    }
                    else
                    {
                        OlemenuItem.Visible = false;
                    }
                }
            }

        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            EnvDTE.DTE dte;
            EnvDTE.SelectedItems selectedItems;
            EnvDTE.ProjectItem projectItem;
            dte = (EnvDTE.DTE)ServiceProvider.GetService(typeof(EnvDTE.DTE));
            selectedItems = dte.SelectedItems;

            if (selectedItems != null)
            {
                foreach (EnvDTE.SelectedItem selectedItem in selectedItems)
                {
                    projectItem = selectedItem.ProjectItem as EnvDTE.ProjectItem;

                    if (projectItem != null)
                    {
                        try
                        {
                            projectItem.Save();
                        }
                        catch { }
                        ServiceProvider.PreviewFileContent(projectItem.FileNames[1]);
                    }
                }
            }

        }

    }
}