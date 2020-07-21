using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace SolvingMaze1
{
    class Program
    {
        static int startPoint = 0;
        static int endPoint;
        static void Main(string[] args)
        {
            Queue<Vertex> starting = new Queue<Vertex>();
            List<Vertex> vertices = GetData();

            

            starting.Enqueue(vertices[startPoint]);
            vertices[startPoint].Discovered = true;

            while (starting.Count > 0)
            {
                bool isEndPoint = false;
                Vertex currentVertex = starting.Dequeue();
                foreach (Vertex vtx in currentVertex.Neighbours)
                {
                    if (!vtx.Discovered)
                    {
                        vtx.Discovered = true;
                        vtx.Parent = currentVertex;
                        starting.Enqueue(vtx);
                    }
                    if (vtx.Id == endPoint)
                    {
                        isEndPoint = true;
                        break;
                    }
                }
                if (isEndPoint) break;
            }
            foreach (Vertex vtx in vertices)
            {
                Console.WriteLine(vtx.Diag());
            }
            // print the shortest path
            PrintPath(vertices);

            Console.ReadLine();
        } // Main

        static void PrintPath(List<Vertex> vertices)
        {
            Stack<Vertex> vertexPath = new Stack<Vertex>();
            string result = "";
            int lastPoint = endPoint;

            while (lastPoint != startPoint)
            {
                vertexPath.Push(vertices[lastPoint]);
                lastPoint = vertices[lastPoint].Parent.Id;
            }
            //vertexPath.Push(vertices[startPoint]);

            //result = vertexPath.Pop().Id.ToString();
            result = vertices[startPoint].Id.ToString();
            while (vertexPath.Count > 0)
            {
                result += string.Format(" {0}", vertexPath.Pop().Id);
            }

            Console.WriteLine(result);
        } // PrintPath
        static List<Vertex> GetData()
        {
            TextReader stdin = Console.In;
            Console.SetIn(new StreamReader("graph.txt"));

            // Create Vertices
            List<Vertex> vertices = new List<Vertex>();
            endPoint = int.Parse(Console.ReadLine()) - 1;
            for (int i = 0; i <= endPoint; i++)
            {
                vertices.Add(new Vertex(i));
            }

            string line;
            // Build Graph
            while ((line = Console.ReadLine()) != null)
            {
                string[] parts = line.Split(' ');
                int n1 = int.Parse(parts[0]);
                int n2 = int.Parse(parts[1]);
                vertices[n1].Neighbours.Add(vertices[n2]);
                vertices[n2].Neighbours.Add(vertices[n1]);
            }

            Console.SetIn(stdin);
            return vertices;
        } // GetData
    } // Class Program

    class Vertex
    {
        public int Id { get; set; }
        public List<Vertex> Neighbours { get; set; }
        public Vertex Parent { get; set; }

        public bool Discovered { get; set; }

        public Vertex(int id)
        {
            Id = id;
            Discovered = false;
            Neighbours = new List<Vertex>();
        }

        public string Diag()
        {
            string result;
            if (Parent != null)
            {
                result = string.Format("{0} {1}:", Id, Parent.Id);
            }
            else
            {
                result = string.Format("{0} Null:", Id);
            }
            foreach (Vertex vtx in Neighbours)
            {
                result += string.Format(" {0}", vtx.Id);
            }
            return result;
        } // Diag
    } // Class Vertex
}
