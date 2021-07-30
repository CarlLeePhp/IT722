#define LOCALX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChemicalComposition
{
    class Program
    {
        static List<List<Point>> factories = new List<List<Point>>();
        static void Main(string[] args)
        {
            GetData();
            for(int i=0; i < factories.Count; i++)
            {
                FindConvexHull(factories[i], i + 1);
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
            int numFactory = int.Parse(line);
            int numSample;
            string[] parts;
            List<Point> samples;
            for(int i=0; i<numFactory; i++)
            {
                line = Console.ReadLine().Trim();
                parts = line.Split(' ');
                numSample = int.Parse(parts[1]);
                samples = new List<Point>();
                for(int j = 0; j < numSample; j++)
                {
                    line = Console.ReadLine().Trim();
                    parts = line.Split(' ');
                    int x = int.Parse(parts[0]);
                    int y = int.Parse(parts[1]);
                    samples.Add(new Point(x, y));
                }
                factories.Add(samples);
            }
#if LOCAL
            Console.SetIn(stdin);
#endif
        } // Get data
        static void FindConvexHull(List<Point> samples, int factoryId)
        {
            // lowest left point
            for(int i=1; i < samples.Count; i++)
            {
                if(samples[i].X < samples[0].X || 
                    (samples[i].X == samples[0].X && samples[i].Y < samples[0].Y))
                {
                    Point tmp = samples[0];
                    samples[0] = samples[i];
                    samples[i] = tmp;
                }
            }

            // radio sort
            RadioSort radioSort = new RadioSort(samples[0]);
            samples.Sort(radioSort);
            for(int i=0;i<samples.Count - 1;)
            {
                if(samples[i].X == samples[i+1].X && samples[i].Y == samples[i + 1].Y)
                {
                    samples.RemoveAt(i + 1);
                }
                else
                {
                    i++;
                }
            }

            Console.WriteLine(string.Format("{0} ({1},{2})", factoryId, samples[0].X, samples[0].Y));
            Console.WriteLine();
            for(int i = 1; i < samples.Count; i++)
            {
                Console.WriteLine(string.Format("({0},{1})",samples[i].X,samples[i].Y));
            }
            Console.WriteLine();

            // convex hull
            List<Point> selectedSamples = new List<Point>();
            selectedSamples.Add(samples[0]);
            selectedSamples.Add(samples[1]);
            for(int i=2; i<samples.Count; i++)
            {
                while(selectedSamples.Count > 1)
                {
                    int selectedNum = selectedSamples.Count;
                    double signedArea = MathHelper.SignedArea(
                            selectedSamples[selectedNum - 2],
                            selectedSamples[selectedNum - 1],
                            samples[i]
                        );
                    if(signedArea < 0 || Math.Abs(signedArea) < 0.00001)
                    {
                        selectedSamples.RemoveAt(selectedNum - 1);
                    }
                    else
                    {
                        break;
                    }
                }
                selectedSamples.Add(samples[i]);
            }

            
            foreach (Point point in selectedSamples)
            {
                Console.WriteLine(string.Format("({0},{1})", point.X, point.Y));
            }
            Console.WriteLine(string.Format("({0},{1})", selectedSamples[0].X, selectedSamples[0].Y));
            Console.WriteLine();
        } // find convex hull
    } // Class Program

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    } // class point
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
            if(Math.Abs(signedArea) < 0.00001)
            {
                double distanceX = MathHelper.PointsDistance(_pivot, x);
                double distanceY = MathHelper.PointsDistance(_pivot, y);
                return distanceX.CompareTo(distanceY);
            }
            if (signedArea > 0) return -1;
            return 1;
        }
    } // Radio sort
    static class MathHelper
    {
        public static double SignedArea(Point a, Point b, Point c)
        {
            return a.X * b.Y + a.Y * c.X + b.X * c.Y
                - a.X * c.Y - a.Y * b.X - b.Y * c.X;
        }
        public static double PointsDistance(Point a, Point b)
        {
            return Math.Sqrt(
                    Math.Pow(a.X - b.X, 2)
                    + Math.Pow(a.Y - b.Y,2)
                );
        }
    }
}
