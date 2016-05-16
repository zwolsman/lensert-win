using System.Drawing;
using System.Windows.Forms;

namespace Lensert.Screenshot
{
    internal abstract class AbstractAreaTemplate : AbstractScreenshotTemplate
    {
        protected readonly SelectionForm SelectionForm;

        protected AbstractAreaTemplate()
        {
            SelectionForm = new SelectionForm();
            SelectionForm.MouseUp += SelectionForm_MouseUp;
        }

        private void SelectionForm_MouseUp(object sender, MouseEventArgs e)
        {
            SelectionForm.Close();
        }

        protected override Rectangle GetArea()
        {
            if (SelectionForm.Visible)
                return Rectangle.Empty;

            var screenshot = ScreenshotFactory.Create<FullScreenTemplate>();
            SelectionForm.Screenshot = screenshot;
            SelectionForm.ShowDialog();
            return SelectionForm.SelectedArea;
        }
    }
}