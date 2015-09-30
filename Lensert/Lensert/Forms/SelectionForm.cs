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

        public Rectangle SelectedArea { get; set; }

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
            SelectedArea = Rectangle.Empty;
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

            var rectangleLeft = new Rectangle(
                source.X,
                source.Y,
                toRemove.X,
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
                toRemove.Y);

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
            if (SelectedArea == Rectangle.Empty)
                return;


            Rectangle currentScreenBounds = Screen.FromPoint(MousePosition).Bounds;

            var rectangles = SplitRectangle(Bounds, SelectedArea);
            foreach (var rectangle in rectangles)
                e.Graphics.FillRectangle(_rectangleBrush, rectangle);                  
            
            e.Graphics.DrawRectangle(_rectanglePen, SelectedArea);                      //Draw the border

            var dimension = $"{SelectedArea.Width}x{SelectedArea.Height}";
            var size = e.Graphics.MeasureString(dimension, Font);                       //generates dimension string

            float y = SelectedArea.Y + SelectedArea.Height + DIMENSION_TEXT_OFFSET;     //spaces the dimension text right bottom corner
            float x = SelectedArea.X + SelectedArea.Width - size.Width;                 //calculates the x_pos of the dimension 

            if (y + size.Height > currentScreenBounds.Height)
                y -= size.Height + DIMENSION_TEXT_OFFSET*2;

            e.Graphics.DrawString(dimension, Font, _textBrush, x, y);                   //draws string
        }
    }
}