using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroSet_UI;
using MetroSet_UI.Forms;
using MetroSet_UI.Interfaces;
namespace safe_password
{
    public partial class Login : MetroSetForm
    {
        public Login()
        {
            InitializeComponent();
        }

        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            new MainForm(metroSetTextBox1.Text).Show();
            metroSetTextBox1.Text = "BlueSafePassword";
            this.Hide();
        }
    }
}
