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
        private int _x1, _y1, _x2, _y2;

        public SelectionForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            Bounds = SystemInformation.VirtualScreen;
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
            _backgroundBrush = new SolidBrush(Preferences.Default.SelectionBackgroundColor);
            _rectangleBrush = new SolidBrush(Preferences.Default.SelectionRectangleColor);
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
                graphics.FillRectangle(_backgroundBrush, Bounds);

                var selectedArea = GetSelectedArea();
                if (selectedArea != Rectangle.Empty)
                {
                    graphics.FillRectangle(_rectangleBrush, GetSelectedArea());

                    var dimension = $"{selectedArea.Width}x{selectedArea.Height}";
                    var size = graphics.MeasureString(dimension, Font);

                    float y = selectedArea.Y + selectedArea.Height + DIMENSION_TEXT_OFFSET; //spaces the dimension text above the selection box
                    if (Height < y + size.Height)                                           //checks if it goes out of screen
                        y -= DIMENSION_TEXT_OFFSET*2 - size.Height;                         //soo set the top in the selectionbox itself

                    float x = selectedArea.X + selectedArea.Width - size.Width;             //calculates the x_pos of the dimension 

                    graphics.DrawString(dimension, Font, _textBrush, x, y);
                }
                
                e.Graphics.DrawImage(bitmap, 0, 0);
            }           
        }
    }
}
