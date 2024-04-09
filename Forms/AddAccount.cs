using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace safe_password
{
    public partial class AddAccount : MetroSetForm
    {
        public JsonItem json { get; private set; }
        public AddAccount()
        {
            InitializeComponent();
        }

        private void metroSetButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            json = new JsonItem() { Name = metroSetTextBox1.Text, Url = metroSetTextBox2.Text, User = metroSetTextBox3.Text,  Password = metroSetTextBox4.Text };
            this.Close();
        }
    }
}
