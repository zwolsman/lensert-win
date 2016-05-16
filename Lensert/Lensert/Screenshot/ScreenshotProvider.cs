using System;
using System.Collections.Generic;
using System.Drawing;

namespace Lensert.Screenshot
{
    static class ScreenshotProvider
    {
        private static readonly Dictionary<Type, ScreenshotFactory> _factories = new Dictionary<Type, ScreenshotFactory>();
                
        public static Image Create(Type type)
        {
            if (!typeof(ScreenshotFactory).IsAssignableFrom(type))
                throw new ArgumentException("Type doesn't inherit from ScreenshotFactory", nameof(type));

            if (!_factories.ContainsKey(type))
                _factories[type] = (ScreenshotFactory)Activator.CreateInstance(type, true);

            var factory = _factories[type];
            return factory.TakeScreenshot();
        }

        public static Image Create<T>() where T : ScreenshotFactory
            => Create(typeof(T));
    }
}
