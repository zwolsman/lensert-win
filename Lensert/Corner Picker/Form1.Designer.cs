namespace Corner_Picker
{
    partial class Form1
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
            this.picUpLeft = new System.Windows.Forms.PictureBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.picUpRight = new System.Windows.Forms.PictureBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.picLowerRight = new System.Windows.Forms.PictureBox();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.picLowerLeft = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picUpLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUpRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLowerRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLowerLeft)).BeginInit();
            this.SuspendLayout();
            // 
            // picUpLeft
            // 
            this.picUpLeft.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picUpLeft.Location = new System.Drawing.Point(14, 35);
            this.picUpLeft.Name = "picUpLeft";
            this.picUpLeft.Size = new System.Drawing.Size(94, 76);
            this.picUpLeft.TabIndex = 0;
            this.picUpLeft.TabStop = false;
            this.picUpLeft.Click += new System.EventHandler(this.picUpLeft_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(14, 12);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(81, 17);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Upper left";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(119, 12);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radioButton2.Size = new System.Drawing.Size(89, 17);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Upper right";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // picUpRight
            // 
            this.picUpRight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picUpRight.Location = new System.Drawing.Point(114, 35);
            this.picUpRight.Name = "picUpRight";
            this.picUpRight.Size = new System.Drawing.Size(94, 76);
            this.picUpRight.TabIndex = 2;
            this.picUpRight.TabStop = false;
            this.picUpRight.Click += new System.EventHandler(this.picUpRight_Click);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(119, 199);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radioButton3.Size = new System.Drawing.Size(89, 17);
            this.radioButton3.TabIndex = 7;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Lower right";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // picLowerRight
            // 
            this.picLowerRight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picLowerRight.Location = new System.Drawing.Point(114, 117);
            this.picLowerRight.Name = "picLowerRight";
            this.picLowerRight.Size = new System.Drawing.Size(94, 76);
            this.picLowerRight.TabIndex = 6;
            this.picLowerRight.TabStop = false;
            this.picLowerRight.Click += new System.EventHandler(this.picLowerRight_Click);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(12, 199);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(81, 17);
            this.radioButton4.TabIndex = 5;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Lower left";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // picLowerLeft
            // 
            this.picLowerLeft.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picLowerLeft.Location = new System.Drawing.Point(14, 117);
            this.picLowerLeft.Name = "picLowerLeft";
            this.picLowerLeft.Size = new System.Drawing.Size(94, 76);
            this.picLowerLeft.TabIndex = 4;
            this.picLowerLeft.TabStop = false;
            this.picLowerLeft.Click += new System.EventHandler(this.picLowerLeft_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 222);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(196, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Random snapshot";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(225, 257);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.picLowerRight);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.picLowerLeft);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.picUpRight);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.picUpLeft);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picUpLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUpRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLowerRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLowerLeft)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picUpLeft;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.PictureBox picUpRight;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.PictureBox picLowerRight;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.PictureBox picLowerLeft;
        private System.Windows.Forms.Button button1;
    }
}

