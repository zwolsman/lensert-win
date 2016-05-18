using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Corner_Picker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private readonly Brush _fillBrush = new SolidBrush(Color.FromArgb(50, 30, 130, 255));
        private readonly Pen _borderPen = new Pen(Color.FromArgb(50, 204, 229, 255));
        private readonly Font _font = new Font("Arial", 8.25F);
        private Image backgroundImage;
        private void Form1_Load(object sender, EventArgs e)
        {
            RandomSnapshot();
            UpdateTest();
        }


        void RandomSnapshot()
        {
            Image bg = Image.FromFile(GetCurrentDesktopWallpaper());

            Random r = new Random();

            backgroundImage = CropImage(bg,
                new Rectangle(r.Next(bg.Width - picUpLeft.Width), r.Next(bg.Height - picUpLeft.Height), picUpLeft.Width,
                    picUpLeft.Height));

        }
        void UpdateTest()
        {
            topLeft();
            botLeft();
            botRight();
            topRight();
        }
        void botRight()
        {
            using (Bitmap b = new Bitmap(picUpLeft.Width, picUpLeft.Height))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.DrawImageUnscaledAndClipped(backgroundImage, new Rectangle(0, 0, b.Width, b.Height));

                    Rectangle targetRectangle = new Rectangle(0, 0, b.Width - 20, b.Height - 20);

                    g.FillRectangle(_fillBrush, targetRectangle);
                    g.DrawRectangle(_borderPen, targetRectangle);

                    SizeF info = g.MeasureString($"{b.Width - 20}x{b.Height - 20}", _font);
                    g.DrawString($"{b.Width - 20}x{b.Height - 20}", _font, _fillBrush,
                        targetRectangle.X + targetRectangle.Width - info.Width, targetRectangle.Y + targetRectangle.Height - info.Height);
                }

                picLowerRight.Image = (Image)b.Clone();
            }
        }

        void botLeft()
        {
            using (Bitmap b = new Bitmap(picUpLeft.Width, picUpLeft.Height))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.DrawImageUnscaledAndClipped(backgroundImage, new Rectangle(0, 0, b.Width, b.Height));

                    Rectangle targetRectangle = new Rectangle(20, 0, 100, b.Height - 20);

                    g.FillRectangle(_fillBrush, targetRectangle);
                    g.DrawRectangle(_borderPen, targetRectangle);

                    SizeF info = g.MeasureString($"{b.Width - 20}x{b.Height - 20}", _font);
                    g.DrawString($"{b.Width - 20}x{b.Height - 20}", _font, _fillBrush,
                        targetRectangle.X, targetRectangle.Y + targetRectangle.Height - info.Height);
                }

                picLowerLeft.Image = (Image)b.Clone();
            }
        }

        void topLeft()
        {
            using (Bitmap b = new Bitmap(picUpLeft.Width, picUpLeft.Height))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.DrawImageUnscaledAndClipped(backgroundImage, new Rectangle(0, 0, b.Width, b.Height));

                    Rectangle targetRectangle = new Rectangle(20, 20, 100, 100);

                    g.FillRectangle(_fillBrush, targetRectangle);
                    g.DrawRectangle(_borderPen, targetRectangle);
                    g.DrawString($"{b.Width - 20}x{b.Height - 20}", _font, _fillBrush,
                20, 20);
                }

                picUpLeft.Image = (Image)b.Clone();
            }
        }
        void topRight()
        {
            using (Bitmap b = new Bitmap(picUpLeft.Width, picUpLeft.Height))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.DrawImageUnscaledAndClipped(backgroundImage, new Rectangle(0, 0, b.Width, b.Height));

                    Rectangle targetRectangle = new Rectangle(0, 20, b.Width - 20, b.Height - 20);

                    g.FillRectangle(_fillBrush, targetRectangle);
                    g.DrawRectangle(_borderPen, targetRectangle);

                    SizeF info = g.MeasureString($"{b.Width - 20}x{b.Height - 20}", _font);
                    g.DrawString($"{b.Width - 20}x{b.Height - 20}", _font, _fillBrush,
                        targetRectangle.X + targetRectangle.Width - info.Width, targetRectangle.Y);
                }

                picUpRight.Image = (Image)b.Clone();
            }
        }

        public Bitmap CropImage(Image source, Rectangle section)
        {
            // An empty bitmap which will hold the cropped image
            Bitmap bmp = new Bitmap(section.Width, section.Height);

            Graphics g = Graphics.FromImage(bmp);

            // Draw the given area (section) of the source image
            // at location 0,0 on the empty bitmap (bmp)
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            return bmp;
        }

        private const uint SPI_GETDESKWALLPAPER = 0x73;

      private const int MAX_PATH = 260;



      [DllImport("user32.dll", CharSet = CharSet.Auto)]

      public static extern int SystemParametersInfo(uint uAction, int uParam, string lpvParam, int fuWinIni);





      public string GetCurrentDesktopWallpaper()

      {

          string currentWallpaper = new string('\0', MAX_PATH);

          SystemParametersInfo(SPI_GETDESKWALLPAPER, currentWallpaper.Length, currentWallpaper, 0);

          return currentWallpaper.Substring(0, currentWallpaper.IndexOf('\0'));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RandomSnapshot();
            UpdateTest();
        }

        private void picUpLeft_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
        }

        private void picUpRight_Click(object sender, EventArgs e)
        {
            radioButton2.Checked = true;
        }

        private void picLowerLeft_Click(object sender, EventArgs e)
        {
            radioButton4.Checked = true;
        }

        private void picLowerRight_Click(object sender, EventArgs e)
        {
            radioButton3.Checked = true;
        }
    }
}
