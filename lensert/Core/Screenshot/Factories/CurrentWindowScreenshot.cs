using System.Drawing;
using Lensert.Helpers;

namespace Lensert.Core.Screenshot.Factories
{
    internal class CurrentWindowScreenshot : ScreenshotFactory
    {
        protected override Rectangle GetArea()
            => Native.GetForegroundWindowAea();
    }
}
