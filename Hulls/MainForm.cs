using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Hulls
{
    public partial class MainForm : Form
    {
        List<Point> points = new List<Point>();
        List<Point> hullPoints = new List<Point>();
        bool isConvex = false;
        bool showRadio = false;
        bool showConvex = false;
        public MainForm()
        {
            InitializeComponent();
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            KeyPreview = true;
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen firstPen = new Pen(Color.Red);
            Pen restPen = new Pen(Color.Blue);
            Pen linePen = new Pen(Color.Black);
            Pen hullPen = new Pen(Color.Pink);
            bool isFirstPoint = true;
            foreach(Point point in points)
            {
                if (isFirstPoint)
                {
                    DrawPoint(g, firstPen, point);
                    isFirstPoint = false;
                }
                else
                {
                    DrawPoint(g, restPen, point);
                }
                
            }
            if (showRadio)
            {
                for (int i = 1; i < points.Count; i++)
                {
                    g.DrawLine(linePen, points[0], points[i]);
                    g.DrawString(i.ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), points[i]);
                }

                
            }

            if (showConvex)
            {
                // draw hull
                for (int i = 0; i < hullPoints.Count; i++)
                {
                    if (i == 0)
                    {
                        g.DrawLine(hullPen, hullPoints[hullPoints.Count - 1], hullPoints[i]);
                    }
                    else
                    {
                        g.DrawLine(hullPen, hullPoints[i - 1], hullPoints[i]);
                    }

                }
            }
            

        }

        private void DrawPoint(Graphics g, Pen myPen, Point point)
        {
            g.DrawLine(myPen, point.X - 5, point.Y, point.X + 5, point.Y);
            g.DrawLine(myPen, point.X , point.Y - 5, point.X, point.Y + 5);
        }
        
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            showRadio = false;
            showConvex = false;
            points.Add(new Point(e.X, e.Y));
            SortPoints();
            FindConvexHull();
            pictureBox.Refresh();
        }
        
        private void SortPoints()
        {
            if (points.Count == 0) return;
            // left lowest
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].X < points[0].X)
                {
                    SwapPoints(0, i);
                }
                else if (points[i].X == points[0].X && points[i].Y > points[0].Y)
                {
                    SwapPoints(0, i);
                }
            }


            // IComparer
            IComparer<Point> radioSort = new RadioSort(points[0]);
            points.Sort(radioSort);

        }
        private void FindConvexHull()
        {
            if (isConvex)
            {
                isConvex = false;
            }
            
            if (points.Count < 3) return;
            // get hull
            hullPoints.Clear();
            hullPoints.Add(points[0]);
            hullPoints.Add(points[1]);
            for (int i = 2; i < points.Count; i++)
            {
                hullPoints.Add(points[i]);
                while (!CheckHull())
                {
                    hullPoints.RemoveAt(hullPoints.Count - 2);
                }
            }
            isConvex = true;
            
        }
        
        private bool CheckHull()
        {
            int hullCount = hullPoints.Count;
            if (hullCount <= 3) return true;
            return (SignedArea(hullPoints[hullCount - 3], hullPoints[hullCount - 2], hullPoints[hullCount - 1]) < 0);
        }
        private double SignedArea(Point a, Point b, Point c)
        {
            return a.X * b.Y + a.Y * c.X + b.X * c.Y - a.X * c.Y - b.X * a.Y - c.X * b.Y;
        }
        private void SwapPoints(int indexOne, int indexTwo)
        {
            Point tmp = points[indexOne];
            points[indexOne] = points[indexTwo];
            points[indexTwo] = tmp;
        }

       
        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F2)
            {
                showConvex = false;
            }
            if(e.KeyCode == Keys.F1)
            {
                showRadio = false;
            }
            pictureBox.Refresh();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if(!showConvex)
                {
                    showConvex = true;

                    pictureBox.Refresh();
                }
                
            }
            if (e.KeyCode == Keys.F1)
            {
                if(!showRadio)
                {
                    showRadio = true;
                    pictureBox.Refresh();
                }
                
            }
            
        }

        
    }

    public class RadioSort : IComparer<Point>
    {
        Point pivot;
        public RadioSort(Point pivot)
        {
            this.pivot = pivot;
        }
        public int Compare(Point x, Point y)
        {
            if(SignedArea(pivot, x, y) > -0.001 && SignedArea(pivot, x, y) < 0.001)
            {
                return 0;
            }
            else if(y == pivot || SignedArea(pivot, x, y) < 0)
            {
                return -1;
            }
            else if(x == pivot)
            {
                return 1;
            }
            else
            {
                return 1;
            }
        }
        private double SignedArea(Point a, Point b, Point c)
        {
            return a.X * b.Y + a.Y * c.X + b.X * c.Y - a.X * c.Y - b.X * a.Y - c.X * b.Y;
        }
        
    }
}
