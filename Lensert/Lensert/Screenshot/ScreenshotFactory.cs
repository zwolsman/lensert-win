using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lensert.Screenshot
{
    static class ScreenshotFactory
    {
        private static Dictionary<Type, IScreenshot> _screenshotDictionary = new Dictionary<Type, IScreenshot>();

        private static IScreenshot GetScreenshot<T>() where T : IScreenshot
        {
            var type = typeof(T);
            if (!_screenshotDictionary.ContainsKey(type))
                _screenshotDictionary[type] = Activator.CreateInstance<T>();

            return _screenshotDictionary[type];
        }

        public static Bitmap Create<T>() where T : IScreenshot
        {
            var screenshot = GetScreenshot<T>();
            var area = screenshot.GetArea();

            return NativeHelper.TakeScreenshot(area);
        }
    }
}
