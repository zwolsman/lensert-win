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

        private static IScreenshot GetScreenshot(Type type)
        { 
            if (!_screenshotDictionary.ContainsKey(type))
                _screenshotDictionary[type] = (IScreenshot) Activator.CreateInstance(type, true);

            return _screenshotDictionary[type];
        }
        
        public static Bitmap Create(Type type)
        {
            if (!typeof(IScreenshot).IsAssignableFrom(type))
                throw new ArgumentException("Type doesn't inherit from IScreenshot", nameof(type));

            var screenshot = GetScreenshot(type);
            var area = screenshot.GetArea();

            return NativeHelper.TakeScreenshot(area);
        }

        public static Bitmap Create<T>() where T : IScreenshot
            => Create(typeof(T));
    }
}
