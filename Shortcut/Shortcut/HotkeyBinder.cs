using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Shortcut
{
    /// <summary>
    /// Used to bind and unbind <see cref="Hotkey"/>s to 
    /// <see cref="HotkeyCallback"/>s.
    /// </summary>
    public class HotkeyBinder : IDisposable
    {
        private readonly HotkeyContainer _container;
        private readonly HotkeyWindow _hotkeyWindow;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="HotkeyBinder"/> class.
        /// </summary>
        public HotkeyBinder()
        {
            _container = new HotkeyContainer();
            _hotkeyWindow = new HotkeyWindow();
            _hotkeyWindow.HotkeyPressed += OnHotkeyPressed;
        }

        /// <summary>
        /// Indicates whether a <see cref="Hotkey"/> has been bound already either 
        /// by this application or another application.
        /// </summary>
        /// <param name="hotkeyCombo">
        /// The <see cref="Hotkey"/> to evaluate.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <see cref="Hotkey"/> has not been previously bound 
        /// and is available to be bound; otherwise, <c>false</c>.
        /// </returns>
        public bool IsHotkeyAlreadyBound(Hotkey hotkeyCombo)
        {
            if (!NativeMethods.RegisterHotKey(_hotkeyWindow.Handle, hotkeyCombo.GetHashCode(), (uint)hotkeyCombo.Modifier, (uint)hotkeyCombo.Key))
                return true;
            
            NativeMethods.UnregisterHotKey(_hotkeyWindow.Handle,  hotkeyCombo.GetHashCode());
            return false;
        }

        /// <summary>
        /// Binds a hotkey combination to a <see cref="HotkeyCallback"/>.
        /// </summary>
        /// <param name="modifiers">The modifers that constitute this hotkey.</param>
        /// <param name="keys">The keys that constitute this hotkey.</param>
        /// <exception cref="HotkeyAlreadyBoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void Bind(Modifiers modifiers, Keys keys, Action<HotkeyPressedEventArgs> hotkeyPressed)
        {
            Bind(new Hotkey(modifiers, keys), hotkeyPressed);
        }

        /// <summary>
        /// Binds a <see cref="Hotkey"/> to a <see cref="HotkeyCallback"/>.
        /// </summary>
        /// <exception cref="HotkeyAlreadyBoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void Bind(Hotkey hotkeyCombo, Action<HotkeyPressedEventArgs> hotkeyPressed)
        {
            if (hotkeyCombo == null) 
                throw new ArgumentNullException(nameof(hotkeyCombo));

            if (hotkeyPressed == null)
                throw new ArgumentNullException(nameof(hotkeyPressed));

            _container.Add(hotkeyCombo, hotkeyPressed);
            RegisterHotkey(hotkeyCombo);
        }

        private void RegisterHotkey(Hotkey hotkeyCombo)
        {
            if (!NativeMethods.RegisterHotKey(_hotkeyWindow.Handle, hotkeyCombo.GetHashCode(), (uint)hotkeyCombo.Modifier, (uint)hotkeyCombo.Key))
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        /// <summary>
        /// Unbinds a previously bound hotkey combination.
        /// </summary>
        public void Unbind(Modifiers modifiers, Keys keys)
        {
            Unbind(new Hotkey(modifiers, keys));
        }

        /// <summary>
        /// Unbinds a previously bound <see cref="Hotkey"/>.
        /// </summary>
        /// <exception cref="HotkeyNotBoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void Unbind(Hotkey hotkeyCombo)
        {
            _container.Remove(hotkeyCombo);
            UnregisterHotkey(hotkeyCombo);
        }

        private void UnregisterHotkey(Hotkey hotkeyCombo)
        {
            if (!NativeMethods.UnregisterHotKey(_hotkeyWindow.Handle, hotkeyCombo.GetHashCode()))
                throw new HotkeyNotBoundException(Marshal.GetLastWin32Error());
        }

        private void OnHotkeyPressed(object sender, HotkeyPressedEventArgs e)
        {
            var callback = _container.Find(e.Hotkey);
            callback?.Invoke(e);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _hotkeyWindow.Dispose();
        }
    }
}