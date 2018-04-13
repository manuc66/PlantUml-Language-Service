using System;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using PlantUmlLanguageService.Services;

namespace PlantUmlLanguageService.Commands
{
    internal sealed class ContextMenuCodeWindowCommand
    {
        public const int CodeWindowContextMenuCommandId = 0x0100;

        public static readonly Guid CommandSet = new Guid("0c1acc31-15ac-417c-86b2-eefdc669e8bf");

        private readonly Package package;

        private ContextMenuCodeWindowCommand(Package package)
        {
            this.package = package ?? throw new ArgumentNullException("package");

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CodeWindowContextMenuCommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        public static ContextMenuCodeWindowCommand Instance
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
            Instance = new ContextMenuCodeWindowCommand(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            EnvDTE.DTE dte;
            EnvDTE.Document activeDocument;

            dte = (EnvDTE.DTE)ServiceProvider.GetService(typeof(EnvDTE.DTE));
            activeDocument = dte.ActiveDocument;
            activeDocument.Save();
            ServiceProvider.PreviewFileContent(activeDocument.FullName);
        }
    }
}