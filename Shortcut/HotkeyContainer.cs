using System;
using System.Collections.Generic;

namespace Shortcut
{
    internal class HotkeyContainer
    {
        private readonly IDictionary<Hotkey, Action<HotkeyPressedEventArgs>> container;

        internal HotkeyContainer()
        {
            container = new Dictionary<Hotkey, Action<HotkeyPressedEventArgs>>();
        }

        internal void Add(Hotkey hotkey, Action<HotkeyPressedEventArgs> callback)
        {
            if (container.ContainsKey(hotkey))
            {
                throw new HotkeyAlreadyBoundException(
                    "This hotkey cannot be bound because it has been previously bound either by this " +
                    "application or another running application.");
            }

            container.Add(hotkey, callback);
        }

        internal void Remove(Hotkey hotkey)
        {
            if (!container.ContainsKey(hotkey))
            {
                throw new HotkeyNotBoundException(
                    "This hotkey cannot be unbound because it has not previously been bound by this application");
            }

            container.Remove(hotkey);
        }

        internal Action<HotkeyPressedEventArgs> Find(Hotkey hotkey)
        {
            return container[hotkey];
        }
    }
}