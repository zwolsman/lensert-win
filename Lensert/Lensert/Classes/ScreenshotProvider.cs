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
        private static readonly SelectionForm _selectionForm;
        private static readonly SelectionForm _windowForm;

        private static Point _drawStart, _drawEnd;
        private static IEnumerable<Rectangle> _rectangles;

        static ScreenshotProvider()
        {
            _selectionForm = new SelectionForm();
            _selectionForm.MouseDown += SelectionForm_MouseDown;
            _selectionForm.MouseMove += SelectionForm_MouseMove;

            _windowForm = new SelectionForm();
            _windowForm.MouseMove += WindowForm_MouseMove;
        }

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
                case ScreenshotType.Fullscreen:
                    return SystemInformation.VirtualScreen;

                case ScreenshotType.CurrentWindow:
                    return NativeHelper.GetForegroundWindowAea();

                case ScreenshotType.SelectWindow:
                    return SelectWindowArea();

                case ScreenshotType.Area:
                    return SelectArea();
            }

            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        private static Rectangle SelectArea()
        {
            if (_selectionForm.Visible)
                return Rectangle.Empty;

            _selectionForm.ShowDialog();
            return _selectionForm.SelectedArea;
        }

        private static Rectangle SelectWindowArea()
        {
            if (_windowForm.Visible)
                return Rectangle.Empty;

            _rectangles = NativeHelper.GetWindowDimensions();
            _windowForm.testRectangles = _rectangles;
            _windowForm.ShowDialog();
            
            return _windowForm.SelectedArea;
        }

        private static void SelectionForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            _drawStart = e.Location;
        }

        private static void SelectionForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            _drawEnd = e.Location;

            _selectionForm.SelectedArea = new Rectangle(Math.Min(_drawStart.X, _drawEnd.X),
                                                        Math.Min(_drawStart.Y, _drawEnd.Y),
                                                        Math.Abs(_drawStart.X - _drawEnd.X),
                                                        Math.Abs(_drawStart.Y - _drawEnd.Y));

            _selectionForm.Invalidate();
        }

        private static void WindowForm_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle selectedRectangle = default(Rectangle);// = _rectangles.FirstOrDefault(r => r.Contains(e.Location));

            foreach (var rectangle in _rectangles)
            {
                if (rectangle.Contains(e.Location))
                {
                    selectedRectangle = rectangle;
                    break;
                }
            }

            if (selectedRectangle == default(Rectangle))
                return;

            Console.WriteLine(e.Location);
            _windowForm.SelectedArea = selectedRectangle;
            _windowForm.Invalidate();
        }
    }
}
