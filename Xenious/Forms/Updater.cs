using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xenious.Forms
{
    public partial class Updater : Form
    {

        public bool updated = false;
        public Updater()
        {
            InitializeComponent();
        }

        private void Updater_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = Startup.Update.get_update_info();
        }

        public void update_status(string msg)
        {
            textBox1.Text = msg;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            label2.Enabled = false;

            update_status("Downloading update....");
            System.Threading.Thread.Sleep(1000);
            Startup.Update.download_update();
            update_status("Update downloaded....");
            System.Threading.Thread.Sleep(1000);
            update_status("Deleting old data....");
            System.Threading.Thread.Sleep(1000);
            if(Startup.Update.delete_old() == false) {
                this.updated = false;
                this.Close();
            }
            update_status("Extracting update....");
            System.Threading.Thread.Sleep(1000);
            Startup.Update.extract_update();
            System.Threading.Thread.Sleep(1000);
            update_status("Update complete !");
            this.updated = true;
            this.Close();
        }
    }
}
