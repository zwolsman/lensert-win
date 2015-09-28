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

        private readonly SolidBrush _transparantBrush, _textBrush;
        private readonly Pen _rectanglePen;
        internal IEnumerable<Rectangle> testRectangles;

        public Rectangle SelectedArea { get; set; }

        public SelectionForm()
        {
            InitializeComponent();
            Bounds = SystemInformation.VirtualScreen;
            
            _textBrush = new SolidBrush(ForeColor);
            _transparantBrush = new SolidBrush(Color.Red);                          //This is actually a bug where the transparancykey with the Red does register the mous input 
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
            
            e.Graphics.FillRectangle(_transparantBrush, SelectedArea);                  //makes transparant region
            e.Graphics.DrawRectangle(_rectanglePen, SelectedArea);                      //Draw the border

            var dimension = $"{SelectedArea.Width}x{SelectedArea.Height}";
            var size = e.Graphics.MeasureString(dimension, Font);                       //generates dimension string

            float y = SelectedArea.Y + SelectedArea.Height + DIMENSION_TEXT_OFFSET;     //spaces the dimension text right bottom corner
            float x = SelectedArea.X + SelectedArea.Width - size.Width;                 //calculates the x_pos of the dimension 
            e.Graphics.DrawString(dimension, Font, _textBrush, x, y);                   //draws string
        }
    }
}