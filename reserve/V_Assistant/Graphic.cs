using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace V_Assistant
{
    internal class Graphic
    {
        Control comm = null;

        JsonData storage = null;

        List<string> active_widgets = new List<string>();

        internal Graphic(Vass main,JsonData suka)
        {
            comm = main;
            storage = suka;
            set_window_size();
            



            main.notifyIcon1.Icon = main.Icon;



            main.notifyIcon1.BalloonTipTitle = "Vass";
          //  main.notifyIcon1.BalloonTipText = "Activated!";
           // main.notifyIcon1.ShowBalloonTip(0);
            main.notifyIcon1.Visible = true;
            main.ShowInTaskbar = false;

            foreach (string s in storage.MenuList)
            {   
                ToolStripMenuItem mi = new ToolStripMenuItem(s, null);// (s, a) => actionOnClicItem1());
                mi.ForeColor = Color.White;
                mi.Name = s;
                main.Menu.Items.Add(mi);

                // Separator
               // main.Menu.Items.Add(new ToolStripSeparator());

            }

            ToolStripMenuItem settings = new ToolStripMenuItem("Settings", null);// (s, a) => actionOnClicItem1());
            settings.ForeColor = Color.White;
            settings.Name = "Settings";
            main.Menu.Items.Add(settings);
            ToolStripMenuItem exit = new ToolStripMenuItem("Exit", null);// (s, a) => actionOnClicItem1());
            exit.Name = "Exit";
            exit.ForeColor = Color.White;
            main.Menu.Items.Add(exit);


        }
        
        void set_window_size()
        {
            var screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            comm.Width = screen.Width;
            comm.Height = screen.Height;

            
        }
       
      
    }
    public class Menu:DragLabel
    {
        public Menu()
        {
            int[]Bounds = new int[4];

        }
    }
    public class WidgetIcon:PictureBox
    {
        Graphic graphic = null;
        String type = "";
        public WidgetIcon(string input)
        {
            
            type = input;
            this.Click += new EventHandler(action);
            load_image();

        }
        protected void action(object sender, EventArgs e)
        {
            Console.Beep();
        }
        void load_image()
        {
            if (type == "time") this.LoadAsync("cog.png");
            Console.Beep();
        }
       

    }
    public  class TimeWidget : DragLabel
    {
        public TimeWidget(Point point)
        {
            this.Top = point.X;
            this.Left = point.Y;
            
            

        }
        public void udpate()
        {

        }
      

    }
}
