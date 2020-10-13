#define LOCALX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConvexHullFinding
{
    class Program
    {
        static int caseNum;
        static List<List<Point>> pointCase = new List<List<Point>>();
        static void Main(string[] args)
        {
            List<Point> results;
            GetData();
            for(int caseIndex = 0; caseIndex < caseNum; caseIndex++)
            {
                results = new List<Point>();
                results.Add(pointCase[caseIndex][0]);
                results.Add(pointCase[caseIndex][1]);
                for(int i=2; i < pointCase[caseIndex].Count; i++)
                {
                    results.Add(pointCase[caseIndex][i]);
                    while (results.Count > 2 && SignedArea(results[results.Count - 3], results[results.Count - 2], results[results.Count - 1]) < 0)
                    {
                        results.RemoveAt(results.Count - 2);
                    }
                }
                pointCase[caseIndex] = results;
            }

            // print results
            Console.WriteLine(pointCase.Count);
            for(int caseIndex = 0; caseIndex < pointCase.Count; caseIndex++)
            {
                if (caseIndex != 0) Console.WriteLine(-1);
                Console.WriteLine(pointCase[caseIndex].Count + 1);
                foreach(Point point in pointCase[caseIndex])
                {
                    Console.WriteLine(point);
                }
                Console.WriteLine(pointCase[caseIndex][0]);
            }
            
#if LOCAL
            Console.ReadLine();
#endif
        } // Main
        static void GetData()
        {
#if LOCAL
            TextReader stdin = Console.In;
            Console.SetIn(new StreamReader("graph.txt"));
#endif
            string line = Console.ReadLine().Trim();
            caseNum = int.Parse(line);
            int pointNum;
            List<Point> points;
            string[] parts;
            for (int i = 0; i < caseNum; i++)
            {
                line = Console.ReadLine().Trim();
                if (line == "-1") line = Console.ReadLine().Trim();
                pointNum = int.Parse(line);
                points = new List<Point>();
                for(int j=0; j < pointNum; j++)
                {
                    line = Console.ReadLine();
                    parts = line.Split(' ');
                    double x = double.Parse(parts[0]);
                    double y = double.Parse(parts[1]);
                    points.Add(new Point(x, y));
                }
                points.RemoveAt(points.Count - 1);
                for(int m=1; m<points.Count; m++)
                {
                    if (points[m].Y < points[0].Y || (points[m].Y == points[0].Y && points[m].X < points[0].X))
                    {
                        Point tmp = points[0];
                        points[0] = points[m];
                        points[m] = tmp;
                    }
                }
                Point pivot = points[0];
                IComparer<Point> radioSort = new RadioSort(pivot);
                points.Sort(radioSort);
                pointCase.Add(points);
            }
#if LOCAL
            Console.SetIn(stdin);
#endif
        } // Get Data
        static double SignedArea(Point a, Point b, Point c)
        {
            return a.X * b.Y + a.Y * c.X + b.X * c.Y
                - a.X * c.Y - b.X * a.Y - c.X * b.Y;
        }
    }
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
            if (this.Y == other.Y) return this.X.CompareTo(other.X);
            return this.Y.CompareTo(other.Y);
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", X, Y);
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
            if (x == pivot)
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
