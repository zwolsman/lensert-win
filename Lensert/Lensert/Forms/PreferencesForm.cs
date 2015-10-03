using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
        public event EventHandler<AccountEventArgs> AccountChanged;
        public event EventHandler<HotkeyEventArgs> HotkeyChanged;
        
        public PreferencesForm()
        {
            InitializeComponent();
            InitializeHotkeys();
            LoadPreferences();

            AccountChanged += PreferencesForm_AccountChanged;
        }

        public void Login(string username, string password)
        {
            textboxUsername.Text = username;
            textboxPassword.Text = password;

            LoginHandler_UI(this, EventArgs.Empty);
        }

        private void PreferencesForm_AccountChanged(object sender, AccountEventArgs e)
        {
            if (e.LensertClient == null)
            {                                                                   //resets controls due log out
                labelLogin.Text = "Sign in to your account";
                labelLogin.ForeColor = SystemColors.ControlText;

                textboxUsername.Text = "";
                textboxPassword.Text = "";

                buttonLogin.Text = "Sign in";
            }
            else if (e.LensertClient.LoggedIn)
            {                                                                   //shows logged in state
                labelLogin.Text = "Logged in.";
                labelLogin.ForeColor = Color.Green;

                buttonLogin.Text = "Log out";

                this.EnableControls(false, textboxUsername, textboxPassword);
            }
            else
            {                                                                   //invalid login
                labelLogin.Text = "Invalid credentials.";
                labelLogin.ForeColor = Color.Red;
            }
        }
        
        private void LoadPreferences()
        {
            chRememberMe.Checked = Preferences.Default.RememberMe;
            cbCopyLink.Checked = Preferences.Default.CopyToClipboard;
            cbNotify.Checked = Preferences.Default.ShowNotification;
        }

        private void SavePreferences()
        {
            Preferences.Default.RememberMe = chRememberMe.Checked;
            Preferences.Default.CopyToClipboard = cbCopyLink.Checked;
            Preferences.Default.ShowNotification = cbNotify.Checked;
            Preferences.Default.Save();
        }

        private void InitializeHotkeys()
        {
            listHotkeys.Items.Clear();

            var hotkeySettings = Utils.SettingsOfType(typeof(Hotkey));
            var items = hotkeySettings.Select(setting => new ListViewItem(new[] {
                                                                                    setting.GetSettingDescription(),
                                                                                    setting.PropertyValue.ToString()
                                                                                }));

            listHotkeys.Items.AddRange(items.ToArray());
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            SavePreferences();
            Close();
        }

        private void PreferencesForm_Load(object sender, EventArgs e)
        {
            comboboxLanguage.SelectedIndex = 0;
            tabControl1.TabPages.Remove(tabPersonal);
        }

        private void listHotkeys_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listHotkeys.SelectedItems.Count == 0)
                return;

            var hotkey = listHotkeys.SelectedItems[0].SubItems[1].Text;
            if (string.IsNullOrEmpty(hotkey))
                return;
            
            textboxHotkey.Hotkey = Utils.ConvertToHotkey(hotkey);
        }

        private void buttonAssign_Click(object sender, EventArgs e)
        {
            if (listHotkeys.SelectedItems.Count == 0)
                return;

            var selectedItem = listHotkeys.SelectedItems[0];
            if (selectedItem == null)
                return;

            var oldHotkey = Utils.ConvertToHotkey(selectedItem.SubItems[1].Text);
            var newHotkey = textboxHotkey.Hotkey;

            if (oldHotkey == newHotkey)                                //don't bother to rebind same hotkey
                return;

            var setting = Utils.FindSettingByValue(oldHotkey);
            var settingName = setting.Name;

            Preferences.Default[settingName] = newHotkey;
            Preferences.Default.Save();
            
            HotkeyChanged?.Invoke(this, new HotkeyEventArgs(oldHotkey, newHotkey));
            InitializeHotkeys();
        }


        private void listHotkeys_KeyDown(object sender, KeyEventArgs e)
        {
            textboxHotkey.Select();
        }

        private async void LoginHandler_UI(object sender, EventArgs e)
        {
            var keyEventArgs = e as KeyEventArgs;
            if (keyEventArgs != null)                       //fired by textbox
            {                                               //(if not it's the button click)
                if (keyEventArgs.KeyCode != Keys.Enter)     //check if enter is pressed
                    return;
                keyEventArgs.SuppressKeyPress = true;       //Stop the beeping from happening
                keyEventArgs.Handled = true;                //Stop the beeping from happening
            }

            this.EnableControls(false, textboxUsername, textboxPassword, buttonLogin);   //disable controls, so can't be activated again

            var username = textboxUsername.Text;
            var password = textboxPassword.Text;

            LensertClient client = null;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && buttonLogin.Text == "Sign in")
            {                                               //deny empty boxes :(
                client = new LensertClient(username, password);
                if (await client.Login())                   //maybe give response if login failed?
                {
                    Preferences.Default.Username = username;
                    Preferences.Default.Password = password;

                    Preferences.Default.Save();
                }
            }

            this.EnableControls(true, textboxUsername, textboxPassword, buttonLogin);   //Re-enable them :)
            AccountChanged?.Invoke(this, new AccountEventArgs(client));
        }

        private void labelLogin_TextChanged(object sender, EventArgs e)
        {
            var parent = labelLogin.Parent.ClientRectangle;
            labelLogin.Left = (parent.Width - labelLogin.Width) / 2;
        }

        
    }
}
