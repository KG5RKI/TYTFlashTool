namespace TyteraFlashTool
{
    partial class TyteraFlashToolForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TyteraFlashToolForm));
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.nsTheme = new NSTheme();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.nsProgressBar = new NSProgressBar();
            this.GroupBox = new NSGroupBox();
            this.WriteButton = new NSButton();
            this.ReadButton = new NSButton();
            this.FlashFirmwareFileButton = new NSButton();
            this.FlashFirmwareButton = new NSButton();
            this.DownloadNONGPSButton = new NSButton();
            this.DownloadGPSButton = new NSButton();
            this.FlashfromFileUserDBButton = new NSButton();
            this.FlashUserDBButton = new NSButton();
            this.DownloadUserDBButton = new NSButton();
            this.nsSeperator3 = new NSSeperator();
            this.nsLabel3 = new NSLabel();
            this.nsLabel2 = new NSLabel();
            this.nsLabel1 = new NSLabel();
            this.nsSeperator1 = new NSSeperator();
            this.OpenButton = new NSButton();
            this.FilenameLabel = new NSLabel();
            this.FilenameTextBox = new NSTextBox();
            this.ExitButton = new NSButton();
            this.nsTheme.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // Timer
            // 
            this.Timer.Interval = 5;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // nsTheme
            // 
            this.nsTheme.AccentOffset = 42;
            this.nsTheme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.nsTheme.BorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.nsTheme.Colors = new Bloom[0];
            this.nsTheme.Controls.Add(this.pictureBox1);
            this.nsTheme.Controls.Add(this.StatusLabel);
            this.nsTheme.Controls.Add(this.nsProgressBar);
            this.nsTheme.Controls.Add(this.GroupBox);
            this.nsTheme.Controls.Add(this.OpenButton);
            this.nsTheme.Controls.Add(this.FilenameLabel);
            this.nsTheme.Controls.Add(this.FilenameTextBox);
            this.nsTheme.Controls.Add(this.ExitButton);
            this.nsTheme.Customization = "";
            this.nsTheme.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nsTheme.Font = new System.Drawing.Font("Verdana", 8F);
            this.nsTheme.Image = null;
            this.nsTheme.Location = new System.Drawing.Point(0, 0);
            this.nsTheme.Movable = true;
            this.nsTheme.Name = "nsTheme";
            this.nsTheme.NoRounding = false;
            this.nsTheme.Sizable = false;
            this.nsTheme.Size = new System.Drawing.Size(384, 260);
            this.nsTheme.SmartBounds = true;
            this.nsTheme.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.nsTheme.TabIndex = 0;
            this.nsTheme.Text = "Tytera Flash Tool  v1.04                                     by KG5RKI";
            this.nsTheme.TransparencyKey = System.Drawing.Color.Empty;
            this.nsTheme.Transparent = false;
            this.nsTheme.Click += new System.EventHandler(this.nsTheme_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(341, 222);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 31);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.BackColor = System.Drawing.Color.Transparent;
            this.StatusLabel.Font = new System.Drawing.Font("Verdana", 9F);
            this.StatusLabel.ForeColor = System.Drawing.Color.White;
            this.StatusLabel.Location = new System.Drawing.Point(22, 229);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(23, 14);
            this.StatusLabel.TabIndex = 9;
            this.StatusLabel.Text = "....";
            // 
            // nsProgressBar
            // 
            this.nsProgressBar.Location = new System.Drawing.Point(15, 225);
            this.nsProgressBar.Name = "nsProgressBar";
            this.nsProgressBar.Size = new System.Drawing.Size(320, 23);
            this.nsProgressBar.TabIndex = 16;
            this.nsProgressBar.Click += new System.EventHandler(this.nsProgressBar_Click);
            // 
            // GroupBox
            // 
            this.GroupBox.Controls.Add(this.WriteButton);
            this.GroupBox.Controls.Add(this.ReadButton);
            this.GroupBox.Controls.Add(this.FlashFirmwareFileButton);
            this.GroupBox.Controls.Add(this.FlashFirmwareButton);
            this.GroupBox.Controls.Add(this.DownloadNONGPSButton);
            this.GroupBox.Controls.Add(this.DownloadGPSButton);
            this.GroupBox.Controls.Add(this.FlashfromFileUserDBButton);
            this.GroupBox.Controls.Add(this.FlashUserDBButton);
            this.GroupBox.Controls.Add(this.DownloadUserDBButton);
            this.GroupBox.Controls.Add(this.nsSeperator3);
            this.GroupBox.Controls.Add(this.nsLabel3);
            this.GroupBox.Controls.Add(this.nsLabel2);
            this.GroupBox.Controls.Add(this.nsLabel1);
            this.GroupBox.Controls.Add(this.nsSeperator1);
            this.GroupBox.DrawSeperator = false;
            this.GroupBox.Location = new System.Drawing.Point(12, 66);
            this.GroupBox.Name = "GroupBox";
            this.GroupBox.Size = new System.Drawing.Size(360, 152);
            this.GroupBox.SubTitle = "";
            this.GroupBox.TabIndex = 6;
            this.GroupBox.Text = "GroupBox";
            this.GroupBox.Title = "";
            // 
            // WriteButton
            // 
            this.WriteButton.Location = new System.Drawing.Point(94, 7);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(42, 23);
            this.WriteButton.TabIndex = 14;
            this.WriteButton.Text = "Write";
            this.WriteButton.Click += new System.EventHandler(this.WriteButton_Click);
            // 
            // ReadButton
            // 
            this.ReadButton.Location = new System.Drawing.Point(142, 7);
            this.ReadButton.Name = "ReadButton";
            this.ReadButton.Size = new System.Drawing.Size(42, 23);
            this.ReadButton.TabIndex = 13;
            this.ReadButton.Text = "Read";
            this.ReadButton.Click += new System.EventHandler(this.ReadButton_Click);
            // 
            // FlashFirmwareFileButton
            // 
            this.FlashFirmwareFileButton.Location = new System.Drawing.Point(142, 77);
            this.FlashFirmwareFileButton.Name = "FlashFirmwareFileButton";
            this.FlashFirmwareFileButton.Size = new System.Drawing.Size(94, 23);
            this.FlashFirmwareFileButton.TabIndex = 12;
            this.FlashFirmwareFileButton.Text = "Flash from file";
            this.FlashFirmwareFileButton.Click += new System.EventHandler(this.FlashFirmwareFileButton_Click);
            // 
            // FlashFirmwareButton
            // 
            this.FlashFirmwareButton.Location = new System.Drawing.Point(94, 77);
            this.FlashFirmwareButton.Name = "FlashFirmwareButton";
            this.FlashFirmwareButton.Size = new System.Drawing.Size(42, 23);
            this.FlashFirmwareButton.TabIndex = 11;
            this.FlashFirmwareButton.Text = "Flash";
            this.FlashFirmwareButton.Click += new System.EventHandler(this.FlashFirmwareButton_Click);
            // 
            // DownloadNONGPSButton
            // 
            this.DownloadNONGPSButton.Location = new System.Drawing.Point(203, 48);
            this.DownloadNONGPSButton.Name = "DownloadNONGPSButton";
            this.DownloadNONGPSButton.Size = new System.Drawing.Size(132, 23);
            this.DownloadNONGPSButton.TabIndex = 10;
            this.DownloadNONGPSButton.Text = "Download NON-GPS";
            this.DownloadNONGPSButton.Click += new System.EventHandler(this.DownloadNONGPSButton_Click);
            // 
            // DownloadGPSButton
            // 
            this.DownloadGPSButton.Location = new System.Drawing.Point(94, 48);
            this.DownloadGPSButton.Name = "DownloadGPSButton";
            this.DownloadGPSButton.Size = new System.Drawing.Size(103, 23);
            this.DownloadGPSButton.TabIndex = 9;
            this.DownloadGPSButton.Text = "Download GPS";
            this.DownloadGPSButton.Click += new System.EventHandler(this.DownloadGPSButton_Click);
            // 
            // FlashfromFileUserDBButton
            // 
            this.FlashfromFileUserDBButton.Location = new System.Drawing.Point(238, 122);
            this.FlashfromFileUserDBButton.Name = "FlashfromFileUserDBButton";
            this.FlashfromFileUserDBButton.Size = new System.Drawing.Size(94, 23);
            this.FlashfromFileUserDBButton.TabIndex = 8;
            this.FlashfromFileUserDBButton.Text = "Flash from file";
            this.FlashfromFileUserDBButton.Click += new System.EventHandler(this.FlashfromFileUserDBButton_Click);
            // 
            // FlashUserDBButton
            // 
            this.FlashUserDBButton.Location = new System.Drawing.Point(190, 122);
            this.FlashUserDBButton.Name = "FlashUserDBButton";
            this.FlashUserDBButton.Size = new System.Drawing.Size(42, 23);
            this.FlashUserDBButton.TabIndex = 7;
            this.FlashUserDBButton.Text = "Flash";
            this.FlashUserDBButton.Click += new System.EventHandler(this.FlashUserDBButton_Click);
            // 
            // DownloadUserDBButton
            // 
            this.DownloadUserDBButton.Location = new System.Drawing.Point(71, 122);
            this.DownloadUserDBButton.Name = "DownloadUserDBButton";
            this.DownloadUserDBButton.Size = new System.Drawing.Size(113, 23);
            this.DownloadUserDBButton.TabIndex = 6;
            this.DownloadUserDBButton.Text = "Download Update";
            this.DownloadUserDBButton.Click += new System.EventHandler(this.DownloadUserDBButton_Click);
            // 
            // nsSeperator3
            // 
            this.nsSeperator3.Location = new System.Drawing.Point(3, 106);
            this.nsSeperator3.Name = "nsSeperator3";
            this.nsSeperator3.Size = new System.Drawing.Size(354, 10);
            this.nsSeperator3.TabIndex = 5;
            this.nsSeperator3.Text = "nsSeperator3";
            // 
            // nsLabel3
            // 
            this.nsLabel3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.nsLabel3.Location = new System.Drawing.Point(3, 122);
            this.nsLabel3.Name = "nsLabel3";
            this.nsLabel3.Size = new System.Drawing.Size(72, 23);
            this.nsLabel3.TabIndex = 4;
            this.nsLabel3.Text = "nsLabel3";
            this.nsLabel3.Value1 = "USERDB";
            this.nsLabel3.Value2 = "";
            // 
            // nsLabel2
            // 
            this.nsLabel2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.nsLabel2.Location = new System.Drawing.Point(3, 48);
            this.nsLabel2.Name = "nsLabel2";
            this.nsLabel2.Size = new System.Drawing.Size(85, 23);
            this.nsLabel2.TabIndex = 3;
            this.nsLabel2.Text = "nsLabel2";
            this.nsLabel2.Value1 = "FIRMWARE";
            this.nsLabel2.Value2 = "";
            // 
            // nsLabel1
            // 
            this.nsLabel1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.nsLabel1.Location = new System.Drawing.Point(3, 3);
            this.nsLabel1.Name = "nsLabel1";
            this.nsLabel1.Size = new System.Drawing.Size(85, 23);
            this.nsLabel1.TabIndex = 2;
            this.nsLabel1.Text = "nsLabel1";
            this.nsLabel1.Value1 = "CODEPLUG";
            this.nsLabel1.Value2 = "";
            // 
            // nsSeperator1
            // 
            this.nsSeperator1.Location = new System.Drawing.Point(0, 32);
            this.nsSeperator1.Name = "nsSeperator1";
            this.nsSeperator1.Size = new System.Drawing.Size(354, 10);
            this.nsSeperator1.TabIndex = 0;
            this.nsSeperator1.Text = "nsSeperator1";
            // 
            // OpenButton
            // 
            this.OpenButton.Location = new System.Drawing.Point(325, 37);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(44, 23);
            this.OpenButton.TabIndex = 4;
            this.OpenButton.Text = "Open";
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // FilenameLabel
            // 
            this.FilenameLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.FilenameLabel.Location = new System.Drawing.Point(12, 37);
            this.FilenameLabel.Name = "FilenameLabel";
            this.FilenameLabel.Size = new System.Drawing.Size(75, 23);
            this.FilenameLabel.TabIndex = 3;
            this.FilenameLabel.Value1 = "FILENAME";
            this.FilenameLabel.Value2 = "";
            // 
            // FilenameTextBox
            // 
            this.FilenameTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.FilenameTextBox.Location = new System.Drawing.Point(93, 37);
            this.FilenameTextBox.MaxLength = 32767;
            this.FilenameTextBox.Multiline = false;
            this.FilenameTextBox.Name = "FilenameTextBox";
            this.FilenameTextBox.ReadOnly = true;
            this.FilenameTextBox.Size = new System.Drawing.Size(226, 23);
            this.FilenameTextBox.TabIndex = 2;
            this.FilenameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FilenameTextBox.UseSystemPasswordChar = false;
            // 
            // ExitButton
            // 
            this.ExitButton.Font = new System.Drawing.Font("Verdana", 12F);
            this.ExitButton.Location = new System.Drawing.Point(354, 3);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(27, 20);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.Text = "X";
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // TyteraFlashToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 260);
            this.Controls.Add(this.nsTheme);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TyteraFlashToolForm";
            this.Text = "Tytera Flash Tool";
            this.nsTheme.ResumeLayout(false);
            this.nsTheme.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.GroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private NSTheme nsTheme;
        private NSButton ExitButton;
        private NSTextBox FilenameTextBox;
        private NSLabel FilenameLabel;
        private NSButton OpenButton;
        private NSGroupBox GroupBox;
        private NSSeperator nsSeperator1;
        private NSLabel nsLabel1;
        private NSLabel nsLabel2;
        private NSLabel nsLabel3;
        private NSSeperator nsSeperator3;
        private NSButton DownloadUserDBButton;
        private NSButton FlashUserDBButton;
        private NSButton FlashfromFileUserDBButton;
        private System.Windows.Forms.Label StatusLabel;
        private NSButton DownloadGPSButton;
        private NSButton DownloadNONGPSButton;
        private NSButton FlashFirmwareFileButton;
        private NSButton FlashFirmwareButton;
        private NSButton ReadButton;
        private NSButton WriteButton;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.Timer Timer;
        private NSProgressBar nsProgressBar;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

