using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Lensert
{
    static class ScreenshotProvider
    {
        public static Image GetScreenshot(ScreenshotType type)
        {
            return DummyImageProvider.Next();
        }

        private static Rectangle GetArea(ScreenshotType type)
        {
            switch (type)
            {
                case ScreenshotType.CurrentWindow:
                    return NativeHelper.GetForegroundWindowAea();
                case ScreenshotType.Area:
                    break;
                case ScreenshotType.Fullscreen:
                    return SystemInformation.VirtualScreen;
            }

            throw new ArgumentOutOfRangeException(nameof(type), type, null);

        }
    }
}
