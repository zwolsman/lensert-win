using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lensert
{
    static class ScreenshotProvider
    {
        public static Image GetScreenshot(ScreenshotType type)
        {
            var rectangle = GetArea(type);
        }

        private static Rectangle GetArea(ScreenshotType type)
        {
            switch (type)
            {
                case ScreenshotType.Fullscreen:
                    return SystemInformation.VirtualScreen;
                case ScreenshotType.Window:
                    return NativeHelper.GetForegroundWindowAea();
                case ScreenshotType.Area:
                    break;
            }

            throw new InvalidOperationException();
        }
    }
}
