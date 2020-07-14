using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GraphDemo02
{
    class Program
    {
        static void Main(string[] args)
        {
            //Vertex[] vertices = GetData();
            //for(int i=0; i< vertices.Length; i++)
            //{
            //    Console.WriteLine(i + ": " + vertices[i].ToString());
            //}

            Dictionary<int, List<int>> graph = GetData();
            foreach(KeyValuePair<int, List<int>> kvp in graph)
            {
                kvp.Value.Sort();
                string neighbors = "";
                foreach(int neighbor in kvp.Value)
                {
                    neighbors = neighbors + neighbor + " ";
                }
                neighbors.Trim();
                Console.WriteLine(kvp.Key + ": " + neighbors);
            }
            Console.ReadKey();

        }

        //static Vertex[] GetData()
        //{
        //    string[] lines = File.ReadAllLines(@"graph.txt");
        //    int vertexNum = int.Parse(lines[0]);
        //    Vertex[] vertices = new Vertex[vertexNum];
        //    for(int i=0; i<vertexNum; i++)
        //    {
        //        vertices[i] = new Vertex(i);
        //    }
        //    for(int i=1; i< lines.Length; i++)
        //    {
        //        string[] items = lines[i].Split(' ');
        //        int index = int.Parse(items[0]);
        //        int neighbor = int.Parse(items[1]);

        //        vertices[index].AddEdge(vertices[neighbor]);
        //    }
        //    return vertices;
        //}
        static Dictionary<int, List<int>> GetData()
        {
            TextReader stdin = Console.In;
            // Console.SetIn(new StreamReader("graph.txt"));
            Console.SetIn(stdin);

            string line = Console.ReadLine().Trim();
            int vertexNum = int.Parse(line);
            Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();
            
            for (int i = 0; i < vertexNum; i++)
            {
                graph.Add(i, new List<int>());
            }

            while((line = Console.ReadLine()) != null)
            {
                string[] items = line.Split(' ');
                int index = int.Parse(items[0]);
                int neighbor = int.Parse(items[1]);
                graph[index].Add(neighbor);
            }
            
            return graph;
        }

        
    }
}
