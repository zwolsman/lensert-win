using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lensert.Screenshot
{
    abstract class AbstractArea : IScreenshot
    {
        protected readonly SelectionForm _selectionForm;

        public AbstractArea()
        {
            _selectionForm = new SelectionForm();
            _selectionForm.MouseUp += SelectionForm_MouseUp;
        }
        
        private void SelectionForm_MouseUp(object sender, MouseEventArgs e)
        {
            _selectionForm.Close();
        }
        
        public virtual Rectangle GetArea()
        {
            if (_selectionForm.Visible)
                return Rectangle.Empty;

            var screenshot = ScreenshotFactory.Create<FullScreen>();
            _selectionForm.Screenshot = screenshot;
            _selectionForm.ShowDialog();
            return _selectionForm.SelectedArea; 
        }
    }
}
