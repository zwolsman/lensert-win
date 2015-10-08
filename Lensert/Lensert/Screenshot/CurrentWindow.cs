using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lensert.Screenshot
{
    class CurrentWindow : ScreenshotFactory
    {
        protected override Rectangle GetArea()
            => NativeHelper.GetForegroundWindowAea();
    }
}
