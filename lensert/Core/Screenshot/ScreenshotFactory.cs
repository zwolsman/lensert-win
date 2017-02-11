using System;
using System.Collections.Generic;
using System.Drawing;
using Lensert.Core.Screenshot.Factories;
using Lensert.Helpers;

namespace Lensert.Core.Screenshot
{
    internal abstract class ScreenshotFactory
    {
        private static readonly Dictionary<Type, ScreenshotFactory> _templates;
        public bool SpecialKeyPressed { get; protected set; }

        static ScreenshotFactory()
        {
            _templates = new Dictionary<Type, ScreenshotFactory>();
        }

        public static Image Create(Type templateType)
        {
            if (!typeof(ScreenshotFactory).IsAssignableFrom(templateType))
                throw new ArgumentException("Type doesn't inherit from AbstractScreenshotTemplate", nameof(templateType));

            if (!_templates.ContainsKey(templateType))
                _templates[templateType] = (ScreenshotFactory) Activator.CreateInstance(templateType, true);

            var template = _templates[templateType];
            var image = template.TakeScreenshot();

            return template.SpecialKeyPressed
                ? HandleSpecialKey(templateType)
                : image;
        }
        
        public static Image Create<T>() where T : ScreenshotFactory
            => Create(typeof(T));

        private static Image HandleSpecialKey(Type templateType)
        {
            return templateType == typeof(SelectWindowScreenshot)
                ? Create<UserSelectionScreenshot>()
                : (templateType == typeof(UserSelectionScreenshot)
                    ? Create<SelectWindowScreenshot>()
                    : null);
        }
        
        protected abstract Rectangle GetArea();

        public virtual Image TakeScreenshot()
        {
            var area = GetArea();
            return Native.TakeScreenshot(area);
        }
    }
}
