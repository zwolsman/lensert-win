using System.Drawing;
using Lensert.Helpers;

namespace Lensert.Core.Screenshot.Factories
{
    internal sealed class FullScreenshot : ScreenshotFactory
    {
        protected override Rectangle GetArea()
            => Native.UnscaledBounds;
    }
}
