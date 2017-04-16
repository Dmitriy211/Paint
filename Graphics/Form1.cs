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

namespace Graphic
{
    public partial class Form1 : Form
    {
        //Graphics g;
        Pen pen = new Pen(Color.Blue, 5);        
        GraphicsPath g;
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush CoralBrush = new SolidBrush(Color.Coral);
        SolidBrush whBrush = new SolidBrush(Color.White);
        SolidBrush blueBrush = new SolidBrush(Color.Blue);

       /* LinearGradientBrush linGrBrush = new LinearGradientBrush(
           new Point(0, 10),
           new Point(200, 10),
           Color.FromArgb(255, 255, 0, 0),   // Opaque red
           Color.FromArgb(255, 0, 0, 255));  // Opaque blue*/


        public Form1()
        {
            InitializeComponent();
           //g = CreateGraphics();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        void DrawStar(PaintEventArgs e, float a, float b, float m)  // a = center x , b = center y, m = size multiplying
        {
            g = new GraphicsPath();

            g.AddLine(m * 2 + a, b, m * 4 + a, m * 2 + b);
            g.AddLine(m * 4 + a, m * 2 + b, m * 6 + a, b);
            g.AddLine(m * 6 + a, b, m * 6 + a, m * 3 + b);
            g.AddLine(m * 6 + a, m * 3 + b, m * 8 + a, m * 4 + b);
            g.AddLine(m * 8 + a, m * 4 + b, m * 6 + a, m * 5 + b);
            g.AddLine(m * 6 + a, m * 5 + b, m * 6 + a, m * 8 + b);
            g.AddLine(m * 6 + a, m * 8 + b, m * 4 + a, m * 6 + b);
            g.AddLine(m * 4 + a, m * 6 + b, m * 2 + a, m * 8 + b);
            g.AddLine(m * 2 + a, m * 8 + b, m * 2 + a, m * 5 + b);
            g.AddLine(m * 2 + a, m * 5 + b, a, m * 4 + b);
            g.AddLine(a, m * 4 + b, m * 2 + a, m * 3 + b);
            g.AddLine(m * 2 + a, m * 3 + b, m * 2 + a, b);
            
            e.Graphics.FillPath(CoralBrush, g);            
        }

  
        void DrawShip(PaintEventArgs e, float a, float b, float m)
        {           
            e.Graphics.FillEllipse(blueBrush, a, b, 4 * m, 4 * m);
            e.Graphics.FillEllipse(whBrush, a + m, b + m, 2 * m, 2 * m);
            e.Graphics.DrawLine(pen, a, b, a + m, b + m);
            e.Graphics.DrawLine(pen, a + 4 * m, b, a + 3 * m, b + m);
            e.Graphics.DrawLine(pen, a + 4 * m, b + 4 * m, a + 3 * m, b + 3 * m);
            e.Graphics.DrawLine(pen, a, b + 4 * m, a + m, b + 3 * m);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < Width; i += 100)
                for (int j = 0; j < Height; j += 100)
                    e.Graphics.FillEllipse(whBrush, i, j, 2, 2);
            for (int i = 50; i < Width; i += 100)
                for (int j = 50; j < Height; j += 100)
                    e.Graphics.FillEllipse(whBrush, i, j, 2, 2);

            DrawStar(e, 0, 0, 10);
            DrawStar(e, 250, 50, 10);
            DrawStar(e, 180, 200, 10);
            DrawStar(e, 400, 100, 10);
            DrawStar(e, 600, 250, 10);
            DrawStar(e, 700, 0, 10);
            DrawStar(e, 400, 300, 10);

            Bitmap bitmap = new Bitmap("Bez-imeni-2-_2.gif");  // Раскомменчивать только в крайних случаях
            e.Graphics.DrawImage(bitmap, 350, 20);

            DrawShip(e, 100, 100, 10);
        }
    }
}
