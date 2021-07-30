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
        bool isRadio = false;
        public MainForm()
        {
            InitializeComponent();
            
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
            if (isRadio)
            {
                for (int i = 1; i < points.Count; i++)
                {
                    g.DrawLine(linePen, points[0], points[i]);
                    g.DrawString(i.ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), points[i]);
                }

                // draw hull
                for(int i=0; i< hullPoints.Count; i++)
                {
                    if(i == 0)
                    {
                        g.DrawLine(hullPen, hullPoints[hullPoints.Count - 1], hullPoints[i]);
                    }
                    else
                    {
                        g.DrawLine(hullPen, hullPoints[i-1], hullPoints[i]);
                    }
                    
                }
                // draw circle

            }
            

        }

        private void DrawPoint(Graphics g, Pen myPen, Point point)
        {
            g.DrawLine(myPen, point.X - 5, point.Y, point.X + 5, point.Y);
            g.DrawLine(myPen, point.X , point.Y - 5, point.X, point.Y + 5);
        }
        
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            points.Add(new Point(e.X, e.Y));
            pictureBox.Refresh();
        }
        
        private void buttonSort_Click(object sender, EventArgs e)
        {
            // left lowest
            for(int i=1; i<points.Count; i++)
            {
                if(points[i].X < points[0].X)
                {
                    SwapPoints(0, i);
                }
                else if(points[i].X == points[0].X && points[i].Y > points[0].Y)
                {
                    SwapPoints(0, i);
                }
            }

            // sort others
            // bulb sort
            //for(int i=1;i<points.Count - 1; i++)
            //{
            //    for(int j = i + 1; j < points.Count; j++)
            //    {
            //        if (SignedArea(points[0], points[i], points[j]) > 0) SwapPoints(i, j);
            //    }
            //}

            // IComparer
            IComparer<Point> radioSort = new RadioSort(points[0]);
            points.Sort(radioSort);

            // get hull
            hullPoints.Clear();
            hullPoints.Add(points[0]);
            hullPoints.Add(points[1]);
            for(int i=2; i < points.Count; i++)
            {
                hullPoints.Add(points[i]);
                while(!CheckHull())
                {
                    hullPoints.RemoveAt(hullPoints.Count - 2);
                }
            }

            // get circle
            // c^2 = a^2 + b^2 - 2ab cosC
            // cosC = (a^2 + b^2 - c^2) / 2ab
            double cosC = -1;
            Point pointA = hullPoints[0];
            Point pointB = hullPoints[1];
            Point pointC;
            for(int i=2; i < hullPoints.Count; i++)
            {
                double c = Distance(pointA, pointB);
                double a = Distance(pointB, hullPoints[i]);
                double b = Distance(pointA, hullPoints[i]);
                double cos = (Math.Pow(a, 2) + Math.Pow(b, 2) - Math.Pow(c, 2)) / (2 * a * b);
                if (cos > cosC)
                {
                    cosC = cos;
                    pointC = hullPoints[i];
                }
            }

            isRadio = true;
            pictureBox.Refresh();
        }

        private Circle GetCircleFromTreePoints(Point pointA, Point pointB, Point pointC)
        {
            double maxEdge;
            double a = Distance(pointB, pointC);
            double b = Distance(pointC, pointA);
            double c = Distance(pointA, pointB);
            double cosA = GetCosByPoints(pointB, pointC, pointA);
            double cosB = GetCosByPoints(pointC, pointA, pointB);
            double cosC = GetCosByPoints(pointA, pointB, pointC);
            double cosMin = cosA;
            maxEdge = a;
            Point center = new Point((pointB.X + pointC.X) / 2, (pointB.Y + pointC.Y) / 2); ;
            double radius;
            if (cosB < cosMin)
            {
                cosMin = cosB;
                maxEdge = b;
                center = new Point((pointA.X + pointC.X)/2, (pointA.Y + pointC.Y)/2);
            }
            if (cosC < cosMin)
            {
                cosMin = cosC;
                maxEdge = c;
                center = new Point((pointA.X + pointB.X) / 2, (pointA.Y + pointB.Y) / 2);
            }
            if(cosMin <= 0) // > 90 degree
            {
                radius = maxEdge / 2;
                return new Circle(center.X, center.Y, radius);
            }

            return new Circle(0, 0, 0);
                
            
        }
        private double GetCosByPoints(Point pointA, Point pointB, Point pointC)
        {
            double c = Distance(pointA, pointB);
            double a = Distance(pointB, pointC);
            double b = Distance(pointA, pointC);
            double cos = (Math.Pow(a, 2) + Math.Pow(b, 2) - Math.Pow(c, 2)) / (2 * a * b);
            return cos;
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
        private double Distance(Point a, Point b)
        {
            return Math.Sqrt(
                Math.Pow(a.X - b.X, 2)
                + Math.Pow(a.Y - b.Y, 2)
                );
        }
        private void SwapPoints(int indexOne, int indexTwo)
        {
            Point tmp = points[indexOne];
            points[indexOne] = points[indexTwo];
            points[indexTwo] = tmp;
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
