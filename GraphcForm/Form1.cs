using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphcForm
{
    public partial class Form1 : Form
    {
        Point a, b, c;
        int count = 0;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(count == 0)
            {
                a = new Point(e.X, e.Y);
                count++;
            }
            if (count == 1)
            {
                b = new Point(e.X, e.Y);
                count++;
            }
            if (count == 2)
            {
                c = new Point(e.X, e.Y);
                count++;
            }

        }

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if(count >= 1)
            {
                DrawPoint(g, a, Color.Red);

            }
            g.DrawRectangle(new Pen(Color.Red), 10, 10, 400, 400);
        }

        private void DrawPoint(Graphics g, Point point, Color color)
        {
            g.DrawLine(new Pen(color), point.X - 5, point.Y, point.X + 5, point.Y);
            g.DrawLine(new Pen(color), point.X, point.Y - 5, point.X, point.Y + 5);
        }
    }
}
