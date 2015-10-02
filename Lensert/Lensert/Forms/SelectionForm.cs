using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Lensert
{
    public sealed partial class SelectionForm : Form
    {
        private const int DIMENSION_TEXT_OFFSET = 2; //TODO: Refactor into settings

        private readonly SolidBrush _rectangleBrush, _textBrush;
        private readonly Pen _rectanglePen;

        private Rectangle _oldSelectedArea, _selectedArea;
        private Bitmap _shadedScreenshot, _cleanScreenshot;

        public Bitmap Screenshot
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
                    graphics.FillRectangle(_rectangleBrush, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
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
                _oldSelectedArea = _selectedArea;
                _selectedArea = value;

                Invalidate();
                Update();
            }
        }
        
        public SelectionForm()
        {
            InitializeComponent();
            Bounds = SystemInformation.VirtualScreen;
            Location = Bounds.Location;

            DoubleBuffered = true;
            
            _textBrush = new SolidBrush(Preferences.Default.SelectionRectangleColor);
            _rectangleBrush = new SolidBrush(Color.FromArgb(120, Color.White));       //This is actually a bug where the transparancykey with the Red does register the mous input 
            _rectanglePen = new Pen(Preferences.Default.SelectionRectangleColor);   //(fyi, with any other color the mouse would click through it)
        }

        private void SelectionForm_Load(object sender, EventArgs e)
        {
            _selectedArea = Rectangle.Empty;
            _oldSelectedArea = Rectangle.Empty;
        }
        
        private void SelectionForm_MouseUp(object sender, MouseEventArgs e)
        {
            Close();
        }

        //private Rectangle[] SplitRectangle(Rectangle source, Rectangle toRemove)
        //{
        //    // The left rectangle is from the topleft to the left of the removed rectangle
        //    // thus the overlap of bottom/top part with right/left part rectangles are calculated in
        //    // the left and right rectangle.
        //    // This is done because it won't calculate any overlap :)

        //    if (!source.IntersectsWith(toRemove))
        //        return new Rectangle[0];

        //    var rectangleLeft = new Rectangle(
        //        source.X,
        //        source.Y,
        //        toRemove.X - source.X,
        //        source.Height);

        //    var rectangleRight = new Rectangle(
        //        toRemove.Right,
        //        source.Y,
        //        Math.Abs(source.Right - toRemove.Right),
        //        source.Height);

        //    var rectangleTop = new Rectangle(
        //        toRemove.X,
        //        source.Y,
        //        toRemove.Width,
        //        toRemove.Y - source.Y);

        //    var rectangleBottom = new Rectangle(
        //        toRemove.X,
        //        toRemove.Bottom,
        //        toRemove.Width,
        //        Math.Abs(source.Bottom - toRemove.Bottom));

        //    return new[] { rectangleLeft, rectangleTop, rectangleRight, rectangleBottom };
        //}

        private void SelectionForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                SelectedArea = Rectangle.Empty;
                Close();
            }
        }

        private void SelectionForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(_shadedScreenshot, Bounds);
            e.Graphics.DrawImage(_cleanScreenshot, _selectedArea, _selectedArea, GraphicsUnit.Pixel);

            var borderRectangle = _selectedArea;
            borderRectangle.Width -= 1;
            borderRectangle.Height -= 1;
            if(Bounds.Width <= borderRectangle.Right)
            { 
                var deltaRight = borderRectangle.Right - Bounds.Right;
                borderRectangle.Width -= deltaRight == 0 ? 1 : deltaRight;
            }

            if(Bounds.Height <= borderRectangle.Bottom)
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