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
            var selectedRectangle = _rectangles.FirstOrDefault(r => r.Contains(e.Location));
            if (selectedRectangle == default(Rectangle))
                return;

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
