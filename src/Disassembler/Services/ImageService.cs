using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using static PlantUmlLanguageService.Disassembler.Globals;

namespace PlantUmlLanguageService.Disassembler.Services
{
    public class ImageService
    {
        /// <summary>
        /// Gets or sets the temporary SVG.
        /// </summary>
        /// <value>
        /// The temporary SVG.
        /// </value>
        public static string TempSvg { get; set; } = Path.GetTempPath() + "temp_image.svg";
        /// <summary>
        /// Saves the imagefrom URL to disk.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="format">The format.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string SaveImagefromUrlToDisk(string url, ImageRequestFormat format, string path)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(url);
                string pathformat = "{0}.{1}";
                ImageFormat _imgFormat = ImageFormat.Bmp;

                using (MemoryStream mem = new MemoryStream(data))
                {
                    using (var _img = Image.FromStream(mem))
                    {

                        switch (format)
                        {
                            case ImageRequestFormat.Png:
                                path = string.Format(pathformat, path, "png");
                                _imgFormat = ImageFormat.Png;
                                break;
                            case ImageRequestFormat.Jpg:
                                path = string.Format(pathformat, path, "jpg");
                                _imgFormat = ImageFormat.Jpeg;

                                break;
                        }

                        _img.Save(path, _imgFormat);

                    }
                }
                return path;

            }

        }

        /// <summary>
        /// Copies the temporary image from URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string CopyTempImageFromUrl(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(url);
                using (var _stream = new MemoryStream(data))
                {
                    using (FileStream _file = new FileStream(TempSvg, FileMode.Create, FileAccess.Write))
                    {
                        _stream.WriteTo(_file);

                    }
                }

                return TempSvg;

            }

        }

    }

}
