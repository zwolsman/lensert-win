using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lensert.Core.ScreenshotHandler
{
    internal sealed class ClipboardHandler : IScreenshotHandler
    {
        public Task<string> HandleAsync(Image screenshot)
        {
            if (screenshot == null)
                throw new ArgumentNullException(nameof(screenshot));

            try
            {
                Clipboard.SetDataObject(screenshot, true, 10, 200);
                NotificationProvider.Show("Clipboard complete", "Image succesfully saved to clipboard.");

                return Task.FromResult("clipboard");
            }
            catch (ExternalException)
            {
                NotificationProvider.Show(
                    "Clipboard Error",
                    "Lensert was unable to write image to the clipboard.");
            }

            return null;
        }
    }
}
