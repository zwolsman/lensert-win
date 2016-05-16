using System;
using System.Collections.Generic;
using System.Drawing;
using Lensert.Screenshot;

namespace Lensert
{
    internal static class ScreenshotFactory
    {
        private static readonly Dictionary<Type, AbstractScreenshotTemplate> _templates;

        static ScreenshotFactory()
        {
            _templates = new Dictionary<Type, AbstractScreenshotTemplate>();
        }

        public static Image Create(Type type)
        {
            if (!typeof (AbstractScreenshotTemplate).IsAssignableFrom(type))
                throw new ArgumentException("Type doesn't inherit from ScreenshotFactory", nameof(type));

            if (!_templates.ContainsKey(type))
                _templates[type] = (AbstractScreenshotTemplate) Activator.CreateInstance(type, true);
            
            return _templates[type].TakeScreenshot();
        }

        public static Image Create<T>() where T : AbstractScreenshotTemplate
            => Create(typeof (T));
    }
}