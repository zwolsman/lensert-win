using System;
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
            SelectionForm.KeyUp += SelectionFormOnKeyUp;
            SelectionForm.Load += SelectionFormOnLoad;
        }

        private void SelectionFormOnLoad(object sender, EventArgs eventArgs)
        {
            SpecialKeyPressed = false;
        }

        private void SelectionFormOnKeyUp(object sender, KeyEventArgs e)
        {
            SpecialKeyPressed = e.KeyCode == Keys.Space;

            if (SpecialKeyPressed)
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