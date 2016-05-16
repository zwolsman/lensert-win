using System.Drawing;

namespace Lensert.Screenshot
{
    internal abstract class AbstractScreenshotTemplate
    {
        protected abstract Rectangle GetArea();

        public virtual Image TakeScreenshot()
        {
            var area = GetArea();
            return NativeHelper.TakeScreenshot(area);
        }
    }
}