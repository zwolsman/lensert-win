using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lensert.Core.Screenshot.Factories
{
    internal sealed class UserSelectionScreenshot : AreaScreenshot
    {
        private Point _drawStart;

        public UserSelectionScreenshot()
        {
            SelectionForm.MouseDown += SelectionForm_MouseDown;
            SelectionForm.MouseMove += SelectionForm_MouseMove;
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

            var selectionRectangle = new Rectangle(Math.Min(_drawStart.X, e.X), //calculates selection with the begin- and endpoints
                Math.Min(_drawStart.Y, e.Y),
                Math.Abs(_drawStart.X - e.X),
                Math.Abs(_drawStart.Y - e.Y));

            SelectionForm.SelectedArea = selectionRectangle;
        }
    }
}
