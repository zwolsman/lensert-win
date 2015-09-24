namespace Lensert.Forms
{
    partial class SelectionForm
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
            this.SuspendLayout();
            // 
            // SelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 261);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SelectionForm";
            this.Opacity = 0.5D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SelectionForm";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Plum;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.SelectionForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SelectionForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectionForm_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SelectionForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SelectionForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SelectionForm_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}