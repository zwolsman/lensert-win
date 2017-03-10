using System.Drawing;
using Lensert.Helpers;

namespace Lensert.Core.Screenshot.Factories
{
    internal sealed class FullScreenshot : ScreenshotFactory
    {
        public override Image TakeScreenshot()
            => Native.TakeScreenshot(Native.UnscaledBounds);
    }
}
