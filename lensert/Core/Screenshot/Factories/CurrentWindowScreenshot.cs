using System.Drawing;
using Lensert.Helpers;

namespace Lensert.Core.Screenshot.Factories
{
    internal class CurrentWindowScreenshot : ScreenshotFactory
    {
        public override Image TakeScreenshot()
        {
            var screenshot = new Bitmap(Create<FullScreenshot>());
            return screenshot.Clone(Native.GetForegroundWindowAea(), screenshot.PixelFormat);
        }
    }
}
