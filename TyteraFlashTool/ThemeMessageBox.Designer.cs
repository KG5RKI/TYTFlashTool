namespace TyteraFlashTool
{
    partial class ThemeMessageBox
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
            this.Theme = new NSTheme();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.OkayButton = new NSButton();
            this.Theme.SuspendLayout();
            this.SuspendLayout();
            // 
            // Theme
            // 
            this.Theme.AccentOffset = 42;
            this.Theme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Theme.BorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Theme.Colors = new Bloom[0];
            this.Theme.Controls.Add(this.MessageLabel);
            this.Theme.Controls.Add(this.OkayButton);
            this.Theme.Customization = "";
            this.Theme.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Theme.Font = new System.Drawing.Font("Verdana", 8F);
            this.Theme.Image = null;
            this.Theme.Location = new System.Drawing.Point(0, 0);
            this.Theme.Movable = true;
            this.Theme.Name = "Theme";
            this.Theme.NoRounding = false;
            this.Theme.Sizable = true;
            this.Theme.Size = new System.Drawing.Size(284, 101);
            this.Theme.SmartBounds = true;
            this.Theme.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Theme.TabIndex = 0;
            this.Theme.Text = "Tytera Flash Tool";
            this.Theme.TransparencyKey = System.Drawing.Color.Empty;
            this.Theme.Transparent = false;
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.BackColor = System.Drawing.Color.Transparent;
            this.MessageLabel.Font = new System.Drawing.Font("Verdana", 9F);
            this.MessageLabel.ForeColor = System.Drawing.Color.White;
            this.MessageLabel.Location = new System.Drawing.Point(12, 45);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(23, 14);
            this.MessageLabel.TabIndex = 10;
            this.MessageLabel.Text = "....";
            // 
            // OkayButton
            // 
            this.OkayButton.Location = new System.Drawing.Point(226, 66);
            this.OkayButton.Name = "OkayButton";
            this.OkayButton.Size = new System.Drawing.Size(46, 23);
            this.OkayButton.TabIndex = 1;
            this.OkayButton.Text = "Okay";
            this.OkayButton.Click += new System.EventHandler(this.OkayButton_Click);
            // 
            // ThemeMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 101);
            this.Controls.Add(this.Theme);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ThemeMessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ThemeMessageBox";
            this.Theme.ResumeLayout(false);
            this.Theme.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private NSTheme Theme;
        private NSButton OkayButton;
        private System.Windows.Forms.Label MessageLabel;
    }
}