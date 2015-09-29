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

        public event EventHandler<AccountEventArgs> AccountChanged;
        public event EventHandler<HotkeyEventArgs> HotkeyChanged;
        
        public PreferencesForm()
        {
            _hotkeyConverter = new HotkeyConverter();
            
            InitializeComponent();
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
            var newHotkey = textboxHotkey.Hotkey;

            if (oldHotkey == newHotkey)                                //don't bother to rebind same hotkey
                return;
            
            var settingName = Utils.GetPropertySettingName(selectedItem.Text);
            Preferences.Default[settingName] = newHotkey;              //saves hotkey

            HotkeyChanged?.Invoke(this, new HotkeyEventArgs(oldHotkey, newHotkey));
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

            var client = new LensertClient(username, password);
            if (await client.Login())                                  //maybe give response if login failed?
            {
                Preferences.Default.Username = username;
                Preferences.Default.Password = password;

                Preferences.Default.Save();
                
            }

            AccountChanged?.Invoke(this, new AccountEventArgs(client));

            foreach (var control in controls)                           //re-enable controls
                control.Enabled = true;
        }
    }
}
