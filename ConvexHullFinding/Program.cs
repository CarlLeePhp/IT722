#define LOCAL
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
                results.Sort();
                pointCase[caseIndex] = results;
            }

            foreach(List<Point> points in pointCase)
            {
                foreach(Point point in points)
                {
                    Console.WriteLine(point.X + " : " + point.Y);
                }
                Console.WriteLine("****Next Case****");
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
    }
}
