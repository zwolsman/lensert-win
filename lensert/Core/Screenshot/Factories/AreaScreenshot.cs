using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lensert.Core.Screenshot.Factories
{
    internal abstract class AreaScreenshot : ScreenshotFactory
    {
        protected readonly SelectionForm SelectionForm;

        protected AreaScreenshot()
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

        public override Image TakeScreenshot()
        {
            if (SelectionForm.Visible)
                return null;

            var screenshot = new Bitmap(Create<FullScreenshot>());
            SelectionForm.Screenshot = screenshot;
            SelectionForm.ShowDialog();

            return screenshot.Clone(SelectionForm.SelectedArea, screenshot.PixelFormat);
        }
    }
}
