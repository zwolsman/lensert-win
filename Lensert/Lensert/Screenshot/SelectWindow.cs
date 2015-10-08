using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lensert.Screenshot
{
    class SelectWindow : AbstractArea
    {
        private IEnumerable<Rectangle> _rectangles;

        public SelectWindow()
        {
            _selectionForm.MouseMove += SelectionForm_MouseMove; 
        }

        private void SelectionForm_MouseMove(object sender, MouseEventArgs e)
        {
            var selectedRectangle = _rectangles.FirstOrDefault(r => r.Contains(e.Location));
            if (selectedRectangle == default(Rectangle))
                return;

            _selectionForm.SelectedArea = selectedRectangle;
        }

        public override Rectangle GetArea()
        {
            _rectangles = NativeHelper.GetWindowDimensions();
            return base.GetArea();
        }
    }
}
