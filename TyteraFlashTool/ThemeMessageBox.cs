using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TyteraFlashTool
{
    public partial class ThemeMessageBox : Form
    {
        public static void ShowMessageBox(string message)
        {
            ThemeMessageBox tmb = new ThemeMessageBox(message);
            tmb.ShowDialog();
        }

        public ThemeMessageBox(string message)
        {
            InitializeComponent();
            MessageLabel.Text = message;
        }

        private void OkayButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
