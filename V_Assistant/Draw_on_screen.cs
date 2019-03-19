using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace V_Assistant
{
    public partial class Draw_on_screen : Form
    {   public bool isDrawing=false;
        Control main = null;
        SolidBrush brush;
        Graphics graphics;
        public Boolean Active = false;
        public Point mousepos;
        public Draw_on_screen(Control control)
        {
            InitializeComponent();
            main = control;
            brush = new SolidBrush(Color.DarkRed);
            Thread thread = new Thread(loop);
            thread.SetApartmentState(ApartmentState.STA);
            CheckForIllegalCrossThreadCalls = false;
            thread.Start();
            


        }

        private void Draw_on_screen_Load(object sender, EventArgs e)
        {

        }
        void loop()
        {
            while (true)
            {


                Thread.Sleep(1);

                this.mousepos = Cursor.Position;
                if (isDrawing)
                {
                   
                    graphics = main.CreateGraphics();
                    graphics.FillEllipse(brush, Cursor.Position.X, Cursor.Position.Y, trackBar1.Value, trackBar1.Value);
                }


            }
        }
        void clear()
        {
            main.Invalidate();
        }
        private void panel1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                brush.Color = colorDialog1.Color;
        } 
    }

}
