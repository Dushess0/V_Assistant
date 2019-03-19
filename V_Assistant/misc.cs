using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace V_Assistant
{
    public class Menu : DragLabel
    {
        public Menu()
        {
            int[] Bounds = new int[4];

        }
    }
    public class WidgetIcon : PictureBox
    {
        Graphic graphic = null;
        String type = "";
        public WidgetIcon(string input)
        {

            type = input;
            this.Click += new EventHandler(Action);
            Load_image();

        }
        protected void Action(object sender, EventArgs e)
        {
            Console.Beep();
        }
        void Load_image()
        {
            if (type == "time") this.LoadAsync("cog.png");
            Console.Beep();
        }


    }
    public class TimeWidget : DragLabel
    {
        Data data;
        TextBox input = new TextBox();
        public bool Active = false;
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        Timer timer = new Timer();
        int buff;
        Action action;

        public TimeWidget(Data s)
        {
            data = s;
            this.Location = data.storage.TimePos;
            this.Move += new EventHandler(save_new_pos);
            this.MouseClick += new MouseEventHandler(check_for_activate);
            this.DoubleClick += new EventHandler(to_clock);
            this.Text = "Hello";
            timer.Tick += new EventHandler(End_of_timer);
            input.KeyDown += Input_KeyDown; ;
            action = clock_action;
            stopwatch.Start();




        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {


                    int interval = (int)(Double.Parse(input.Text) * 60 * 1000);
                    stopwatch.Reset();
                    timer.Start();
                    buff = interval / 1000;

                    this.timer.Interval = interval;
                    action = timer_action;
                    stopwatch.Start();
                    this.Controls.Remove(input);
                }
                catch { return; }

            }



        }

        void End_of_timer(object sender, EventArgs e)
        {
            action = clock_action;
            Console.Beep();
            Console.Beep(50, 3000);
            Console.Beep();
            timer.Stop();



        }
        void stopwatch_action()
        {

            this.Text = stopwatch.Elapsed.ToString("hh\\:mm\\:ss");

        }
        void to_clock(object sender, EventArgs e)
        {
            action = clock_action;



        }

        void timer_action()
        {
            int delta = buff - stopwatch.Elapsed.Hours * 3600 - stopwatch.Elapsed.Minutes * 60 - stopwatch.Elapsed.Seconds;
            TimeSpan span = TimeSpan.FromSeconds(delta);


            this.Text = span.ToString("hh\\:mm\\:ss");


        }
        void get_timer_input()
        {
            input.Width = this.Width;
            input.Height = this.Height;

            input.Text = "Enter value in minutes ";


            this.Controls.Add(input);
        }









        void clock_action()
        {
            this.Text = DateTime.Now.ToString("hh:mm:ss");
        }
        void check_for_activate(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                stopwatch.Reset();

                get_timer_input();
            }
            else if (e.Button == MouseButtons.Left)
            {
                action = stopwatch_action;
                 
                stopwatch.Reset();
                stopwatch.Start();
            }
        }
        public void save_new_pos(object sender, EventArgs e)
        {
            data.storage.TimePos = this.Location;
            data.save_current_settings();
            
        }

        public void tick()
        {
            action();

        }


    }
}
