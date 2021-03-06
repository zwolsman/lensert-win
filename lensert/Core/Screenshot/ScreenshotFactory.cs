﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Lensert.Core.Screenshot.Factories;

namespace Lensert.Core.Screenshot
{
    internal abstract class ScreenshotFactory
    {
        private static readonly Dictionary<Type, ScreenshotFactory> _templates;

        static ScreenshotFactory()
        {
            _templates = new Dictionary<Type, ScreenshotFactory>();
        }

        public bool SpecialKeyPressed { get; protected set; }

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

        public abstract Image TakeScreenshot();
    }
}
