using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace V_Assistant
{
    public class DragLabel : Label
    {
        Control comm = new Control();
        private Point mouseOffset;
        private bool isMouseDown = false;
        
        public DragLabel()
        {


           
            this.BackColor = Color.DimGray;
            this.ForeColor = Color.Coral;
            this.Font = new Font("Arial", 24);
            
            this.AutoSize = true;

            this.MouseDown += new MouseEventHandler(click_on_label);
            this.MouseMove += new MouseEventHandler(Move);
            this.MouseUp += new MouseEventHandler(Release);



        }
        private void Release(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                isMouseDown = false;
            }
        }
        protected void click_on_label(object sender, MouseEventArgs e)
        {


            int offsetx;
            int offsety;
            if (e.Button == MouseButtons.Right)
            {
                offsetx = -e.X - SystemInformation.FrameBorderSize.Width;
                offsety = -e.Y - SystemInformation.CaptionHeight - SystemInformation.FrameBorderSize.Height;
                mouseOffset = new Point(offsetx, offsety);
                isMouseDown = true;

            }


        }
        private void Move(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Label label = sender as Label;
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseOffset.X, mouseOffset.Y);
                label.Left = mousePos.X;
                label.Top = mousePos.Y;
            }
        }
    }
}
