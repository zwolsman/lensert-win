﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lensert.Screenshot
{
    class FullScreen : IScreenshot
    {
        public Rectangle GetArea()
            => SystemInformation.VirtualScreen;
    }
}
