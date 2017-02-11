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

        public static Image Create(Type templateType)
        {
            if (!typeof (AbstractScreenshotTemplate).IsAssignableFrom(templateType))
                throw new ArgumentException("Type doesn't inherit from AbstractScreenshotTemplate", nameof(templateType));

            if (!_templates.ContainsKey(templateType))
                _templates[templateType] = (AbstractScreenshotTemplate) Activator.CreateInstance(templateType, true);

            var template = _templates[templateType];
            var image = template.TakeScreenshot();

            return template.SpecialKeyPressed
                ? HandleSpecialKey(templateType)
                : image;
        }

        public static Image Create<T>() where T : AbstractScreenshotTemplate
            => Create(typeof (T));

        private static Image HandleSpecialKey(Type templateType)
        {
            return templateType == typeof (SelectWindowTemplate) 
                ? Create<UserSelectionTemplate>() 
                : (templateType == typeof (UserSelectionTemplate) 
                    ? Create<SelectWindowTemplate>() 
                    : null);
        }
    }
}