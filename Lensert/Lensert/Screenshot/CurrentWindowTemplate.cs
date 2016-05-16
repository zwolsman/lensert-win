using System.Drawing;

namespace Lensert.Screenshot
{
    internal class CurrentWindowTemplate : AbstractScreenshotTemplate
    {
        protected override Rectangle GetArea()
            => NativeHelper.GetForegroundWindowAea();
    }
}