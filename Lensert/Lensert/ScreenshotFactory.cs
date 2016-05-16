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

        public static Image Create(Type template)
        {
            if (!typeof (AbstractScreenshotTemplate).IsAssignableFrom(template))
                throw new ArgumentException("Type doesn't inherit from AbstractScreenshotTemplate", nameof(template));

            if (!_templates.ContainsKey(template))
                _templates[template] = (AbstractScreenshotTemplate) Activator.CreateInstance(template, true);
            
            return _templates[template].TakeScreenshot();
        }

        public static Image Create<T>() where T : AbstractScreenshotTemplate
            => Create(typeof (T));
    }
}