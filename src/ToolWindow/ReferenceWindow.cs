using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms.Integration;
using Microsoft.VisualStudio.Shell;
using PdfiumViewer;

namespace PlantUmlLanguageService.ToolWindow
{
    [Guid(WindowGuidString)]
    public class ReferenceWindow : ToolWindowPane
    {
        public const string WindowGuidString = "e7090fd8-8163-4e9a-9616-45ff87e0816e";
        public const string Title = "PlantUml Language Reference";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceWindow"/> class.
        /// </summary>
        public ReferenceWindow(string title) : base()
        {
            Caption = title;
            WebClient client = new WebClient();
            Byte[] FileBuffer = client.DownloadData("http://plantuml.com/PlantUML_Language_Reference_Guide.pdf");
            var ControlHost = new WindowsFormsHost();
            if (FileBuffer != null)
            {
                var pdfDocument = PdfDocument.Load((new MemoryStream(FileBuffer)));
                var viewer = new PdfViewer
                {
                    Document = pdfDocument,
                    ZoomMode = PdfViewerZoomMode.FitWidth
                };
                ControlHost.Child = viewer;

            }
            Content = ControlHost;

        }
    }
}
