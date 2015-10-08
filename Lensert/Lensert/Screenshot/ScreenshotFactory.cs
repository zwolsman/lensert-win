using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lensert.Screenshot
{
    abstract class ScreenshotFactory
    {
        protected abstract Rectangle GetArea();

        public virtual Image TakeScreenshot()
        {
            var area = GetArea();
            return NativeHelper.TakeScreenshot(area);
        }
    }
}
