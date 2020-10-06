#define LOCALX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HerdingFrosh
{
    class Program
    {
        static List<List<Point>> cases = new List<List<Point>>();
        static void Main(string[] args)
        {
            GetData();
            foreach(List<Point> points in cases)
            {
                PrintSilkLength(points);
            }

#if LOCAL
            Console.ReadLine();
#endif
        } // Main
        static void PrintSilkLength(List<Point> points)
        {
            
            for(int i=1;i<points.Count; i++)
            {
                if(points[i].X < points[0].X || (points[i].X == points[0].X && points[i].Y < points[0].Y) )
                {
                    Point tmp = points[0];
                    points[0] = points[i];
                    points[i] = tmp;
                }
            }
            Point pivot = points[0];
            IComparer<Point> radioSort = new RadioSort(pivot);
            points.Sort(radioSort);

            double result = 0.0;
            double tieLength = Math.Sqrt (Math.Pow(points[0].X, 2) + Math.Pow(points[0].Y, 2)) * 2.0 + 2.0;

            List<Point> hullPoints = new List<Point>();
            hullPoints.Add(points[0]);
            for(int i=1; i < points.Count; i++)
            {
                while(hullPoints.Count > 1 && SignedArea(hullPoints[hullPoints.Count - 2], hullPoints[hullPoints.Count - 1], points[i]) < 0)
                {
                    hullPoints.RemoveAt(hullPoints.Count - 1);
                }
                hullPoints.Add(points[i]);
            }

            for(int i=0; i<hullPoints.Count - 1; i++)
            {
                result += Math.Sqrt(
                    Math.Pow(hullPoints[i].X - hullPoints[i + 1].X, 2) +
                    Math.Pow(hullPoints[i].Y - hullPoints[i + 1].Y, 2)
                    );
            }
            result += Math.Sqrt(
                    Math.Pow(hullPoints[0].X - hullPoints[hullPoints.Count - 1].X, 2) +
                    Math.Pow(hullPoints[0].Y - hullPoints[hullPoints.Count - 1].Y, 2)
                    );
            result += tieLength;
            Console.WriteLine(string.Format("{0:0.00}", result));
        }
        static double SignedArea(Point a, Point b, Point c)
        {
            return a.X * b.Y + a.Y * c.X + b.X * c.Y - a.X * c.Y - b.X * a.Y - c.X * b.Y;
        }
        static void GetData()
        {
#if LOCAL
            TextReader stdin = Console.In;
            Console.SetIn(new StreamReader("graph.txt"));
#endif
            string line = Console.ReadLine().Trim();
            string[] parts;
            int caseNum = int.Parse(line);
            int pointNum;
            List<Point> points;
            for(int i = 0; i< caseNum; i++)
            {
                line = Console.ReadLine().Trim();
                pointNum = int.Parse(line);
                points = new List<Point>();
                for(int j = 0; j < pointNum; j++)
                {
                    line = Console.ReadLine().Trim();
                    parts = line.Split(' ');
                    double x = double.Parse(parts[0]);
                    double y = double.Parse(parts[1]);
                    points.Add(new Point(x, y));
                }
                cases.Add(points);
            }
#if LOCAL
            Console.SetIn(stdin);
#endif
        }
    } // Class Program
    class Point : IComparable<Point>
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public int CompareTo(Point other)
        {
            if (this.X == other.X) return this.Y.CompareTo(other.Y);
            return this.X.CompareTo(other.X);
        }
    }
    class RadioSort : IComparer<Point>
    {
        Point pivot;
        public RadioSort(Point pivot)
        {
            this.pivot = pivot;
        }
        public int Compare(Point x, Point y)
        {
            if(x == pivot)
            {
                return 1;
            }
            if (SignedArea(pivot, x, y) > -0.001 && SignedArea(pivot, x, y) < 0.001)
            {
                double distanceX = Math.Sqrt(
                    Math.Pow(pivot.X - x.X, 2) +
                    Math.Pow(pivot.Y - x.Y, 2)
                    );
                double distanceY = Math.Sqrt(
                    Math.Pow(pivot.X - y.X, 2) +
                    Math.Pow(pivot.Y - y.Y, 2)
                    );
                return distanceX.CompareTo(distanceY);
            }
            else if (y == pivot || SignedArea(pivot, x, y) > 0)
            {
                return -1;
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
