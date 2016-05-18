using System.Drawing;

namespace Lensert.Screenshot
{
    internal abstract class AbstractScreenshotTemplate
    {
        public bool SpecialKeyPressed { get; protected set; }
        protected abstract Rectangle GetArea();

        public virtual Image TakeScreenshot()
        {
            var area = GetArea();
            return NativeHelper.TakeScreenshot(area);
        }
    }
}