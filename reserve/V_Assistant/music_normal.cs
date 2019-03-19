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
    public partial class music_normal : Form
    {
        List<string> songs;
        int current = 0;
        private const int CP_NOCLOSE_BUTTON = 0x200;
        Control comm;
        Data data;

        List<string>  SongPaths;

       

        internal music_normal(Control main, Data datas)
        {
            InitializeComponent();

            SongPaths = datas.storage.songs;
            
            foreach (string song in datas.storage.songs.ToList())
            {
                listBox1.Items.Add(Path.GetFileName(song));
                SongPaths.Add(song);
            }
            this.ShowInTaskbar = false;
            openFileDialog1.Multiselect = true;
            comm = main;
            data = datas;
          
        }
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MINIMIZE = 0xf020;


        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.WindowState.Equals(FormWindowState.Minimized))
            {
                this.WindowState = FormWindowState.Normal;
                this.Hide();
           
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }
        private void music_normal_Load(object sender, EventArgs e)
        {
            
            axWindowsMediaPlayer1.settings.volume = trackBar1.Value*15;
        }

        private void Add_Click(object sender, EventArgs e)
        {   
            openFileDialog1.ShowDialog();
            
            

            string[] files, paths;
            paths = openFileDialog1.FileNames;
            files = openFileDialog1.SafeFileNames;
            foreach (string file in files)
            {
                listBox1.Items.Add(file);
            }

            axWindowsMediaPlayer1.URL = paths[0];
            axWindowsMediaPlayer1.Ctlcontrols.play();
            update_label();
            foreach (string path in paths)

            {
                SongPaths.Add(path);
                
            }

            data.storage.songs = SongPaths;
            data.save_current_settings();
           




        }

        private void button1_Click(object sender, EventArgs e)
        {
            current--;
            if (current < 0) current = 0;
            axWindowsMediaPlayer1.URL=SongPaths[current];
            axWindowsMediaPlayer1.Ctlcontrols.play();
            update_label();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            current++;
            if (current > SongPaths.Count) current = 0;
            axWindowsMediaPlayer1.URL = SongPaths[current];
            axWindowsMediaPlayer1.Ctlcontrols.play();
            update_label();



        }

        private void Stop_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();

        } 

        private void Play_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
           
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = SongPaths[listBox1.SelectedIndex];
            current = listBox1.SelectedIndex;
            axWindowsMediaPlayer1.Ctlcontrols.play();
            update_label();
        }
        void update_label()
        {
            label1.Text =Path.GetFileName(SongPaths[current]);
            int cut=label1.Text.Length;
            if (label1.Text.Length > 35) cut = 35;

            label1.Text = label1.Text.Substring(0,cut);
            Console.Beep();
        
        }
        private void button3_Click(object sender, EventArgs e)
        {    
            SongPaths.Clear();
            data.storage.songs.Clear();
            listBox1.Items.Clear();
            data.save_current_settings();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            
            current = random.Next(0, SongPaths.Count);
            axWindowsMediaPlayer1.URL = SongPaths[current];
            axWindowsMediaPlayer1.Ctlcontrols.play();
            update_label(); 
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
          
            axWindowsMediaPlayer1.settings.volume = trackBar1.Value*15;
        }
    }
}
