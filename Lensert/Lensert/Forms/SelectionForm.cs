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
        private int _x1, _y1, _x2, _y2;

        public SelectionForm()
        {
            InitializeComponent();

            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            Bounds = SystemInformation.VirtualScreen;
            BackgroundImage = ScreenshotProvider.GetScreenshot(ScreenshotType.Fullscreen);
        }

        public Rectangle GetSelectedArea()
        {
            return new Rectangle(
                Math.Min(_x1, _x2),
                Math.Min(_y1, _y2),
                Math.Abs(_x1 - _x2),
                Math.Abs(_y1 - _y2));
        }

      
        private void SelectionForm_Load(object sender, EventArgs e)
        {
            _backgroundBrush?.Dispose();
            _rectangleBrush?.Dispose();

            _x1 = _x2 = _y1 = _y2 = 0;
            
            _textBrush = new SolidBrush(ForeColor);
            _backgroundBrush = new SolidBrush(Color.FromArgb(100, Preferences.Default.SelectionBackgroundColor));
            _rectangleBrush = new SolidBrush(Preferences.Default.SelectionRectangleColor);
            _rectanglePen = new Pen(Preferences.Default.SelectionRectangleColor);
        }

        private void SelectionForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            _x1 = e.X;
            _y1 = e.Y;
        }

        private void SelectionForm_MouseUp(object sender, MouseEventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void SelectionForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape) return;
            DialogResult = DialogResult.Cancel;
            Close();
        }


        private void SelectionForm_MouseMove(object sender, MouseEventArgs e)
        {
            var selectedArea = GetSelectedArea();
            if (selectedArea == Rectangle.Empty)
                return;

            _x2 = e.X;
            _y2 = e.Y;

            Invalidate();
        }

        private void SelectionForm_Paint(object sender, PaintEventArgs e)
        {
            using (var bitmap = new Bitmap(Width, Height))
            using (var graphics = Graphics.FromImage(bitmap))
            {

                var selectedArea = GetSelectedArea();
                if (selectedArea != Rectangle.Empty)
                {

                    graphics.DrawRectangle(_rectanglePen, GetSelectedArea());   //Draw the border
                    graphics.ExcludeClip(GetSelectedArea());                    //Exclude the rectangle so the white brush doesn't paint it
                    var dimension = $"{selectedArea.Width}x{selectedArea.Height}";
                    var size = graphics.MeasureString(dimension, Font);

                    float y = selectedArea.Y + selectedArea.Height + DIMENSION_TEXT_OFFSET; //spaces the dimension text above the selection box
                    if (Height < y + size.Height)                                           //checks if it goes out of screen
                        y -= DIMENSION_TEXT_OFFSET*2 - size.Height;                         //soo set the top in the selectionbox itself

                    float x = selectedArea.X + selectedArea.Width - size.Width;             //calculates the x_pos of the dimension 

                    graphics.DrawString(dimension, Font, _textBrush, x, y);
                }
                    graphics.FillRectangle(_backgroundBrush, Bounds);

                e.Graphics.DrawImage(bitmap, 0, 0);
            }           
        }
    }
}
