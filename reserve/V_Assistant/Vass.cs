using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace V_Assistant
{
   
    public partial class Vass : Form
    {     
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        int mActionHotKeyID = 40;

      



        public  Data data = new Data();
        Graphic UI;
        public music_normal music = null;
        public List<Action> update_queue = new List<Action>();
        
        public Vass()
        {
            InitializeComponent();
            
            UI = new Graphic(this,data.storage);
            music = new music_normal(this, data);
            bindHotkeys();
          


        }
        #region HotKeys
        string get_between(string full,string first,string second)
        {
           

            int pFrom = full.IndexOf(first) + first.Length;
            int pTo = full.LastIndexOf(second);

            return full.Substring(pFrom, pTo - pFrom);
        }
        void bindHotkeys()          
        {

            KeysConverter keysConverter = new KeysConverter();
            
            foreach (string s in data.storage.HotKeys)
            {
                string key = get_between(s, ",", "#").ToUpper();
                
                
                Keys keycode = (Keys)keysConverter.ConvertFromString(key);

                RegisterHotKey(this.Handle, mActionHotKeyID, calc_modifiers(s), (int)keycode);



            }
            
         
        }
        string hex_to_string(string hexString)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hexString.Length; i += 2)
            {
                string hs = hexString.Substring(i, 2);
                sb.Append(Convert.ToChar(Convert.ToUInt32(hs, 16)));
            }
            String ascii = sb.ToString();
            return ascii;
        

        }
        string int_to_mod_string(int input)
        {
            
            string myHex = input.ToString("x");
            string key = myHex.Substring(0, 2);

            key = hex_to_string(key);
            string modifiers = myHex.Substring(4, 2);
            int mod = Int32.Parse(modifiers);
            string full = "";
            if (mod>=8) { mod -= 8; full += "win+"; }
            if (mod >= 4) { mod -= 4; full += "shift+"; }
            if (mod >= 2) { mod -= 2; full += "ctrl+"; }
            if (mod >= 1) { mod -= 1; full += "alt+"; }
           
            return full+","+key.ToUpper();
        }
        int calc_modifiers(string input)
        {
            int mod = 0;
            if (input.Split(',').Length > 1)
            {



                int pTo = input.LastIndexOf(',');
                string s_mods = input.Substring(0, pTo);

                foreach (string sub in s_mods.Split('+'))
                {
                    if (sub.ToLower() == "shift") mod += 4;
                    if (sub.ToLower() == "ctrl") mod += 2;
                    if (sub.ToLower() == "alt") mod++;
                    if (sub.ToLower() == "win") mod += 8;

                }
            }
            return mod;
        }
        void HotKeysActions(string pressed)
        {
            notifyIcon1.BalloonTipText = pressed;
            notifyIcon1.ShowBalloonTip(1);
            foreach (string s in data.storage.HotKeys)
            {
                
                if (s.StartsWith(pressed))
                {
                    Console.Beep();
                }
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == mActionHotKeyID)
            {



              
               
              
               
                 HotKeysActions(int_to_mod_string(m.LParam.ToInt32()));
            }
            base.WndProc(ref m);
        }
        #endregion
        private void Vass_Load(object sender, EventArgs e)
        {

         //назначение функций для меню
         foreach (ToolStripMenuItem item in Menu.Items)
            {   if (item.Name == "Exit") item.Click += new EventHandler(close_app);
                if (item.Name == "Music") item.Click += new EventHandler(Open_Music);
                if (item.Name == "Exit") item.Click += new EventHandler(close_app);
                if (item.Name == "Exit") item.Click += new EventHandler(close_app);
                if (item.Name == "Exit") item.Click += new EventHandler(close_app);
                if (item.Name == "Exit") item.Click += new EventHandler(close_app);


            }




        }
        void close_app(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();

        }
        void Open_Music(object sender, EventArgs e)
        {
            try
            {
                music.Show();
            }
            catch (System.ObjectDisposedException)
            {
                music = new music_normal(this, data);
                music.Show();
            }


        }


        void start_time(TimeWidget a)
        {
            update_queue.Add(update);
            void update()
            {
                a.Text = DateTime.Now.ToString("hh:mm:ss");
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            foreach (Action func in update_queue)
                func();

        }


    }
}
