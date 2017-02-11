using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Lensert.Helpers;

namespace Lensert.Core.Screenshot.Factories
{
    internal sealed class SelectWindowScreenshot : AreaScreenshot
    {
        private IEnumerable<Rectangle> _rectangles;

        public SelectWindowScreenshot()
        {
            SelectionForm.MouseMove += SelectionForm_MouseMove;
        }

        private void SelectionForm_MouseMove(object sender, MouseEventArgs e)
        {
            var x = e.X;
            var y = e.Y;

            // take account for monitors on the left/top
            if (Native.UnscaledBounds.X < 0)
                x += Native.UnscaledBounds.X;
            if (Native.UnscaledBounds.Y < 0)
                y += Native.UnscaledBounds.Y;

            var location = new Point(x, y);
            var selectedRectangle = _rectangles.FirstOrDefault(r => r.Contains(location));
            if (selectedRectangle == default(Rectangle))
                return;

            // at painting, the left/top monitor is the 0,0 so again take account for that
            if (Native.UnscaledBounds.X < 0)
                selectedRectangle.X -= Native.UnscaledBounds.X;
            if (Native.UnscaledBounds.Y < 0)
                selectedRectangle.Y -= Native.UnscaledBounds.Y;

            SelectionForm.SelectedArea = selectedRectangle;
        }

        public override Image TakeScreenshot()
        {
            return base.TakeScreenshot();
        }

        protected override Rectangle GetArea()
        {
            _rectangles = Native.GetWindowDimensions();
            return base.GetArea();
        }
    }
}
