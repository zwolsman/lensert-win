using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Lensert.Core.Screenshot.Factories
{
    internal sealed class ClipboardScreenshot : ScreenshotFactory
    {
        public override Image TakeScreenshot()
        {
            var image = ScreenshotImpl();
            if (image != null)
                return image;

            NotificationProvider.Show(
                "Clipboard Error",
                "Make sure you have a bitmap image or a single file in your clipboard.");

            return null;
        }

        private static Image ScreenshotImpl()
        {
            try
            {
                var clipboardData = Clipboard.GetDataObject();
                if (clipboardData == null)
                    return null;

                if (clipboardData.GetDataPresent(DataFormats.Bitmap, true))
                    return (Image) clipboardData.GetData(DataFormats.Bitmap);

                if (clipboardData.GetDataPresent(DataFormats.FileDrop, true))
                {
                    var fileList = (string[]) clipboardData.GetData(DataFormats.FileDrop);
                    return fileList.Length != 1
                        ? null
                        : Image.FromFile(fileList[0]);
                }
            }
            catch (ExternalException)
            {
                NotificationProvider.Show(
                    "Clipboard Error",
                    "Lensert was unable to read current clipboard content.");
            }

            return null;
        }
    }
}
