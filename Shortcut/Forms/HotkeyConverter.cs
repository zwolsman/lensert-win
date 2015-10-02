using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace Shortcut.Forms
{
    /// <summary>
    /// Provides a type converter to convert Hotkey objects to and from other representations.
    /// </summary>
    public class HotkeyConverter : KeysConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof (Keys))
            {
                var hotkey = value as Hotkey;

                if (hotkey != null)
                {
                    var keys = Keys.None;
                    if (hotkey.Modifier.HasFlag(Modifiers.Alt))
                        keys |= Keys.Alt;

                    if (hotkey.Modifier.HasFlag(Modifiers.Control))
                        keys |= Keys.Control;

                    if (hotkey.Modifier.HasFlag(Modifiers.Shift))
                        keys |= Keys.Shift;
                    
                    keys |= hotkey.Key;
                    return keys;
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                value = base.ConvertFrom(context, culture, value);
            }


            if (value.GetType() == typeof(Keys))
            {
                var keys = (Keys)value;
                var modifiers = Modifiers.None;

                if (keys.HasFlag(Keys.Alt))
                    modifiers |= Modifiers.Alt;

                if (keys.HasFlag(Keys.Control))
                    modifiers |= Modifiers.Control;

                if (keys.HasFlag(Keys.Shift))
                    modifiers |= Modifiers.Shift;

                keys = ExtractNonModifiers(keys);
                return new Hotkey(modifiers, keys);
            }

            return base.ConvertFrom(context, culture, value);
        }

        // Brian: Extract non-modifiers from the low word of keys
        private static Keys ExtractNonModifiers(Keys keys) 
            => (Keys)((int)keys & 0x0000FFFF);
    }
}