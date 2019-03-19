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
    enum KeyModifier
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        WinKey = 8
    }
    public partial class Vass : Form
    {     
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        int mActionHotKeyID = 47;

        



        public  Data data = new Data();
        Graphic UI;
        public music_normal music = null;
        public Draw_on_screen draw;
        public TimeWidget timeWidget;
        public Settings settings;
        public List<Action> update_queue = new List<Action>();
        bool listen = true;
        public Vass()
        {
            InitializeComponent();
            
            UI = new Graphic(this,data);
            music = new music_normal(this, data);
            draw = new Draw_on_screen(this);
            bindHotkeys();
            //engine = new SpeechRecognitionEngine();
            //engine.SetInputToDefaultAudioDevice();
            //Choices choices = new Choices();

            //choices.Add(data.storage.phrases.ToArray());

            //GrammarBuilder builder = new GrammarBuilder(choices);
            //Grammar grammar = new Grammar(builder);
            //engine.LoadGrammarAsync(grammar);
            //engine.RecognizeAsync(RecognizeMode.Multiple);
           // engine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(reacting);



        }
        #region hotkeys
        void bindHotkeys()          
        {

            KeysConverter keysConverter = new KeysConverter();

        
            //} //40 music 45 draw 50 clear all 55 clear region 
            RegisterHotKey(this.Handle, 40, (int)KeyModifier.Alt, Keys.X.GetHashCode());
            RegisterHotKey(this.Handle, 45, (int)KeyModifier.Alt, Keys.Q.GetHashCode());
            RegisterHotKey(this.Handle, 50, (int)KeyModifier.Alt, Keys.E.GetHashCode());
            RegisterHotKey(this.Handle, 55, (int)KeyModifier.Alt, Keys.A.GetHashCode());
            RegisterHotKey(this.Handle, 60, (int)KeyModifier.Alt, Keys.D.GetHashCode());

        }
      
        protected override void WndProc(ref Message m)
        {
            
            if (m.Msg == 0x0312)
            {
               
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);                  
                KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);       
                int id = m.WParam.ToInt32();


                if (id==40) Open_Music();
                if (id==45) ScreenDraw(); 
                if (id==50) draw.isDrawing = !draw.isDrawing;
                if (id == 55)
                {
                    draw.isDrawing = false;
                    this.Invalidate(new Rectangle(draw.mousepos.X - 50, draw.mousepos.Y - 50, 100, 100), true);
                }
                if (id==60)
                {
                    Console.Beep();
                    draw.isDrawing = false;
                    this.Invalidate();
                }
               
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
                if (item.Name == "Apps") item.Click += new EventHandler(OpenGames);
                if (item.Name == "Settings") item.Click += new EventHandler(OpenSettings);
                if (item.Name == "Time") item.Click += new EventHandler(Time);
                if (item.Name == "Draw") item.Click += new EventHandler(ScreenDraw);


            }
            timeWidget = new TimeWidget(this.data);




        }
        void OpenSettings(object sender, EventArgs e)
        {
            try
            {
                settings.Show();
            }
            catch (System.ObjectDisposedException)
            {
                settings = new Settings(data);
                settings.Show();
            }
            catch(System.NullReferenceException)
            {
                settings = new Settings(data);
                settings.Show();
            }
        }
        void Time(object sender=null,EventArgs e=null)
        {   timeWidget.Active = !timeWidget.Active;
            if (timeWidget.Active)
            {
                Console.Beep();

                this.Controls.Add(timeWidget);
                this.update_queue.Add( timeWidget.tick);

            }
            else
            {
                this.Controls.Remove(timeWidget);
                this.update_queue.Remove(timeWidget.Update);
            }
        }
        void OpenGames(object sender,EventArgs e)
        {   for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i].GetType() == typeof( IconWIdget))
                {
                    Controls.RemoveAt(i);
                }
            }
            this.UI.AddAppsIcons();
        }
        void close_app(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        


        }
        void Open_Music(object sender=null, EventArgs e=null)
        {
            music.Active = !music.Active;
            if (music.Active)
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
            else
            {
                try
                {
                    music.Hide();

                }
                catch (System.ObjectDisposedException)
                {
                    music = new music_normal(this, data);
                    music.Hide();
                }

            }

        }
        void ScreenDraw(object sender = null, EventArgs e = null)
        {
            draw.Active = !draw.Active;
            if (draw.Active)
            {
                try
                {
                    draw.Show();

                }
                catch (System.ObjectDisposedException)
                {
                    draw = new Draw_on_screen(this);
                    draw.isDrawing = false;
                    draw.Show();
                }
            }
            else
            {
                try
                {
                    draw.Hide();
                    draw.isDrawing = false;

                }
                catch (System.ObjectDisposedException)
                {
                    draw = new Draw_on_screen(this);
                    draw.Hide();
                    draw.isDrawing = false;
                }

            }

        }

        //void reacting(object sender,SpeechRecognizedEventArgs e)
        //{   if (!listen) return;
        //    Console.Beep();
        //    string text = e.Result.Text;

        //    if (text == "stop")
        //    {
        //        if (music.Active)
        //            music.axWindowsMediaPlayer1.Ctlcontrols.stop();
        //    }
        //    else if (text == "play")
        //    {
        //        if (music.Active)
        //            music.axWindowsMediaPlayer1.Ctlcontrols.play();
        //    }
        //    else if (text == "music")
        //    {
                
        //        Open_Music();
        //    }
        //    else if (text == "next")
        //    {

        //        music.button2_Click();
        //    }
        //    else if (text == "prev")
        //    {

        //        music.button1_Click();
        //    }
        //    else if (text == "shuffle")
        //    {   if (music.Active)
        //        music.shuffle_click();
        //    }
        //    else if (text == "time")
        //    {
        //        Time();

        //    }
        //}
        private void timer1_Tick(object sender, EventArgs e)
        {

            foreach (Action func in update_queue)
            {
                
                func();
            }

        }


    }
}
