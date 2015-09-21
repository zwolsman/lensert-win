using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lensert
{
    class DummyImageProvider
    {
        public static Image Next()
        {
            Console.WriteLine("Generating dummy image");
            return Image.FromStream(
                    new MemoryStream(
                        new WebClient().DownloadData("http://thecatapi.com/api/images/get?format=src&type=png")));
        }
    }
}
