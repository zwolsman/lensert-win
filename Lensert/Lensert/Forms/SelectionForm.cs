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

        private SolidBrush _transparantBrush, _textBrush;
        private Pen _rectanglePen;
        private Point _startPoint, _endPoint;

        public Rectangle SelectedArea { get; private set; }

        public SelectionForm()
        {
            InitializeComponent();

            DoubleBuffered = true;
            Bounds = SystemInformation.VirtualScreen;
            
            _startPoint = Point.Empty;
            _endPoint = Point.Empty;
        }

        private void SelectionForm_Load(object sender, EventArgs e)
        {
            _transparantBrush?.Dispose();
            _rectanglePen?.Dispose();
            SelectedArea = Rectangle.Empty;

            _textBrush = new SolidBrush(ForeColor);
            _transparantBrush = new SolidBrush(Color.Plum);
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
            if (e.KeyCode == Keys.Escape)
            {
                SelectedArea = Rectangle.Empty;
                Close();
            }
        }


        private void SelectionForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            _endPoint = e.Location;

            SelectedArea = new Rectangle(Math.Min(_startPoint.X, _endPoint.X),
                                         Math.Min(_startPoint.Y, _endPoint.Y),
                                         Math.Abs(_startPoint.X - _endPoint.X),
                                         Math.Abs(_startPoint.Y - _endPoint.Y));

            Invalidate();
        }
        
        private void SelectionForm_Paint(object sender, PaintEventArgs e)
        {
            //TODO: Optimize further, makes user selectable where dimension text is

            if (SelectedArea == Rectangle.Empty)
                return;
            
            e.Graphics.FillRectangle(_transparantBrush, SelectedArea);             //makes transparant region
            e.Graphics.DrawRectangle(_rectanglePen, SelectedArea);                 //Draw the border

            var dimension = $"{SelectedArea.Width}x{SelectedArea.Height}";
            var size = e.Graphics.MeasureString(dimension, Font);               //generates dimension string

            float y = SelectedArea.Y + SelectedArea.Height + DIMENSION_TEXT_OFFSET;   //spaces the dimension text right bottom corner
            float x = SelectedArea.X + SelectedArea.Width - size.Width;               //calculates the x_pos of the dimension 
            e.Graphics.DrawString(dimension, Font, _textBrush, x, y);           //draws string
        }
    }
}