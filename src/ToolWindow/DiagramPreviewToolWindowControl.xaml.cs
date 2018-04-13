namespace PlantUmlLanguageService.ToolWindow
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for DiagramPreviewToolWindowControl.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.UserControl" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class DiagramPreviewToolWindowControl : UserControl
    {
        private string DiagramUrl = string.Empty;
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramPreviewToolWindowControl" /> class.
        /// </summary>
        public DiagramPreviewToolWindowControl()
        {
            InitializeComponent();
            PreviewHost.Child = Controls.PumlViewer;
        }

        /// <summary>
        /// Previews the current diagram instance.
        /// </summary>
        public void Preview()
        {
            DiagramUrl = Global.DiagramUrl;
            ToolTip = Global.CurrentFile;
            Console.WriteLine($"Diagramming Ready: {Controls.PumlViewer.RenderAsync().IsCompleted}");
        }

        /// <summary>
        /// Handles the Loaded event of the PlantUmlToolWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void PlantUmlToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
           Preview();
        }

        /// <summary>
        /// Handles the IsVisibleChanged event of the PlantUmlToolWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void PlantUmlToolWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                Preview();
            }
        }

        /// <summary>
        /// Handles the GotFocus event of the PlantUmlToolWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void PlantUmlToolWindow_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DiagramUrl != Global.DiagramUrl)
            {
                Preview();
            }
        }

    }
}