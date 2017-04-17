using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            g = Graphics.FromImage(pictureBox1.Image);
            //g = pictureBox1.CreateGraphics();
        }

        Point prPnt;
        Graphics g;
        GraphicsPath gp = new GraphicsPath();
        Point curPnt;
        bool endLine = true;
        Point X;
        Queue<Point> q = new Queue<Point>();

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            prPnt = e.Location;
            X = e.Location;

            if (Instrument_box.Text == "Brush")
                g.FillEllipse(new SolidBrush(color), prPnt.X - width / 2, prPnt.Y - width / 2, width + 2, width + 2);

            if (Instrument_box.Text == "Fill")
                Fill(e.Location);

            if (Instrument_box.Text == "Spray")                         /////////////////////////////////
            {
                int k = 0;
                while (k <= width)
                {
                    Spray(e.Location);
                    k++;
                }
                pictureBox1.Refresh();
            }

            endLine = false;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            g.DrawPath(new Pen(color, width), gp);
            endLine = true;          
        }

        

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            gp.Reset();

            if (e.Button == MouseButtons.Left && Instrument_box.Text == "Brush")
            {
                curPnt = e.Location;                
                g.FillEllipse(new SolidBrush(color) , prPnt.X - width/2, prPnt.Y - width/2, width+2, width+2);
                prPnt = curPnt;
            }

            if (e.Button == MouseButtons.Left && Instrument_box.Text == "Spray") ///////////////////////////////////
            {
                curPnt = e.Location;
                int k = 0;
                while (k <= width)
                {
                    Spray(e.Location);
                    k++;
                }
                prPnt = curPnt;
                pictureBox1.Refresh();
            }

            if (e.Button == MouseButtons.Left && Instrument_box.Text == "Pen")
            {
                curPnt = e.Location;
                g.DrawLine(new Pen(color, width), prPnt, curPnt);                
                prPnt = curPnt;
            }
            
            if (Instrument_box.Text == "Line" && endLine == false)
            {               
                curPnt = e.Location;
                gp.AddLine(prPnt, curPnt);                
            }

            if ((Instrument_box.Text == "Rectangle" || Instrument_box.Text == "Ellipse") && endLine == false)
            {
                curPnt = e.Location;

                if (curPnt.X >= prPnt.X && curPnt.Y >= prPnt.Y)
                {
                    curPnt = e.Location;
                    prPnt = X;
                }                

                if (curPnt.X >= X.X && curPnt.Y <= X.Y)
                {
                    prPnt.Y = e.Location.Y;
                    prPnt.X = X.X;
                    curPnt.Y = X.Y;
                    curPnt.X = e.Location.X;
                }

                if (curPnt.X <= X.X && curPnt.Y >= X.Y)
                {
                    prPnt.X = e.Location.X;
                    prPnt.Y = X.Y;
                    curPnt.X = X.X;
                    curPnt.Y = e.Location.Y;
                }

                if (curPnt.X <= X.X && curPnt.Y <= X.Y)
                {
                    curPnt = X;
                    prPnt = e.Location;
                }

                if (Instrument_box.Text == "Rectangle")                 
                    gp.AddRectangle(new Rectangle(prPnt.X, prPnt.Y, curPnt.X - prPnt.X, curPnt.Y - prPnt.Y));
                if (Instrument_box.Text == "Ellipse")
                    gp.AddEllipse(new Rectangle(prPnt.X, prPnt.Y, curPnt.X - prPnt.X, curPnt.Y - prPnt.Y));
            }



            pictureBox1.Refresh();
        }
        Color color = Color.Red;
        Color colorOrigin;
        float width;

        private void Clear_btn_Click(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);
            pictureBox1.Refresh();
        }

        private void Color_btn_Click(object sender, EventArgs e)
        {            
            ColorDialog cdlg = new ColorDialog();
            if (cdlg.ShowDialog() == DialogResult.OK)
                color = cdlg.Color;
            Color_btn.BackColor = color;
        }

        private void Pen_num_ValueChanged(object sender, EventArgs e)
        {
            width = (float)Pen_num.Value;            
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {            
                e.Graphics.DrawPath(new Pen(color, width), gp);            
        }

        private void Fill(Point mouseLoc)
        {     
            colorOrigin = bmp.GetPixel(mouseLoc.X, mouseLoc.Y);
            if (colorOrigin == color)
                return;
            q.Enqueue(mouseLoc);                       
            while (q.Count > 0)
            {                
                Point curPoint = q.Dequeue();
                Step(curPoint.X + 1, curPoint.Y);
                Step(curPoint.X - 1, curPoint.Y);
                Step(curPoint.X, curPoint.Y + 1);
                Step(curPoint.X, curPoint.Y - 1);
            }
            pictureBox1.Refresh();

        }

        private void Step(int x, int y)
        {
            if (x < 0) return;
            if (y < 0) return;
            if (x >= bmp.Width) return;
            if (y >= bmp.Height) return;
            if (bmp.GetPixel(x, y) != colorOrigin) return;
            bmp.SetPixel(x, y, color);
            q.Enqueue(new Point(x, y));
        }
        Random rpx = new Random();
        private void Spray(Point mouseLoc)                      //////////////////////////////////////////////
        {
        SprayBegin:

            Point rp = new Point();
            
            rp.X = rpx.Next((int)mouseLoc.X - (int)width / 2, (int)mouseLoc.X + (int)width / 2);            
            rp.Y = rpx.Next((int)mouseLoc.Y - (int)width / 2, (int)mouseLoc.Y + (int)width / 2);
            if ((rp.X - mouseLoc.X) * (rp.X - mouseLoc.X) + (rp.Y - mouseLoc.Y) * (rp.Y - mouseLoc.Y) < (width / 2) * (width / 2))
                bmp.SetPixel(rp.X, rp.Y, color);
            else goto SprayBegin;            
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
                pictureBox1.Image.Save(sfd.FileName);
        }
    }
}