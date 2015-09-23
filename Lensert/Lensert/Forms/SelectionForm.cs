using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            BackgroundImage = ScreenshotProvider.GetScreenshot(ScreenshotType.Fullscreen);

            _startPoint = Point.Empty;
            _endPoint = Point.Empty;
        }

        public Rectangle SelectedArea() => new Rectangle(
            Math.Min(_startPoint.X, _endPoint.X), 
            Math.Min(_startPoint.Y, _endPoint.Y), 
            Math.Abs(_startPoint.X - _endPoint.X), 
            Math.Abs(_startPoint.Y - _endPoint.Y));
        
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
            if (_endPoint == Point.Empty || e.Button != MouseButtons.Left)
                return;

            _endPoint = e.Location;
            Invalidate();
        }

        private void SelectionForm_Paint(object sender, PaintEventArgs e)
        {
            var selection = SelectedArea();
            Console.WriteLine(selection);
            if (_endPoint != Point.Empty)
            {
                Console.WriteLine(selection);
                e.Graphics.DrawRectangle(_rectanglePen, selection);   //Draw the border
                e.Graphics.ExcludeClip(selection);                    //Exclude the rectangle so the white brush doesn't paint it

            }

            e.Graphics.FillRectangle(_backgroundBrush, Bounds);
        }
    }
}


//DRAW DIMENSION
//var dimension = $"{_selection.Width}x{_selection.Height}";
//var size = graphics.MeasureString(dimension, Font);

//float y = _selection.Y + _selection.Height + DIMENSION_TEXT_OFFSET; //spaces the dimension text above the selection box
//if (Height < y + size.Height)                                           //checks if it goes out of screen
//    y -= DIMENSION_TEXT_OFFSET*2 - size.Height;                         //soo set the top in the selectionbox itself

//float x = _selection.X + _selection.Width - size.Width;             //calculates the x_pos of the dimension 

//graphics.DrawString(dimension, Font, _textBrush, x, y);