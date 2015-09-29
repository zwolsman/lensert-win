using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shortcut;
using Shortcut.Forms;

namespace Lensert
{
    //TODO make it multi language support, maybe gettext
    //https://code.google.com/p/gettext-cs-utils/
    public partial class PreferencesForm : Form
    {
        private readonly HotkeyConverter _hotkeyConverter;
        private readonly HotkeyBinder _hotkeyBinder;
        private LensertClient _client;

        private bool _hotkeyEnabled;

        private IEnumerable<SettingsProperty> Settings => Preferences.Default.Properties.Cast<SettingsProperty>();

        public PreferencesForm()
        {
            _hotkeyConverter = new HotkeyConverter();
            _hotkeyBinder = new HotkeyBinder();
            _client = new LensertClient(Preferences.Default.Username, Preferences.Default.Password);

            _hotkeyEnabled = true;
            
            InitializeComponent();
            InitializeHotkeys();
        }

        private async void HotkeyHandler(ScreenshotType type)
        {
            if (!_hotkeyEnabled)
                return;

            var screenshot = ScreenshotProvider.GetScreenshot(type);
            if (screenshot == null)
                return;

            var link = await _client.UploadImageAsync(screenshot);

            Console.WriteLine($"Got link '{link}'");
            NotificationProvider.Show(link);

            //if (Preferences.Default.CopyToClipboard)                                                                                         
            //    Clipboard.SetText(link);                                                                                                     
        }
        
        void InitializeHotkeys()
        {
            var hotkeySettings = Settings.Where(setting => setting.PropertyType == typeof(Hotkey));
            var items = hotkeySettings.Select(setting => new ListViewItem(new[] {
                                                                                    setting.GetDescription(),
                                                                                    setting.DefaultValue.ToString()
                                                                                }));

            listHotkeys.Items.Clear();
            listHotkeys.Items.AddRange(items.ToArray());

            if (!_hotkeyBinder.IsHotkeyAlreadyBound(Preferences.Default.HotkeySelectFullscreen))
                _hotkeyBinder.Bind(Preferences.Default.HotkeySelectFullscreen).To(() => HotkeyHandler(ScreenshotType.Fullscreen));

            if (!_hotkeyBinder.IsHotkeyAlreadyBound(Preferences.Default.HotkeySelectArea))
                _hotkeyBinder.Bind(Preferences.Default.HotkeySelectArea).To(() => HotkeyHandler(ScreenshotType.Area));

            if (!_hotkeyBinder.IsHotkeyAlreadyBound(Preferences.Default.HotkeySelectCurrentWindow))
                _hotkeyBinder.Bind(Preferences.Default.HotkeySelectCurrentWindow).To(() => HotkeyHandler(ScreenshotType.CurrentWindow));

            if (!_hotkeyBinder.IsHotkeyAlreadyBound(Preferences.Default.HotkeySelectWindow))
                _hotkeyBinder.Bind(Preferences.Default.HotkeySelectWindow).To(() => HotkeyHandler(ScreenshotType.SelectWindow));

            if (!_hotkeyBinder.IsHotkeyAlreadyBound(Preferences.Default.HotkeyClipboard))
                _hotkeyBinder.Bind(Preferences.Default.HotkeyClipboard).To(() => HotkeyHandler(ScreenshotType.Clipboard));
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Preferences.Default.Save();
            Close();
        }

        private void PreferencesForm_Load(object sender, EventArgs e)
        {
            comboboxLanguage.SelectedIndex = 0;
        }

        private void listHotkeys_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listHotkeys.SelectedItems.Count == 0)
                return;

            var hotkey = listHotkeys.SelectedItems[0].SubItems[1].Text;
            if (string.IsNullOrEmpty(hotkey))
                return;
            
            textboxHotkey.Hotkey = (Hotkey)_hotkeyConverter.ConvertFromString(hotkey);
        }

        private void buttonAssign_Click(object sender, EventArgs e)
        {
            if (listHotkeys.SelectedItems.Count == 0)
                return;

            var selectedItem = listHotkeys.SelectedItems[0];
            if (selectedItem == null)
                return;

            var oldHotkey = (Hotkey)_hotkeyConverter.ConvertFromString(selectedItem.SubItems[1].Text);
            var hotkey = textboxHotkey.Hotkey;

            if (oldHotkey == hotkey)                                //don't bother to rebind same hotkey
                return;

            _hotkeyBinder.Unbind(oldHotkey);
            
            var settingName = Util.GetPropertySettingName(selectedItem.Text);
            Preferences.Default[settingName] = hotkey;              //saves hotkey

            InitializeHotkeys();                                    //rebinds if needed
        }

        private void listHotkeys_KeyDown(object sender, KeyEventArgs e)
        {
            textboxHotkey.Select();
        }

        private async void LoginHandler_UI(object sender, EventArgs e)
        {
            var keyEventArgs = e as KeyEventArgs;
            if (keyEventArgs != null)                                   //fired by textbox
            {                                                           //(if not it's the button click)
                if (keyEventArgs.KeyCode != Keys.Enter)                 //check if enter is pressed
                    return;
            }

            var controls = new Control[] { textboxUsername, textboxPassword, buttonLogin };
            foreach (var control in controls)
                control.Enabled = false;                                //disable controls, so can't be activated again

            var username = textboxUsername.Text;
            var password = textboxPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return;                                                 //deny empty boxes :(

            _client = new LensertClient(username, password);
            if (await _client.Login())                                  //maybe give response if login failed?
            {
                Preferences.Default.Username = username;
                Preferences.Default.Password = password;

                Preferences.Default.Save();
            }

            foreach (var control in controls)                           //re-enable controls
                control.Enabled = true;
        }
    }
}
