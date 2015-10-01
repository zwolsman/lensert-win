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
                _cleanScreenshot = value;

                _shadedScreenshot = new Bitmap(_cleanScreenshot);
                using (var graphics = Graphics.FromImage(_shadedScreenshot))
                    graphics.FillRectangle(_rectangleBrush, 0, 0, _shadedScreenshot.Width, _shadedScreenshot.Height);
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
                //Update();
            }
        }
        
        public SelectionForm()
        {
            InitializeComponent();
            Bounds = SystemInformation.VirtualScreen;
            
            _textBrush = new SolidBrush(Preferences.Default.SelectionRectangleColor);
            _rectangleBrush = new SolidBrush(Color.FromArgb(50, Color.Gray));       //This is actually a bug where the transparancykey with the Red does register the mous input 
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

        private Rectangle[] SplitRectangle(Rectangle source, Rectangle toRemove)
        {
            // The left rectangle is from the topleft to the left of the removed rectangle
            // thus the overlap of bottom/top part with right/left part rectangles are calculated in
            // the left and right rectangle.
            // This is done because it won't calculate any overlap :)

            if (!source.IntersectsWith(toRemove))
                return new Rectangle[0];

            var rectangleLeft = new Rectangle(
                source.X,
                source.Y,
                toRemove.X - source.X,
                source.Height);

            var rectangleRight = new Rectangle(
                toRemove.Right,
                source.Y,
                Math.Abs(source.Right - toRemove.Right),
                source.Height);

            var rectangleTop = new Rectangle(
                toRemove.X,
                source.Y,
                toRemove.Width,
                toRemove.Y - source.Y);

            var rectangleBottom = new Rectangle(
                toRemove.X,
                toRemove.Bottom,
                toRemove.Width,
                Math.Abs(source.Bottom - toRemove.Bottom));

            return new[] { rectangleLeft, rectangleTop, rectangleRight, rectangleBottom };
        }

        private void SelectionForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                SelectedArea = Rectangle.Empty;
                Close();
            }
        }

        Bitmap bitmap;
        private void SelectionForm_Paint(object sender, PaintEventArgs e)
        {
            if (bitmap == null)
                bitmap = new Bitmap(Bounds.Width, Bounds.Height);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                if (_selectedArea == Rectangle.Empty)
                {
                    graphics.FillRectangle(_rectangleBrush, Bounds);
                    e.Graphics.DrawImage(bitmap, 0, 0);
                    return;
                }

                if (_selectedArea.Right > _oldSelectedArea.Right)
                {
                    var largerRectangles = SplitRectangle(_selectedArea, _oldSelectedArea);
                    if (largerRectangles.Length == 0)
                        return;

                    foreach (var rectangle in largerRectangles)
                        graphics.DrawImage(_cleanScreenshot, rectangle, rectangle, GraphicsUnit.Pixel);
                }
                else if (_selectedArea.Right < _oldSelectedArea.Right)
                {
                    var smallerRectangles = SplitRectangle(_selectedArea, _oldSelectedArea);
                    if (smallerRectangles.Length == 0)
                        return;

                    foreach (var rectangle in smallerRectangles)
                        graphics.DrawImage(_shadedScreenshot, rectangle, rectangle, GraphicsUnit.Pixel);
                }
                
                var sw = Stopwatch.StartNew();
                e.Graphics.DrawImage(bitmap, 0, 0);
                Console.WriteLine(sw.ElapsedMilliseconds);
            }

            //if (_oldSelectedArea == Rectangle.Empty)
            {
                
            }
            //else
            //{
            //    var overlap = Rectangle.Intersect(_oldSelectedArea, _selectedArea);

            //    var rectangles = SplitRectangle(_selectedArea, overlap);
            //    foreach (var rectangle in rectangles)
            //    {
            //        graphics.FillRectangle(_rectangleBrush, rectangle);
            //        //e.Graphics.DrawRectangle(Pens.Red, rectangle);                      //Draw the border
            //    }
            //}

            ////e.Graphics.DrawRectangle(Pens.Green, _selectedArea);                      //Draw the border

            //var dimension = $"{_selectedArea.Width}x{_selectedArea.Height}";
            //var size = e.Graphics.MeasureString(dimension, Font);                       //generates dimension string

            //float y = _selectedArea.Y + _selectedArea.Height + DIMENSION_TEXT_OFFSET;     //spaces the dimension text right bottom corner
            //float x = _selectedArea.X + _selectedArea.Width - size.Width;                 //calculates the x_pos of the dimension 

            //var currentScreenBounds = Screen.FromPoint(MousePosition).Bounds;
            //if (y + size.Height > currentScreenBounds.Height)
            //    y -= size.Height + DIMENSION_TEXT_OFFSET*2;

            //e.Graphics.DrawString(dimension, Font, _textBrush, x, y);                   //draws string
        }
    }
}