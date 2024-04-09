using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace safe_password.Forms
{
    public partial class ShowPassword : MetroSetForm
    {
        public string Password { get; private set; }
        public ShowPassword()
        {
            InitializeComponent();
        }

        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            Password = BitConverter.ToString(SHA512.Create().ComputeHash(UTF8Encoding.UTF8.GetBytes(metroSetTextBox1.Text)));
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ShowPassword_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
