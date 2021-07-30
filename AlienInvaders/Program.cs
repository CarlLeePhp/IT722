#define LOCALX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AlienInvaders
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Point> points =  GetData();

            // find lowest left
            for(int i=1; i<points.Count; i++)
            {
                if(points[i].X < points[0].X ||
                    (points[i].X == points[0].X && points[i].Y < points[0].Y))
                {
                    Point tmp = points[0];
                    points[0] = points[i];
                    points[i] = tmp;
                }
            }

            // radio sort
            RadioSort radioSort = new RadioSort(points[0]);
            points.Sort(radioSort);

            // convex hull
            List<Point> selectedPoints = new List<Point>();
            selectedPoints.Add(points[0]);
            selectedPoints.Add(points[1]);
            for(int i = 2; i < points.Count; i++)
            {
                while(selectedPoints.Count > 1 && (
                    MathHelper.SignedArea(selectedPoints[selectedPoints.Count -2],
                    selectedPoints[selectedPoints.Count -1],
                    points[i]) < 0.000001
                    ))
                {
                    selectedPoints.RemoveAt(selectedPoints.Count - 1);
                }
                selectedPoints.Add(points[i]);
            }

            foreach (Point point in selectedPoints)
            {
                Console.WriteLine(string.Format("({0},{1})", point.X, point.Y));
            }
            Console.WriteLine(string.Format("({0},{1})", selectedPoints[0].X, selectedPoints[0].Y));
#if LOCAL
            Console.ReadLine();
            
#endif
        } // Main
        static List<Point> GetData()
        {
#if LOCAL
            TextReader stdin = Console.In;
            Console.SetIn(new StreamReader("graph.txt"));
#endif
            List<Point> points = new List<Point>();
            string line = Console.ReadLine();
            int pointNum = int.Parse(line);
            string[] parts;
            for(int i = 0; i < pointNum; i++)
            {
                line = Console.ReadLine();
                parts = line.Split(' ');
                double x = double.Parse(parts[0]);
                double y = double.Parse(parts[1]);
                points.Add(new Point(x, y));
            }
            
#if LOCAL
            Console.SetIn(stdin);
#endif
            return points;
        }
    } // Class Program
    class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    } // Class Point
    class RadioSort : IComparer<Point>
    {
        Point _pivot;
        public RadioSort(Point pivot)
        {
            _pivot = pivot;
        }
        public int Compare(Point x, Point y)
        {
            if (x == _pivot) return -1;
            double signedArea = MathHelper.SignedArea(_pivot, x, y);
            if (Math.Abs(signedArea) < 0.00001)
            {
                double distanceX = Math.Sqrt(
                    Math.Pow(_pivot.X - x.X, 2)
                    + Math.Pow(_pivot.Y - x.Y, 2)
                    );
                double distanceY = Math.Sqrt(
                    Math.Pow(_pivot.X - y.X, 2)
                    + Math.Pow(_pivot.Y - y.Y, 2)
                    );
                return distanceX.CompareTo(distanceY);
            }
            if (signedArea > 0) return -1;
            return 1;
        }
    } // Class Radio sort
    static class MathHelper
    {
        public static double SignedArea(Point a, Point b, Point c)
        {
            return a.X * b.Y + a.Y * c.X + b.X * c.Y
                - a.X * c.Y - a.Y * b.X - b.Y * c.X;
        }
    }
}
