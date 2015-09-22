using Lensert.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Lensert
{
    static class ScreenshotProvider
    {
        private static readonly SelectionForm _selectionForm = new SelectionForm();

        public static Image GetScreenshot(ScreenshotType type)
        {
            var area = GetArea(type);
            if (area == Rectangle.Empty)
                return null;

            var image = NativeHelper.TakeScreenshot(area);

            return image;
        }

        private static Rectangle GetArea(ScreenshotType type)
        {
            switch (type)
            {
                case ScreenshotType.CurrentWindow:
                    return NativeHelper.GetForegroundWindowAea();
                case ScreenshotType.Area:
                    _selectionForm.ShowDialog();
                    return _selectionForm.SelectedArea();
                case ScreenshotType.Fullscreen:
                    return SystemInformation.VirtualScreen;
            }

            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
