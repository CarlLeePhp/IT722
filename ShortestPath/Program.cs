using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Collections;

namespace ShortestPath
{
    class Program
    {
        static int startPoint;
        static int endPoint;
        static void Main(string[] args)
        {
            Queue<Vertex> starting = new Queue<Vertex>();
            List<Vertex> vertices = GetData();

            starting.Enqueue(vertices[startPoint]);
            vertices[startPoint].Discovered = true;

            while(starting.Count > 0)
            {
                bool isEndPoint = false;
                Vertex currentVertex = starting.Dequeue();
                foreach(Vertex vtx in currentVertex.Neighbours)
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

            // print the shortest path
            PrintPath(vertices);

            //Console.ReadLine();
        } // Main

        static void PrintPath(List<Vertex> vertices)
        {
            Stack<Vertex> vertexPath = new Stack<Vertex>();
            string result = "";
            int lastPoint = endPoint;
            
            while(lastPoint != startPoint)
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
            //TextReader stdin = Console.In;
            //Console.SetIn(new StreamReader("graph.txt"));

            // Create Vertices
            List<Vertex> vertices = new List<Vertex>();
            int vertexNum = int.Parse(Console.ReadLine());
            for(int i = 0; i < vertexNum; i++)
            {
                vertices.Add(new Vertex(i));
            }

            string line = Console.ReadLine();
            string[] parts = line.Split(' ');
            startPoint = int.Parse(parts[0]);
            endPoint = int.Parse(parts[1]);

            // Build Graph
            while((line = Console.ReadLine()) != null)
            {
                parts = line.Split(' ');
                int n1 = int.Parse(parts[0]);
                int n2 = int.Parse(parts[1]);
                vertices[n1].Neighbours.Add(vertices[n2]);
            }

            //Console.SetIn(stdin);
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
            string result = string.Format("{0} :", Id);
            foreach(Vertex vtx in Neighbours)
            {
                result += string.Format(" {0}", vtx.Id);
            }
            return result;
        } // Diag
    } // Class Vertex
}
