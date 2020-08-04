#define LOCAL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace MinispanningTree
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Vertex> vertices = GetData();
            foreach(Vertex vtx in vertices)
            {
                Console.WriteLine(vtx.Diag());
            }
            Console.ReadLine();
        } // Main
        static List<Vertex> GetData()
        {
#if LOCAL
            TextReader stdin = Console.In;
            Console.SetIn(new StreamReader("graph.txt"));
#endif
            List<Vertex> vertices = new List<Vertex>();
            
            string line = Console.ReadLine().Trim();
            string[] parts = line.Split(' ');
            int vtxNum = int.Parse(parts[0]);
            int edgeNum = int.Parse(parts[1]);
            for (int i = 0; i < vtxNum; i++)
            {
                vertices.Add(new Vertex(i + 1));
            }
            while ((line = Console.ReadLine()) != null)
            {
                parts = line.Split(' ');
                int n1 = int.Parse(parts[0]);
                int n2 = int.Parse(parts[1]);
                int n3 = int.Parse(parts[2]);
                vertices[n1 - 1].Adjacents.Add(new Edge(vertices[n2 - 1], n3));
            }
#if LOCAL
            Console.SetIn(stdin);
#endif
            return vertices;
        } // Get Data
    } // Class Program

    class Vertex
    {
        public int Id { get; set; }
        public List<Edge> Adjacents { get; set; }
        public Vertex Parent { get; set; }
        public int Key { get; set; }
        public bool Discovered { get; set; }
        public bool Visted { get; set; }

        public Vertex(int id)
        {
            Id = id;
            Discovered = false;
            Visted = false;
            Adjacents = new List<Edge>();
            Key = int.MaxValue;
        }

        public string Diag()
        {
            string result = "";
            if(Parent != null)
            {
                result = string.Format("{0}|{1}|{2}:", Id, Parent.Id, Key);
            }
            else
            {
                result = string.Format("{0}|{1}|{2}:", Id, "null", Key);
            }
            foreach (Edge nb in Adjacents)
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
