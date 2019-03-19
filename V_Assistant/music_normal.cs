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
        
        int current = 0;
        private const int CP_NOCLOSE_BUTTON = 0x200;
        Control comm;
        Data data;
        public Boolean shuffle = false;
        public Boolean Active = false;
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
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
           
            openFileDialog1.Multiselect = true;
            comm = main;
            data = datas;
            this.Location = data.storage.MusicPos;



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

            songs_to_playlist();
            axWindowsMediaPlayer1.Ctlcontrols.play();
            update_label();
            foreach (string path in paths)

            {
                SongPaths.Add(path);
                
            }

            data.storage.songs = SongPaths;
            data.save_current_settings();
            update_label();





        }

        public void button1_Click(object sender = null, EventArgs e = null)
        {
            //current--;
            //if (current < 0) current = 0;
            //axWindowsMediaPlayer1.URL=SongPaths[current];
            //axWindowsMediaPlayer1.Ctlcontrols.play();
            if (shuffle)
            {
                for_shuffle_playlist();
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            else axWindowsMediaPlayer1.Ctlcontrols.previous();
            update_label();
        }

        public void button2_Click(object sender=null, EventArgs e = null)
        {

            //current++;
            //if (current > SongPaths.Count) current = 0;
            //axWindowsMediaPlayer1.URL = SongPaths[current];
            //axWindowsMediaPlayer1.Ctlcontrols.play();
            if (shuffle)
            {
                for_shuffle_playlist();
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            else axWindowsMediaPlayer1.Ctlcontrols.next();

            update_label();



        }

        private void Stop_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
            update_label();
        } 

        private void Play_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
            update_label();


        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


            songs_to_playlist(SongPaths[listBox1.SelectedIndex]);
            axWindowsMediaPlayer1.Ctlcontrols.play();
            update_label();
        }
        void update_label()
        {
            try
            {
                label1.Text = axWindowsMediaPlayer1.Ctlcontrols.currentItem.name;
            }
            catch (System.NullReferenceException)
            {
                return;
            }
            int cut=label1.Text.Length;
            if (label1.Text.Length > 32) cut = 32;

            label1.Text = label1.Text.Substring(0,cut);
            
        
        }

        void songs_to_playlist(string first="")
        {
            WMPLib.IWMPPlaylist playlist;
            try { playlist = axWindowsMediaPlayer1.playlistCollection.getByName("Vass").Item(0); }
            catch
            {
                Console.Beep();
                playlist = axWindowsMediaPlayer1.playlistCollection.newPlaylist("Vass");
            }
            playlist.clear();
           
            List<string> playnow = SongPaths;
            if (first != "")
            {
                playnow = SongPaths.GetRange(SongPaths.IndexOf(first), SongPaths.Count- SongPaths.IndexOf(first));
                
                
            }
            foreach (string song in playnow)
            {
                WMPLib.IWMPMedia mediaItem = axWindowsMediaPlayer1.newMedia(song);
                playlist.appendItem(mediaItem);

            }

           
            axWindowsMediaPlayer1.currentPlaylist = playlist;

        }
        private void button3_Click(object sender, EventArgs e)
        {    
            SongPaths.Clear();
            data.storage.songs.Clear();
            listBox1.Items.Clear();
            data.save_current_settings();
        }
        void for_shuffle_playlist()
        {
            Random random=new Random();
            current = random.Next(0, SongPaths.Count);
            WMPLib.IWMPPlaylist playlist;
            try { playlist = axWindowsMediaPlayer1.playlistCollection.getByName("Vass").Item(0); }
            catch 
            {
                Console.Beep();
                playlist = axWindowsMediaPlayer1.playlistCollection.newPlaylist("Vass");
            }
            playlist.clear();
            Console.Beep();

            List<string> playnow = SongPaths;
           
                playnow = SongPaths.GetRange(current, SongPaths.Count - current);

            
            
            foreach (string song in playnow)
            {
                WMPLib.IWMPMedia mediaItem = axWindowsMediaPlayer1.newMedia(song);
                playlist.appendItem(mediaItem);

            }
            foreach (string song in SongPaths.GetRange(0,current))
            {
                WMPLib.IWMPMedia mediaItem = axWindowsMediaPlayer1.newMedia(song);
                playlist.appendItem(mediaItem);

            }


            
            axWindowsMediaPlayer1.currentPlaylist = playlist;

            
        }
      

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
          
            axWindowsMediaPlayer1.settings.volume = trackBar1.Value*10;
        }

        private void music_normal_LocationChanged(object sender, EventArgs e)
        {
            try {
                data.storage.MusicPos = this.Location;
                data.save_current_settings();
            }
            catch (System.NullReferenceException)
            {
                return;
            }
            
        }

        private void axWindowsMediaPlayer1_MediaChange(object sender, AxWMPLib._WMPOCXEvents_MediaChangeEvent e)
        {
            update_label();
        }

        public void shuffle_click(object sender=null, EventArgs e=null)
        {
            Console.Beep();
            shuffle = !shuffle;
            if (shuffle)
            {
                for_shuffle_playlist();
                axWindowsMediaPlayer1.settings.setMode("shuffle", true);

                button4.Text = "Off Shuffle";

                update_label();
            }
            else button4.Text = "On Shuffle";
            { axWindowsMediaPlayer1.settings.setMode("shuffle", false); }

        }
    }
}
