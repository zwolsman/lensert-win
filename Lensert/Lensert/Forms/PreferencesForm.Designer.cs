﻿using Lensert.Classes.Controls;

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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.comboboxLanguage = new System.Windows.Forms.ComboBox();
            this.cbNotify = new System.Windows.Forms.CheckBox();
            this.cbCopyLink = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonAssign = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textboxHotkey = new Shortcut.Forms.HotkeyTextBox();
            this.listHotkeys = new Lensert.Classes.Controls.ExplorerListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.pnlConnectFB = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.pnlSignin = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.textboxUsername = new Lensert.Classes.Controls.CueTextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textboxPassword = new Lensert.Classes.Controls.CueTextBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.pnlConnectFB.SuspendLayout();
            this.pnlSignin.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::Lensert.Properties.Resources.Knipsel;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(549, 163);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(14, 169);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(521, 212);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.comboboxLanguage);
            this.tabPage1.Controls.Add(this.cbNotify);
            this.tabPage1.Controls.Add(this.cbCopyLink);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(513, 186);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Language";
            // 
            // comboboxLanguage
            // 
            this.comboboxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboboxLanguage.FormattingEnabled = true;
            this.comboboxLanguage.Items.AddRange(new object[] {
            "English"});
            this.comboboxLanguage.Location = new System.Drawing.Point(9, 159);
            this.comboboxLanguage.Name = "comboboxLanguage";
            this.comboboxLanguage.Size = new System.Drawing.Size(202, 21);
            this.comboboxLanguage.TabIndex = 2;
            // 
            // cbNotify
            // 
            this.cbNotify.AutoSize = true;
            this.cbNotify.Location = new System.Drawing.Point(6, 29);
            this.cbNotify.Name = "cbNotify";
            this.cbNotify.Size = new System.Drawing.Size(255, 17);
            this.cbNotify.TabIndex = 1;
            this.cbNotify.Text = "Show notification after succesfull upload";
            this.cbNotify.UseVisualStyleBackColor = true;
            // 
            // cbCopyLink
            // 
            this.cbCopyLink.AutoSize = true;
            this.cbCopyLink.Location = new System.Drawing.Point(6, 6);
            this.cbCopyLink.Name = "cbCopyLink";
            this.cbCopyLink.Size = new System.Drawing.Size(290, 17);
            this.cbCopyLink.TabIndex = 0;
            this.cbCopyLink.Text = "Automatically copy link after succesfull upload";
            this.cbCopyLink.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonAssign);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.textboxHotkey);
            this.tabPage2.Controls.Add(this.listHotkeys);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(513, 186);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Hotkeys";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonAssign
            // 
            this.buttonAssign.Location = new System.Drawing.Point(180, 150);
            this.buttonAssign.Name = "buttonAssign";
            this.buttonAssign.Size = new System.Drawing.Size(75, 21);
            this.buttonAssign.TabIndex = 3;
            this.buttonAssign.Text = "Assign";
            this.buttonAssign.UseVisualStyleBackColor = true;
            this.buttonAssign.Click += new System.EventHandler(this.buttonAssign_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(249, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Press (new) hotkey for selected command";
            // 
            // textboxHotkey
            // 
            this.textboxHotkey.Hotkey = ((Shortcut.Hotkey)(resources.GetObject("textboxHotkey.Hotkey")));
            this.textboxHotkey.Location = new System.Drawing.Point(9, 150);
            this.textboxHotkey.Name = "textboxHotkey";
            this.textboxHotkey.ReadOnly = true;
            this.textboxHotkey.Size = new System.Drawing.Size(165, 21);
            this.textboxHotkey.TabIndex = 1;
            this.textboxHotkey.Text = "None";
            // 
            // listHotkeys
            // 
            this.listHotkeys.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listHotkeys.FullRowSelect = true;
            this.listHotkeys.Location = new System.Drawing.Point(6, 6);
            this.listHotkeys.MultiSelect = false;
            this.listHotkeys.Name = "listHotkeys";
            this.listHotkeys.Size = new System.Drawing.Size(501, 123);
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
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.pictureBox2);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.linkLabel2);
            this.tabPage3.Controls.Add(this.pnlConnectFB);
            this.tabPage3.Controls.Add(this.pnlSignin);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(513, 186);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Account";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.pictureBox2.Location = new System.Drawing.Point(257, 30);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(1, 122);
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label3.Location = new System.Drawing.Point(160, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "No account yet?";
            // 
            // linkLabel2
            // 
            this.linkLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.LinkColor = System.Drawing.SystemColors.Highlight;
            this.linkLabel2.Location = new System.Drawing.Point(264, 155);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(77, 13);
            this.linkLabel2.TabIndex = 8;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Sign up now";
            // 
            // pnlConnectFB
            // 
            this.pnlConnectFB.Controls.Add(this.label2);
            this.pnlConnectFB.Controls.Add(this.button2);
            this.pnlConnectFB.Location = new System.Drawing.Point(7, 46);
            this.pnlConnectFB.Name = "pnlConnectFB";
            this.pnlConnectFB.Size = new System.Drawing.Size(240, 60);
            this.pnlConnectFB.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Connect with Facebook\r\nfor easy access";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(5, 32);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(228, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Sign in with Facebook";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // pnlSignin
            // 
            this.pnlSignin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSignin.Controls.Add(this.label1);
            this.pnlSignin.Controls.Add(this.linkLabel1);
            this.pnlSignin.Controls.Add(this.textboxUsername);
            this.pnlSignin.Controls.Add(this.checkBox1);
            this.pnlSignin.Controls.Add(this.textboxPassword);
            this.pnlSignin.Controls.Add(this.buttonLogin);
            this.pnlSignin.Location = new System.Drawing.Point(267, 30);
            this.pnlSignin.Name = "pnlSignin";
            this.pnlSignin.Size = new System.Drawing.Size(240, 122);
            this.pnlSignin.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sign in to your account";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkColor = System.Drawing.SystemColors.Highlight;
            this.linkLabel1.Location = new System.Drawing.Point(124, 100);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(111, 13);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Request Password";
            // 
            // textboxUsername
            // 
            this.textboxUsername.Cue = "Username";
            this.textboxUsername.Location = new System.Drawing.Point(7, 16);
            this.textboxUsername.Name = "textboxUsername";
            this.textboxUsername.Size = new System.Drawing.Size(228, 21);
            this.textboxUsername.TabIndex = 0;
            this.textboxUsername.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginHandler_UI);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(7, 99);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(111, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Remember me";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textboxPassword
            // 
            this.textboxPassword.Cue = "Password";
            this.textboxPassword.Location = new System.Drawing.Point(7, 43);
            this.textboxPassword.Name = "textboxPassword";
            this.textboxPassword.Size = new System.Drawing.Size(228, 21);
            this.textboxPassword.TabIndex = 2;
            this.textboxPassword.UseSystemPasswordChar = true;
            this.textboxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginHandler_UI);
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(7, 70);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(228, 23);
            this.buttonLogin.TabIndex = 3;
            this.buttonLogin.Text = "Sign in";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.LoginHandler_UI);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(462, 387);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // PreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 422);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "PreferencesForm";
            this.Text = "Lensert - Preferences";
            this.Load += new System.EventHandler(this.PreferencesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.pnlConnectFB.ResumeLayout(false);
            this.pnlConnectFB.PerformLayout();
            this.pnlSignin.ResumeLayout(false);
            this.pnlSignin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Panel pnlConnectFB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel pnlSignin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private Classes.Controls.CueTextBox textboxUsername;
        private System.Windows.Forms.CheckBox checkBox1;
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
    }
}