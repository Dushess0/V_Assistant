using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace V_Assistant
{
    public partial class Settings : Form
    {
        Data data;
        public Settings(Data t)
        {
            InitializeComponent();
            data = t;
            openFileDialog1.Filter= "EXE files (*.exe)|*.exe|Links (*.lnk)|*.lnk";
            update_list();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            update_list();
            data.storage.Apps.Clear();
            data.save_current_settings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"settings.json"))
            {
                File.Delete(@"settings.json");
                Console.Beep();
                System.Windows.Forms.Application.Exit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {   
            openFileDialog1.ShowDialog();

            data.storage.Apps.Add(openFileDialog1.FileName);
            int cap = data.storage.Apps.Count;
            data.storage.IconsPos.Add(new Point(64 * cap, 200));
            data.save_current_settings();
            update_list();

        }
        void update_list()
        {
            listBox1.Items.Clear();
            foreach (string s in data.storage.Apps)
            {
                listBox1.Items.Add(Path.GetFileName(s));


            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            data.storage.Apps.RemoveAt(listBox1.SelectedIndex);
            data.storage.IconsPos.RemoveAt(listBox1.SelectedIndex);
            data.save_current_settings();
            update_list();
        }
    }
}
