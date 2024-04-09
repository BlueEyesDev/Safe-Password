using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.Threading;
using safe_password.Forms;
namespace safe_password
{
    public partial class MainForm : MetroSetForm
    {
        public string safe_password { get; set; }
        public List<JsonItem> json_items { get; set; } = new List<JsonItem>();
        public MainForm(string safe_password)
        {
            InitializeComponent();
            this.safe_password = BitConverter.ToString(SHA512.Create().ComputeHash(UTF8Encoding.UTF8.GetBytes(safe_password)));
            if (!File.Exists("SafeKey"))
            {
                File.WriteAllText("SafeKey", TDESEncryption("[]"));
            } else {
                string Data = TDESDecrypter(File.ReadAllText("SafeKey"));
                if (Data == null) {
                    new ErrorPassword().Show();
                    return;
                }
                json_items = new JavaScriptSerializer().Deserialize<List<JsonItem>>(Data);
                foreach (JsonItem item in json_items) {
                    ListViewItem x = new ListViewItem(string.Empty);
                    x.SubItems.Add(item.Name);
                    x.SubItems.Add(item.Url);
                    x.SubItems.Add(item.User);
                    x.SubItems.Add("**********");
                    listView1.Items.Add(x);
                }
            }

        }
        private string TDESEncryption(string Input)
        {
            using (TripleDES tripleDES = TripleDES.Create())
            {
                tripleDES.Key = MD5.Create().ComputeHash(UTF8Encoding.UTF8.GetBytes(this.safe_password));
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform encryptor = tripleDES.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(Input);
                    return Convert.ToBase64String(encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length));
                }
            }
        }
        private string TDESDecrypter(string Input)
        {
            try
            {


                using (TripleDES tripleDES = TripleDES.Create())
                {
                    tripleDES.Key = MD5.Create().ComputeHash(UTF8Encoding.UTF8.GetBytes(this.safe_password));
                    tripleDES.Mode = CipherMode.ECB;
                    tripleDES.Padding = PaddingMode.PKCS7;
                    byte[] encryptedBytes = Convert.FromBase64String(Input);
                    using (ICryptoTransform decryptor = tripleDES.CreateDecryptor())
                    {
                        byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                        return Encoding.UTF8.GetString(decryptedBytes);
                    }
                }
            }
            catch (Exception) { }
            return null;
        }
        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            AddAccount maFenetre = new AddAccount();
            if (maFenetre.ShowDialog() == DialogResult.OK)
            {
                JsonItem json = maFenetre.json;
                json_items.Add(json);
                ListViewItem x = new ListViewItem(string.Empty);
                x.ForeColor = Color.White;
                x.SubItems.Add(json.Name);
                x.SubItems.Add(json.Url);
                x.SubItems.Add(json.User);
                x.SubItems.Add("**********");
                listView1.Items.Add(x);
            }
        }
        private void metroSetButton2_Click(object sender, EventArgs e)
        {
            if (new Save().ShowDialog() == DialogResult.OK) {
                File.WriteAllText("SafeKey", TDESEncryption(new JavaScriptSerializer().Serialize(json_items)));
            }

        }
        private void testeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0) {
                ListViewItem selectedRow = listView1.SelectedItems[0];
                Process.Start(selectedRow.SubItems[2].Text);
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedRow = listView1.SelectedItems[0];
                Clipboard.SetText(selectedRow.SubItems[3].Text);
            }
            
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedRow = listView1.SelectedItems[0];
                Clipboard.SetText(json_items[selectedRow.Index].Password);
            }
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ShowPassword sp = new ShowPassword();
                DialogResult teste = sp.ShowDialog();
                if (teste == DialogResult.OK && sp.Password == safe_password)
                {
                    ListViewItem selectedRow = listView1.SelectedItems[0];
                    selectedRow.SubItems[4].Text = json_items[selectedRow.Index].Password;
                    new Thread((id) =>
                    {
                        for (int i = 0; i < 60; i++)
                        {
                            Thread.Sleep(1000);
                        }
                        Invoke(new Action(() =>
                        {
                            listView1.Items[(int)id].SubItems[4].Text = "**********";
                        }));
                    })
                    { IsBackground = true }.Start(selectedRow.Index);
                }
                else {
                    new ErrorPassword().Show();
                }

            }
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedRow = listView1.SelectedItems[0];
                json_items.RemoveAt(selectedRow.Index);
                listView1.Items.RemoveAt(selectedRow.Index);
            }
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
    public class JsonItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
