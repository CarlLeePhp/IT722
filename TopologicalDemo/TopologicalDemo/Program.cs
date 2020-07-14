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
            List<Vertex> startPoints = GetStarts(vertices);
            List<Vertex> sortedPoints = new List<Vertex>();
            // Get a start Point each time
            while(startPoints.Count > 0)
            {
                Vertex currentPoint = startPoints[0];
                startPoints.RemoveAt(0);
                foreach(Vertex nextPoint in currentPoint.Edges)
                {
                    nextPoint.IncomingNum--;
                    if (nextPoint.IncomingNum == 0) startPoints.Add(nextPoint);
                }
                sortedPoints.Add(currentPoint);
            }

            // printe out the result
            foreach(Vertex sortedPoint in sortedPoints)
            {
                Console.WriteLine(sortedPoint.Description);
            }

            Console.ReadKey();
        } // Main

        static Vertex[] GetData()
        {
            TextReader stdin = Console.In;
            Console.SetIn(new StreamReader("graph.txt"));
            

            string line = Console.ReadLine().Trim();
            int vertexNum = int.Parse(line);
            Vertex[] vertices = new Vertex[vertexNum];
            for (int i = 0; i < vertexNum; i++)
            {
                string description = Console.ReadLine();
                vertices[i] = new Vertex(i, description);
            }

            while ((line = Console.ReadLine()) != null) // Ctrl + z -> null, Ctrl + d (linux)
            {
                string[] items = line.Split(' ');
                int scr = int.Parse(items[0]);
                int dst = int.Parse(items[1]);
                vertices[scr].AddEdge(vertices[dst]);
                vertices[dst].IncomingNum++;
            }
            Console.SetIn(stdin);
            return vertices;
        } // Get Data
        static List<Vertex> GetStarts(Vertex[] vertices)
        {
            List<Vertex> startPoints = new List<Vertex>();
            foreach(Vertex vertex in vertices)
            {
                if (vertex.IncomingNum == 0) startPoints.Add(vertex);
            }
            return startPoints;
        } // GetStarts
    } // Class Program


    class Vertex : IComparable
    {
        public int ID { get; set; }
        public List<Vertex> Edges { get; set; }
        public int IncomingNum { get; set; }
        public string Description { get; set; }
        public Vertex(int id, string description)
        {
            ID = id;
            Edges = new List<Vertex>();
            IncomingNum = 0;
            Description = description;
        }

        public void AddEdge(Vertex vertex)
        {
            Edges.Add(vertex);

        } // add edge

        public override string ToString()
        {
            string result = "";
            foreach (Vertex vertex in Edges)
            {
                result = result + vertex.ID + " ";
            }
            result = result.Trim();
            return result;
        } // to string

        public int CompareTo(Object obj)
        {
            Vertex vertex = obj as Vertex;
            return ID.CompareTo(vertex.ID);
        }
    } // Class Vertex
}
