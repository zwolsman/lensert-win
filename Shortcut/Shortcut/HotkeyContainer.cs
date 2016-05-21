using System;
using System.Collections.Generic;

namespace Shortcut
{
    internal class HotkeyContainer
    {
        private readonly IDictionary<Hotkey, Action<HotkeyPressedEventArgs>> _container;

        internal HotkeyContainer()
        {
            _container = new Dictionary<Hotkey, Action<HotkeyPressedEventArgs>>();
        }

        internal void Add(Hotkey hotkey, Action<HotkeyPressedEventArgs> callback)
        {
            if (_container.ContainsKey(hotkey))
            {
                throw new HotkeyAlreadyBoundException(
                    "This hotkey cannot be bound because it has been previously bound either by this " +
                    "application or another running application.");
            }

            _container.Add(hotkey, callback);
        }

        internal void Remove(Hotkey hotkey)
        {
            if (!_container.ContainsKey(hotkey))
            {
                throw new HotkeyNotBoundException(
                    "This hotkey cannot be unbound because it has not previously been bound by this application");
            }

            _container.Remove(hotkey);
        }

        internal Action<HotkeyPressedEventArgs> Find(Hotkey hotkey) 
            => _container[hotkey];
    }
}