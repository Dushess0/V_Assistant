using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace V_Assistant
{
    public class IconWIdget:PictureBox
    {
       
        private Point mouseOffset;
        private bool isMouseDown = false;
        string name;
        Data data;
        public IconWIdget(string url,Data t)
        {
            Icon ico = Icon.ExtractAssociatedIcon(url);
            name = url;
            data = t;
            this.Image=ico.ToBitmap();
            this.Location = data.storage.IconsPos[data.storage.Apps.IndexOf(url)];
            this.MouseDown += new MouseEventHandler(click_on_label);
            this.MouseMove += new MouseEventHandler(Move);
            this.MouseUp += new MouseEventHandler(Release);
            this.LocationChanged+= new EventHandler(SaveLocation);
            



        }
        private void SaveLocation(object sender, EventArgs e)
        {
            data.storage.IconsPos[data.storage.Apps.IndexOf(name)] = this.Location;
            data.save_current_settings();
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
            else if(e.Button == MouseButtons.Left)
            {
                launch_app();
            }


        }
        void launch_app()
        {
            System.Diagnostics.Process.Start(name);
        }
        private void Move(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Label label = sender as Label;
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseOffset.X, mouseOffset.Y);
                this.Left = mousePos.X;
                this.Top = mousePos.Y;
            }
        }

    }
}
