using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lensert.Helpers;
using NLog;

namespace Lensert.Core.ScreenshotHandler
{
    internal abstract class BaseImageUploader : IScreenshotHandler
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        
        public async Task<string> HandleAsync(Image screenshot)
        {
            if (screenshot == null)
                throw new ArgumentNullException(nameof(screenshot));

            string link = null;

            try
            {
                // upload to the server
                link = await UploadAsync(screenshot);
                if (string.IsNullOrEmpty(link))
                {
                    _logger.Error("UploadImageAsync did not return a valid link");
                    NotificationProvider.Show("Upload failed", "Uploading the screenshot failed", LogFile.Open);
                }
                else
                {
                    _logger.Info($"Image uploaded {link}");
                    NotificationProvider.Show("Upload complete", link, () => Process.Start(link), -1); // priority: -1 -> always get overwritten even by itself (spamming lensert e.g.)
                    Clipboard.SetDataObject(link, true, 10, 200);
                }

                return link;
            }
            catch (ExternalException)
            {
                if (!string.IsNullOrEmpty(link))
                {
                    NotificationProvider.Show(
                        "Clipboard Error",
                        "Lensert failed to copy the link to the clipboard",
                        () => Process.Start(link),
                        -1);
                }
            }
            catch (HttpRequestException)
            {
                return null;
            }

            return link;
        }

        protected abstract Task<string> UploadAsync(Image screenshot);
    }
}
