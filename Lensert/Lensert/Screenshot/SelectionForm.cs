using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lensert.Screenshot
{
    public sealed partial class SelectionForm : Form
    {
        private const int DIMENSION_TEXT_OFFSET = 2; //TODO: Refactor into settings

        private readonly SolidBrush _rectangleBrush, _textBrush;
        private readonly Pen _rectanglePen;

        private Rectangle _selectedArea;
        private Image _shadedScreenshot, _cleanScreenshot;

        public Image Screenshot
        {
            get
            {
                return _cleanScreenshot;
            }
            set
            {
                _cleanScreenshot?.Dispose();
                _shadedScreenshot?.Dispose();

                _cleanScreenshot = value;

                _shadedScreenshot = new Bitmap(_cleanScreenshot);
                using (var graphics = Graphics.FromImage(_shadedScreenshot))
                    graphics.FillRectangle(_rectangleBrush, 0, 0, Bounds.Width, Bounds.Height);

                BackgroundImage = _shadedScreenshot;
            }
        }

        public Rectangle SelectedArea
        {
            get
            {
                return _selectedArea;
            }
            set
            {
                _selectedArea = value;

                Invalidate();
                Update();
            }
        }
        
        public SelectionForm()
        {
            InitializeComponent();
            Bounds = SystemInformation.VirtualScreen;

#if (DEBUG)
            {
                TopMost = false;
            }
#endif
            DoubleBuffered = true;
            
            _textBrush = new SolidBrush(Preferences.Default.SelectionRectangleColor);
            _rectangleBrush = new SolidBrush(Color.FromArgb(120, Color.White));       //This is actually a bug where the transparancykey with the Red does register the mous input 
            _rectanglePen = new Pen(Preferences.Default.SelectionRectangleColor);   //(fyi, with any other color the mouse would click through it)
        }

        private void SelectionForm_Load(object sender, EventArgs e)
        {
            _selectedArea = Rectangle.Empty;
        }
        
        private void SelectionForm_MouseUp(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void SelectionForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                SelectedArea = Rectangle.Empty;
                Close();
            }
        }

        private Rectangle RectangleAbs(Rectangle rectangle)
        {
            var rectFixed = rectangle;
            if (rectangle.Width < 0)
            {
                rectFixed.X += rectangle.Width;
                rectFixed.Width = Math.Abs(rectangle.Width);
            }

            if (rectFixed.Height < 0)
            {
                rectFixed.Y += rectangle.Height;
                rectFixed.Height = Math.Abs(rectangle.Height);
            }

            return rectFixed;
        }

        private void SelectionForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(_cleanScreenshot, _selectedArea, _selectedArea, GraphicsUnit.Pixel);

            var borderRectangle = _selectedArea;
            borderRectangle.Width -= 1;
            borderRectangle.Height -= 1;
            if (Bounds.Width <= borderRectangle.Right)
            {
                var deltaRight = borderRectangle.Right - Bounds.Right;
                borderRectangle.Width -= deltaRight == 0 ? 1 : deltaRight;
            }

            if (Bounds.Height <= borderRectangle.Bottom)
            {
                var deltaBottom = borderRectangle.Bottom - Bounds.Bottom;
                borderRectangle.Height -= deltaBottom == 0 ? 1 : deltaBottom;           //compensates for out of bounds (only visually, 
            }                                                                           //screenshot does reach till the end and beyond)
            e.Graphics.DrawRectangle(_rectanglePen, borderRectangle);                   //Draw the border

            var dimension = $"{_selectedArea.Width}x{_selectedArea.Height}";
            var size = e.Graphics.MeasureString(dimension, Font);                       //generates dimension string

            float y = _selectedArea.Y + _selectedArea.Height + DIMENSION_TEXT_OFFSET;   //spaces the dimension text right bottom corner
            float x = _selectedArea.X + _selectedArea.Width - size.Width;               //calculates the x_pos of the dimension 

            var currentScreenBounds = Screen.FromPoint(MousePosition).Bounds;
            if (y + size.Height > currentScreenBounds.Height)
                y -= size.Height + DIMENSION_TEXT_OFFSET * 2;

            e.Graphics.DrawString(dimension, Font, _textBrush, x, y);                    //draws string
        }
    }
}