using System.Drawing;
using System.Threading.Tasks;

namespace Lensert.Core
{
    internal interface IImageUploader
    {
        Task<string> UploadImageAsync(Image bitmap);
    }
}