using System.Drawing;
using System.Windows.Forms;

namespace Lensert.Screenshot
{
    abstract class AbstractArea : ScreenshotFactory
    {
        protected readonly SelectionForm _selectionForm;

        protected AbstractArea()
        {
            _selectionForm = new SelectionForm();
            _selectionForm.MouseUp += SelectionForm_MouseUp;
        }
        
        private void SelectionForm_MouseUp(object sender, MouseEventArgs e)
        {
            _selectionForm.Close();
        }
        
        protected override Rectangle GetArea()
        {
            if (_selectionForm.Visible)
                return Rectangle.Empty;

            var screenshot = ScreenshotProvider.Create<FullScreen>();
            _selectionForm.Screenshot = screenshot;
            _selectionForm.ShowDialog();
            return _selectionForm.SelectedArea; 
        }
    }
}
