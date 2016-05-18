using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lensert.Screenshot
{
    internal sealed class FullScreenTemplate : AbstractScreenshotTemplate
    {
        protected override Rectangle GetArea()
            => NativeHelper.UnscaledBounds;
    }
}