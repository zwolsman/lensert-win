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
        private readonly HotkeyBinder _hotkeyBinder;
        private LensertClient _client;

        private IEnumerable<SettingsProperty> Settings => Preferences.Default.Properties.Cast<SettingsProperty>();

        public PreferencesForm()
        {
            InitializeComponent();
            InitializeHotkeys();
/*

            #################
            # Preview items #
            #################

            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Upload fullscreen screenshot",
            "None"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Upload active window",
            "None"}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Select a window to upload",
            "None"}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Select an area to upload",
            "None"}, -1);
            this.listHotkeys.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4});*/

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
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Preferences.Default.Save();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PreferencesForm_Load(object sender, EventArgs e)
        {
            cbLang.SelectedIndex = 0;
        }


        private readonly HotkeyConverter hotkeyConverter = new HotkeyConverter();
        private void listHotkeys_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem selectedItem = null;
            if (listHotkeys.SelectedItems.Count >= 1)
            {
                selectedItem = listHotkeys.SelectedItems[0];
            }

            if (selectedItem == null)
                return;

            txtHotkey.Hotkey = hotkeyConverter.ConvertFromString(selectedItem.Tag.ToString()) as Hotkey;
        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            //TODO fix the saving/assigning
            ListViewItem selectedItem = null;
            if (listHotkeys.SelectedItems.Count >= 1)
            {
                selectedItem = listHotkeys.SelectedItems[0];
            }

            if (selectedItem == null)
                return;

            Preferences.Default[selectedItem.Text] = txtHotkey.Hotkey;
            InitializeHotkeys();
        }

        private void listHotkeys_KeyDown(object sender, KeyEventArgs e)
        {
            txtHotkey.Select();
        }
    }
}
