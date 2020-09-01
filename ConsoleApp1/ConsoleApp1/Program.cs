using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static int startPoint;
        static int endPoint;
        static void Main(string[] args)
        {
            List<Vertex> vertices = GetData();
            foreach (Vertex vtx in vertices)
            {
                Console.WriteLine(vtx.Diag());
            }

            // Find the shortest path
            Queue<Vertex> starting = new Queue<Vertex>();
            starting.Enqueue(vertices[startPoint]);
            vertices[startPoint].Discovered = true;
            vertices[startPoint].Cost = 0;
            while(starting.Count > 0)
            {
                Vertex currentVtx = starting.Dequeue();
                foreach(Edge nb in currentVtx.Neighbours)
                {
                    if (!nb.Dest.Discovered || nb.Dest.Cost > currentVtx.Cost + nb.Weight)
                    {
                        nb.Dest.Cost = currentVtx.Cost + nb.Weight;
                        nb.Dest.Parent = currentVtx;
                    }
                    if (!nb.Dest.Proccessed) starting.Enqueue(nb.Dest);
                }
            }
            
            Console.ReadLine();
        } // Main
        static void PrintResult()
        {

        } // Print Result
        static List<Vertex> GetData()
        {
            TextReader stdin = Console.In;
            Console.SetIn(new StreamReader("graph.txt"));
            List<Vertex> vertices = new List<Vertex>();
            int vertexNum = int.Parse(Console.ReadLine());
            for (int i = 0; i < vertexNum; i++)
            {
                vertices.Add(new Vertex(i));
            }
            string line = Console.ReadLine().Trim();
            string[] parts = line.Split(' ');
            startPoint = int.Parse(parts[0]);
            endPoint = int.Parse(parts[1]);

            while ((line = Console.ReadLine()) != null)
            {
                parts = line.Split(' ');
                int n1 = int.Parse(parts[0]);
                int n2 = int.Parse(parts[1]);
                int n3 = int.Parse(parts[2]);
                vertices[n1].Neighbours.Add(new Edge(vertices[n2], n3));
            }
            Console.SetIn(stdin);
            return vertices;
        } // GetData
    } // Class Program

    class Vertex
    {
        public int Id { get; set; }
        public List<Edge> Neighbours { get; set; }
        public Vertex Parent { get; set; }
        public int Cost { get; set; }
        public bool Discovered { get; set; }
        public bool Proccessed { get; set; }

        public Vertex(int id)
        {
            Id = id;
            Discovered = false;
            Neighbours = new List<Edge>();
            Cost = int.MaxValue;
        }

        public string Diag()
        {
            string result = string.Format("{0}:", Id);
            foreach (Edge nb in Neighbours)
            {
                result += string.Format(" {0}", nb);
            }
            return result;
        } // Diag
    } // Class Vertex

    class Edge
    {
        public Vertex Dest { get; set; }
        public int Weight { get; set; }
        public Edge(Vertex dst, int wt)
        {
            Dest = dst;
            Weight = wt;
        }
        public override string ToString()
        {
            return string.Format("{0}({1})", Dest.Id, Weight);
        }
    } // Class Edge
}
