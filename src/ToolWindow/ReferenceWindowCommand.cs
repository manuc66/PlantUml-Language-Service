using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using PlantUmlLanguageService.ToolWindow;
using Task = System.Threading.Tasks.Task;

namespace PlantUmlLanguageService
{
    internal static class ReferenceWindowsCommand
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("520eea4d-2604-4bee-bd8d-9dc202d4d107");

        public static async Task InitializeAsync(AsyncPackage package)
        {
            var commandService = (IMenuCommandService)await package.GetServiceAsync(typeof(IMenuCommandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand((sender, e) => Execute(package, sender, e), menuCommandID);
            commandService.AddCommand(menuItem);
        }

        private static void Execute(AsyncPackage package, object sender, EventArgs e)
        {
            package.JoinableTaskFactory.RunAsync(async () =>
            {
                ToolWindowPane window = await package.ShowToolWindowAsync(
                    typeof(ReferenceWindow),
                    0,
                    create: true,
                    cancellationToken: package.DisposalToken);
            });
        }
    }
}
