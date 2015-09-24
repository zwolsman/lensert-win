using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lensert.Forms
{
    public partial class SelectionForm : Form
    {
        private const int DIMENSION_TEXT_OFFSET = 2; //TODO: Refactor into settings

        private SolidBrush _backgroundBrush, _rectangleBrush, _textBrush;
        private Pen _rectanglePen;
        private Point _startPoint, _endPoint;

        public SelectionForm()
        {
            InitializeComponent();

            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            Bounds = SystemInformation.VirtualScreen;
            // BackgroundImage = ScreenshotProvider.GetScreenshot(ScreenshotType.Fullscreen);

            TransparencyKey = Color.AliceBlue;

            _startPoint = Point.Empty;
            _endPoint = Point.Empty;
        }

        public Rectangle SelectedArea() => new Rectangle(
            Math.Min(_startPoint.X, _endPoint.X),
            Math.Min(_startPoint.Y, _endPoint.Y),
            Math.Abs(_startPoint.X - _endPoint.X),
            Math.Abs(_startPoint.Y - _endPoint.Y));

        Rectangle rect;

        private void SelectionForm_Load(object sender, EventArgs e)
        {
            _backgroundBrush?.Dispose();
            _rectangleBrush?.Dispose();

            _textBrush = new SolidBrush(ForeColor);
            _backgroundBrush = new SolidBrush(Color.FromArgb(100, Preferences.Default.SelectionBackgroundColor));
            _rectangleBrush = new SolidBrush(Preferences.Default.SelectionRectangleColor);
            _rectanglePen = new Pen(Preferences.Default.SelectionRectangleColor);
        }

        private void SelectionForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            _startPoint = e.Location;
        }

        private void SelectionForm_MouseUp(object sender, MouseEventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void SelectionForm_KeyDown(object sender, KeyEventArgs e)
        {
            //TODO shit
            Close();
        }


        private void SelectionForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            _endPoint = e.Location;
            Invalidate();
        }

        Rectangle oldRect;
        Stopwatch sw = Stopwatch.StartNew();
        SolidBrush b = new SolidBrush(Color.AliceBlue);
        private void SelectionForm_Paint(object sender, PaintEventArgs e)
        {
            var selection = SelectedArea();
            if (selection == Rectangle.Empty)
                return;

            sw.Restart();
            
            e.Graphics.FillRectangle(b, selection);
            e.Graphics.DrawRectangle(_rectanglePen, selection);                  //Draw the border

            var dimension = $"{selection.Width}x{selection.Height}";
            var size = e.Graphics.MeasureString(dimension, Font);

            float y = selection.Y + selection.Height + DIMENSION_TEXT_OFFSET;    //spaces the dimension text above the selection box
            if (Height < y + size.Height)                                        //checks if it goes out of screen
                y -= DIMENSION_TEXT_OFFSET * 2 - size.Height;                    //soo set the top in the selectionbox itself

            float x = selection.X + selection.Width - size.Width;                //calculates the x_pos of the dimension 

            e.Graphics.DrawString(dimension, Font, _textBrush, x, y);


            Console.WriteLine(sw.ElapsedTicks);
        }
    }
}