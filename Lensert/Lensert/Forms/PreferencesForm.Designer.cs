using Lensert.Classes.Controls;

namespace Lensert
{
    partial class PreferencesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreferencesForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.comboboxLanguage = new System.Windows.Forms.ComboBox();
            this.cbNotify = new System.Windows.Forms.CheckBox();
            this.cbCopyLink = new System.Windows.Forms.CheckBox();
            this.tabHotkeys = new System.Windows.Forms.TabPage();
            this.buttonAssign = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textboxHotkey = new Shortcut.Forms.HotkeyTextBox();
            this.listHotkeys = new Lensert.Classes.Controls.ExplorerListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabAccount = new System.Windows.Forms.TabPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.pnlConnectFB = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.pnlSignin = new System.Windows.Forms.Panel();
            this.labelLogin = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.textboxUsername = new Lensert.Classes.Controls.CueTextBox();
            this.chRememberMe = new System.Windows.Forms.CheckBox();
            this.textboxPassword = new Lensert.Classes.Controls.CueTextBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.tabPersonal = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.buttonOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabHotkeys.SuspendLayout();
            this.tabAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.pnlConnectFB.SuspendLayout();
            this.pnlSignin.SuspendLayout();
            this.tabPersonal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::Lensert.Properties.Resources.Knipsel;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(824, 244);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabHotkeys);
            this.tabControl1.Controls.Add(this.tabAccount);
            this.tabControl1.Controls.Add(this.tabPersonal);
            this.tabControl1.Location = new System.Drawing.Point(21, 254);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(782, 318);
            this.tabControl1.TabIndex = 1;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.label4);
            this.tabGeneral.Controls.Add(this.comboboxLanguage);
            this.tabGeneral.Controls.Add(this.cbNotify);
            this.tabGeneral.Controls.Add(this.cbCopyLink);
            this.tabGeneral.Location = new System.Drawing.Point(4, 29);
            this.tabGeneral.Margin = new System.Windows.Forms.Padding(4);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(4);
            this.tabGeneral.Size = new System.Drawing.Size(774, 285);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 214);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Language";
            // 
            // comboboxLanguage
            // 
            this.comboboxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboboxLanguage.FormattingEnabled = true;
            this.comboboxLanguage.Items.AddRange(new object[] {
            "English"});
            this.comboboxLanguage.Location = new System.Drawing.Point(14, 238);
            this.comboboxLanguage.Margin = new System.Windows.Forms.Padding(4);
            this.comboboxLanguage.Name = "comboboxLanguage";
            this.comboboxLanguage.Size = new System.Drawing.Size(301, 28);
            this.comboboxLanguage.TabIndex = 2;
            // 
            // cbNotify
            // 
            this.cbNotify.AutoSize = true;
            this.cbNotify.Location = new System.Drawing.Point(9, 44);
            this.cbNotify.Margin = new System.Windows.Forms.Padding(4);
            this.cbNotify.Name = "cbNotify";
            this.cbNotify.Size = new System.Drawing.Size(384, 24);
            this.cbNotify.TabIndex = 1;
            this.cbNotify.Text = "Show notification after succesfull upload";
            this.cbNotify.UseVisualStyleBackColor = true;
            // 
            // cbCopyLink
            // 
            this.cbCopyLink.AutoSize = true;
            this.cbCopyLink.Location = new System.Drawing.Point(9, 9);
            this.cbCopyLink.Margin = new System.Windows.Forms.Padding(4);
            this.cbCopyLink.Name = "cbCopyLink";
            this.cbCopyLink.Size = new System.Drawing.Size(436, 24);
            this.cbCopyLink.TabIndex = 0;
            this.cbCopyLink.Text = "Automatically copy link after succesfull upload";
            this.cbCopyLink.UseVisualStyleBackColor = true;
            // 
            // tabHotkeys
            // 
            this.tabHotkeys.Controls.Add(this.buttonAssign);
            this.tabHotkeys.Controls.Add(this.label5);
            this.tabHotkeys.Controls.Add(this.textboxHotkey);
            this.tabHotkeys.Controls.Add(this.listHotkeys);
            this.tabHotkeys.Location = new System.Drawing.Point(4, 29);
            this.tabHotkeys.Margin = new System.Windows.Forms.Padding(4);
            this.tabHotkeys.Name = "tabHotkeys";
            this.tabHotkeys.Padding = new System.Windows.Forms.Padding(4);
            this.tabHotkeys.Size = new System.Drawing.Size(774, 285);
            this.tabHotkeys.TabIndex = 1;
            this.tabHotkeys.Text = "Hotkeys";
            this.tabHotkeys.UseVisualStyleBackColor = true;
            // 
            // buttonAssign
            // 
            this.buttonAssign.Location = new System.Drawing.Point(270, 225);
            this.buttonAssign.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAssign.Name = "buttonAssign";
            this.buttonAssign.Size = new System.Drawing.Size(112, 32);
            this.buttonAssign.TabIndex = 3;
            this.buttonAssign.Text = "Assign";
            this.buttonAssign.UseVisualStyleBackColor = true;
            this.buttonAssign.Click += new System.EventHandler(this.buttonAssign_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 198);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(372, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "Press (new) hotkey for selected command";
            // 
            // textboxHotkey
            // 
            this.textboxHotkey.Hotkey = null;
            this.textboxHotkey.Location = new System.Drawing.Point(14, 225);
            this.textboxHotkey.Margin = new System.Windows.Forms.Padding(4);
            this.textboxHotkey.Name = "textboxHotkey";
            this.textboxHotkey.ReadOnly = true;
            this.textboxHotkey.Size = new System.Drawing.Size(246, 28);
            this.textboxHotkey.TabIndex = 1;
            this.textboxHotkey.Text = "None";
            // 
            // listHotkeys
            // 
            this.listHotkeys.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listHotkeys.FullRowSelect = true;
            this.listHotkeys.Location = new System.Drawing.Point(9, 9);
            this.listHotkeys.Margin = new System.Windows.Forms.Padding(4);
            this.listHotkeys.MultiSelect = false;
            this.listHotkeys.Name = "listHotkeys";
            this.listHotkeys.Size = new System.Drawing.Size(750, 182);
            this.listHotkeys.TabIndex = 0;
            this.listHotkeys.UseCompatibleStateImageBehavior = false;
            this.listHotkeys.View = System.Windows.Forms.View.Details;
            this.listHotkeys.SelectedIndexChanged += new System.EventHandler(this.listHotkeys_SelectedIndexChanged);
            this.listHotkeys.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listHotkeys_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Command";
            this.columnHeader1.Width = 224;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Key";
            this.columnHeader2.Width = 225;
            // 
            // tabAccount
            // 
            this.tabAccount.Controls.Add(this.pictureBox2);
            this.tabAccount.Controls.Add(this.label3);
            this.tabAccount.Controls.Add(this.linkLabel2);
            this.tabAccount.Controls.Add(this.pnlConnectFB);
            this.tabAccount.Controls.Add(this.pnlSignin);
            this.tabAccount.Location = new System.Drawing.Point(4, 29);
            this.tabAccount.Margin = new System.Windows.Forms.Padding(4);
            this.tabAccount.Name = "tabAccount";
            this.tabAccount.Padding = new System.Windows.Forms.Padding(4);
            this.tabAccount.Size = new System.Drawing.Size(774, 285);
            this.tabAccount.TabIndex = 2;
            this.tabAccount.Text = "Account";
            this.tabAccount.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.pictureBox2.Location = new System.Drawing.Point(386, 45);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(2, 183);
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label3.Location = new System.Drawing.Point(240, 232);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "No account yet?";
            // 
            // linkLabel2
            // 
            this.linkLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.LinkColor = System.Drawing.SystemColors.Highlight;
            this.linkLabel2.Location = new System.Drawing.Point(396, 232);
            this.linkLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(117, 20);
            this.linkLabel2.TabIndex = 8;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Sign up now";
            // 
            // pnlConnectFB
            // 
            this.pnlConnectFB.Controls.Add(this.label2);
            this.pnlConnectFB.Controls.Add(this.button2);
            this.pnlConnectFB.Location = new System.Drawing.Point(10, 69);
            this.pnlConnectFB.Margin = new System.Windows.Forms.Padding(4);
            this.pnlConnectFB.Name = "pnlConnectFB";
            this.pnlConnectFB.Size = new System.Drawing.Size(360, 90);
            this.pnlConnectFB.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 40);
            this.label2.TabIndex = 1;
            this.label2.Text = "Connect with Facebook\r\nfor easy access";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(8, 48);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(342, 34);
            this.button2.TabIndex = 3;
            this.button2.Text = "Sign in with Facebook";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // pnlSignin
            // 
            this.pnlSignin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSignin.Controls.Add(this.labelLogin);
            this.pnlSignin.Controls.Add(this.linkLabel1);
            this.pnlSignin.Controls.Add(this.textboxUsername);
            this.pnlSignin.Controls.Add(this.chRememberMe);
            this.pnlSignin.Controls.Add(this.textboxPassword);
            this.pnlSignin.Controls.Add(this.buttonLogin);
            this.pnlSignin.Location = new System.Drawing.Point(400, 45);
            this.pnlSignin.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSignin.Name = "pnlSignin";
            this.pnlSignin.Size = new System.Drawing.Size(360, 183);
            this.pnlSignin.TabIndex = 6;
            // 
            // labelLogin
            // 
            this.labelLogin.Location = new System.Drawing.Point(78, 0);
            this.labelLogin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new System.Drawing.Size(210, 20);
            this.labelLogin.TabIndex = 1;
            this.labelLogin.Text = "Sign in to your account";
            this.labelLogin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelLogin.TextChanged += new System.EventHandler(this.labelLogin_TextChanged);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkColor = System.Drawing.SystemColors.Highlight;
            this.linkLabel1.Location = new System.Drawing.Point(186, 150);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(165, 20);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Request Password";
            // 
            // textboxUsername
            // 
            this.textboxUsername.Cue = "Username";
            this.textboxUsername.Location = new System.Drawing.Point(10, 24);
            this.textboxUsername.Margin = new System.Windows.Forms.Padding(4);
            this.textboxUsername.Name = "textboxUsername";
            this.textboxUsername.Size = new System.Drawing.Size(340, 28);
            this.textboxUsername.TabIndex = 0;
            this.textboxUsername.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginHandler_UI);
            // 
            // chRememberMe
            // 
            this.chRememberMe.AutoSize = true;
            this.chRememberMe.Location = new System.Drawing.Point(10, 148);
            this.chRememberMe.Margin = new System.Windows.Forms.Padding(4);
            this.chRememberMe.Name = "chRememberMe";
            this.chRememberMe.Size = new System.Drawing.Size(162, 24);
            this.chRememberMe.TabIndex = 4;
            this.chRememberMe.Text = "Remember me";
            this.chRememberMe.UseVisualStyleBackColor = true;
            // 
            // textboxPassword
            // 
            this.textboxPassword.Cue = "Password";
            this.textboxPassword.Location = new System.Drawing.Point(10, 64);
            this.textboxPassword.Margin = new System.Windows.Forms.Padding(4);
            this.textboxPassword.Name = "textboxPassword";
            this.textboxPassword.Size = new System.Drawing.Size(340, 28);
            this.textboxPassword.TabIndex = 2;
            this.textboxPassword.UseSystemPasswordChar = true;
            this.textboxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginHandler_UI);
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(10, 105);
            this.buttonLogin.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(342, 34);
            this.buttonLogin.TabIndex = 3;
            this.buttonLogin.Text = "Sign in";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.LoginHandler_UI);
            // 
            // tabPersonal
            // 
            this.tabPersonal.Controls.Add(this.label12);
            this.tabPersonal.Controls.Add(this.label13);
            this.tabPersonal.Controls.Add(this.label10);
            this.tabPersonal.Controls.Add(this.label11);
            this.tabPersonal.Controls.Add(this.label9);
            this.tabPersonal.Controls.Add(this.label8);
            this.tabPersonal.Controls.Add(this.lblDescription);
            this.tabPersonal.Controls.Add(this.lblUsername);
            this.tabPersonal.Controls.Add(this.pictureBox3);
            this.tabPersonal.Location = new System.Drawing.Point(4, 29);
            this.tabPersonal.Margin = new System.Windows.Forms.Padding(4);
            this.tabPersonal.Name = "tabPersonal";
            this.tabPersonal.Padding = new System.Windows.Forms.Padding(4);
            this.tabPersonal.Size = new System.Drawing.Size(774, 285);
            this.tabPersonal.TabIndex = 3;
            this.tabPersonal.Text = "Personal";
            this.tabPersonal.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(519, 56);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 20);
            this.label12.TabIndex = 6;
            this.label12.Text = "views";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(490, 56);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(32, 20);
            this.label13.TabIndex = 5;
            this.label13.Text = "1k";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(378, 56);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(99, 20);
            this.label10.TabIndex = 6;
            this.label10.Text = "comments";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(336, 56);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 20);
            this.label11.TabIndex = 5;
            this.label11.Text = "215";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(250, 56);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 20);
            this.label9.TabIndex = 4;
            this.label9.Text = "uploads";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(208, 56);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 20);
            this.label8.TabIndex = 3;
            this.label8.Text = "116";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(208, 36);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(265, 20);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "Creater and owner of Lensert.";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.Location = new System.Drawing.Point(208, 9);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(79, 28);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "murfz";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::Lensert.Properties.Resources.testpf;
            this.pictureBox3.Location = new System.Drawing.Point(9, 9);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(190, 171);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(693, 580);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(112, 34);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // PreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(824, 633);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "PreferencesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lensert - Preferences";
            this.Load += new System.EventHandler(this.PreferencesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.tabHotkeys.ResumeLayout(false);
            this.tabHotkeys.PerformLayout();
            this.tabAccount.ResumeLayout(false);
            this.tabAccount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.pnlConnectFB.ResumeLayout(false);
            this.pnlConnectFB.PerformLayout();
            this.pnlSignin.ResumeLayout(false);
            this.pnlSignin.PerformLayout();
            this.tabPersonal.ResumeLayout(false);
            this.tabPersonal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabHotkeys;
        private System.Windows.Forms.TabPage tabAccount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Panel pnlConnectFB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel pnlSignin;
        private System.Windows.Forms.Label labelLogin;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private Classes.Controls.CueTextBox textboxUsername;
        private System.Windows.Forms.CheckBox chRememberMe;
        private Classes.Controls.CueTextBox textboxPassword;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.CheckBox cbNotify;
        private System.Windows.Forms.CheckBox cbCopyLink;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.ComboBox comboboxLanguage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonAssign;
        private System.Windows.Forms.Label label5;
        private Shortcut.Forms.HotkeyTextBox textboxHotkey;
        private Lensert.Classes.Controls.ExplorerListView listHotkeys;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TabPage tabPersonal;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblUsername;
    }
}