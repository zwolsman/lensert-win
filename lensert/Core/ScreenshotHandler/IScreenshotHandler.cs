using System.Drawing;
using System.Threading.Tasks;

namespace Lensert.Core.ScreenshotHandler
{
    internal interface IScreenshotHandler
    {
        Task<string> HandleAsync(Image screenshot);
    }
}
