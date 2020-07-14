using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace TopologicalDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Vertex[] vertices = GetData();
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Edges.Sort();
                Console.WriteLine(i + ": " + vertices[i].ToString());
            }

            // Console.ReadKey();
        } // Main

        static Vertex[] GetData()
        {
            TextReader stdin = Console.In;
            Console.SetIn(new StreamReader("graph.txt"));
            // Console.SetIn(stdin);

            string line = Console.ReadLine().Trim();
            int vertexNum = int.Parse(line);
            Vertex[] vertices = new Vertex[vertexNum];
            for (int i = 0; i < vertexNum; i++)
            {
                vertices[i] = new Vertex(i);
            }

            while ((line = Console.ReadLine()) != null) // Ctrl + z -> null, Ctrl + d (linux)
            {
                string[] items = line.Split(' ');
                int index = int.Parse(items[0]);
                int neighbor = int.Parse(items[1]);
                vertices[index].AddEdge(vertices[neighbor]);
            }

            return vertices;
        }
    } // Program


    class Vertex : IComparable
    {
        public int ID { get; set; }
        public List<Vertex> Edges { get; set; }
        public int IncomingNum { get; set; }
        public Vertex(int id)
        {
            ID = id;
            Edges = new List<Vertex>();
            IncomingNum = 0;
        }

        public void AddEdge(Vertex vertex)
        {
            Edges.Add(vertex);

        }

        public override string ToString()
        {
            string result = "";
            foreach (Vertex vertex in Edges)
            {
                result = result + vertex.ID + " ";
            }
            result = result.Trim();
            return result;
        }

        public int CompareTo(Object obj)
        {
            Vertex vertex = obj as Vertex;
            return ID.CompareTo(vertex.ID);
        }
    }
}
