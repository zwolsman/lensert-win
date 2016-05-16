using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lensert.Screenshot
{
    internal sealed class FullScreenTemplate : AbstractScreenshotTemplate
    {
        protected override Rectangle GetArea()
        {
            var factor = NativeHelper.GetScalingFactor();
            var virtualScreen = SystemInformation.VirtualScreen;

            var width = (int)Math.Ceiling(factor * virtualScreen.Width);
            var height = (int)Math.Ceiling(factor * virtualScreen.Height);
            
            return new Rectangle(Point.Empty, new Size(width, height));
        }
    }
}