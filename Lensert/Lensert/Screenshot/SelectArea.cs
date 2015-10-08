using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lensert.Screenshot
{
    class SelectArea : AbstractArea
    {
        private Point _drawStart;

        public SelectArea()
        {
            _selectionForm.MouseDown += SelectionForm_MouseDown;
            _selectionForm.MouseMove += SelectionForm_MouseMove;
        }

        private void SelectionForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            _drawStart = e.Location;
        }

        private void SelectionForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            var selectionRectangle = new Rectangle(Math.Min(_drawStart.X, e.X),      //calculates selection with the begin- and endpoints
                                                   Math.Min(_drawStart.Y, e.Y),
                                                   Math.Abs(_drawStart.X - e.X),
                                                   Math.Abs(_drawStart.Y - e.Y));

            _selectionForm.SelectedArea = selectionRectangle;
        }
    }
}
