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

namespace Lensert
{
    public partial class SelectionForm : Form
    {
        private const int DIMENSION_TEXT_OFFSET = 2; //TODO: Refactor into settings

        private readonly SolidBrush _rectangleBrush, _textBrush;
        private readonly Pen _rectanglePen;

        private Rectangle _oldSelectedArea, _selectedArea;

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
                throw new NotImplementedException();

            var rectangleLeft = new Rectangle(
                source.X,
                source.Y,
                toRemove.X - source.X,
                source.Height);

            var rectangleRight = new Rectangle(
                toRemove.Right,
                source.Y,
                source.Right - toRemove.Right,
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
                source.Bottom - toRemove.Bottom);

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

        private void SelectionForm_Paint(object sender, PaintEventArgs e)
        {
            //if (_selectedArea == Rectangle.Empty)
            //    return;

            var outer = new Rectangle(500, 500, 500, 500);
            var inner = new Rectangle(600, 600, 400, 400);

            e.Graphics.FillRectangle(Brushes.Gray, inner);

            var rectangles = SplitRectangle(outer, inner);
            e.Graphics.DrawRectangle(Pens.Yellow, rectangles[0]);        //left
            
            e.Graphics.DrawRectangle(Pens.Orange, rectangles[2]);       //right
            e.Graphics.DrawRectangle(Pens.Blue, rectangles[3]);          //bottom

            e.Graphics.DrawRectangle(Pens.Green, rectangles[1]);         //top

            //if (_oldSelectedArea == Rectangle.Empty)
            //{
            //    var rectangles = SplitRectangle(Bounds, _selectedArea);
            //    foreach (var rectangle in rectangles)
            //        e.Graphics.FillRectangle(_rectangleBrush, rectangle);
            //}
            //else
            //{
            //    var overlap = Rectangle.Intersect(_oldSelectedArea, _selectedArea);

            //    var rectangles = SplitRectangle(_selectedArea, overlap);
            //    e.Graphics.DrawRectangle(Pens.Pink, rectangles[0]);
            //    e.Graphics.DrawRectangle(Pens.Blue, rectangles[1]);
            //    e.Graphics.DrawRectangle(Pens.Purple, rectangles[2]);
            //    e.Graphics.DrawRectangle(Pens.Red, rectangles[3]);

            //    //foreach (var rectangle in rectangles)
            //    //{
            //    //    e.Graphics.FillRectangle(_rectangleBrush, rectangle);
            //    //    e.Graphics.DrawRectangle(Pens.Red, rectangle);                      //Draw the border
            //    //}
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