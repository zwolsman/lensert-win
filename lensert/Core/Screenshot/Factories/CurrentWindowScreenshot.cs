using System.Drawing;
using Lensert.Helpers;

namespace Lensert.Core.Screenshot.Factories
{
    internal class CurrentWindowScreenshot : ScreenshotFactory
    {
        public override Image TakeScreenshot()
        {
            var area = Native.GetForegroundWindowAea();
            if (!Native.UnscaledBounds.Contains(area))
                return null;

            using (var screenshot = new Bitmap(Create<FullScreenshot>()))
                return screenshot.Clone(area, screenshot.PixelFormat);
        }
    }
}
