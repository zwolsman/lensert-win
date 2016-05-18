using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Lensert.Screenshot
{
    internal sealed class SelectWindowTemplate : AbstractAreaTemplate
    {
        private IEnumerable<Rectangle> _rectangles;

        public SelectWindowTemplate()
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

        protected override Rectangle GetArea()
        {
            _rectangles = NativeHelper.GetWindowDimensions();
            return base.GetArea();
        }
    }
}