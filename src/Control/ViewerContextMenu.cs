using System;
using System.Windows.Forms;

namespace PlantUmlLanguageService.Control
{
    class ViewerContextMenu: ContextMenuStrip
    {
        ToolStripMenuItem GetMarkDown = new ToolStripMenuItem() { Text = "Copy Markdown to clipboard" };
        ToolStripMenuItem GetMarkUp = new ToolStripMenuItem() { Text = "Copy Html to clipboard" };

        public ViewerContextMenu() : base()
        {
            GetMarkDown.Click += new EventHandler(GetMarkdownText);
            GetMarkUp.Click += new EventHandler(GetMarkupText);

            this.Items.Add(GetMarkDown);
            this.Items.Add(GetMarkUp);
        }

        protected void GetMarkdownText(object sender, EventArgs e)
        {
            Clipboard.SetText(string.Format(Constants.UrlFormatMd,Global.DiagramUrl,Global.CurrentFile), TextDataFormat.UnicodeText);
        }

        protected void GetMarkupText(object sender, EventArgs e)
        {
            Clipboard.SetText(string.Format(Constants.UrlFormatSrc, Global.DiagramUrl, Global.CurrentFile), TextDataFormat.UnicodeText);
        }

    }
}
