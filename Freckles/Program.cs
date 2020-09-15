#define LOCAL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Freckles
{
    class Program
    {
        static List<List<Vertex>> caseVtx = new List<List<Vertex>>();
        static List<List<Edge>> caseEdge = new List<List<Edge>>();
        static void Main(string[] args)
        {
            GetData();
            for(int caseIndex = 0; caseIndex < caseVtx.Count; caseIndex++)
            {
                caseEdge.Add(new List<Edge>());
                foreach(Vertex vtx in caseVtx[caseIndex])
                {

                }
            }
#if LOCAL
            Console.ReadLine();
#endif
        }
        static void GetData()
        {
#if LOCAL
            TextReader stdin = Console.In;
            Console.SetIn(new StreamReader("graph.txt"));
#endif
            string line = Console.ReadLine();
            int caseNum = int.Parse(line);
            int freckleNum;
            List<Vertex> vertices;
            for(int i = 0; i < caseNum; i++)
            {
                line = Console.ReadLine(); // blank line
                line = Console.ReadLine();
                freckleNum = int.Parse(line);
                vertices = new List<Vertex>();
                double coordinateX;
                double coordinateY;
                string[] parts;
                for(int j = 0; j < freckleNum; j++)
                {
                    line = Console.ReadLine();
                    parts = line.Split(' ');
                    coordinateX = double.Parse(parts[0]);
                    coordinateY = double.Parse(parts[1]);
                    vertices.Add(new Vertex(j, coordinateX, coordinateY));
                }

                caseVtx.Add(vertices);
            }
#if LOCAL
            Console.SetIn(stdin);
#endif
        }
    }

    class Vertex
    {
        public int ID { get; set; }
        public double CoordinateX { get; set; }
        public double CoordinateY { get; set; }
        public Vertex(int id, double x, double y)
        {
            ID = id;
            CoordinateX = x;
            CoordinateY = y;
        }
        public string Diag()
        {
            return string.Format("{0} : {1:0.00} {2:0.00}", ID, CoordinateX, CoordinateY);
            
        }
    } // Vertex
    class Edge
    {
        public Vertex VtxOne { get; set; }
        public Vertex VtxTwo { get; set; }
        public double Distance { get; set; }
        public Edge(Vertex vtxOne, Vertex vtxTwo, double distance)
        {
            VtxOne = vtxOne;
            VtxTwo = vtxTwo;
            Distance = distance;
        }
    }
}
