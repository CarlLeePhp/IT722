#define LOCAL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UbiquitousReligions
{
    class Program
    {
        static List<List<Vertex>> vtxCases = new List<List<Vertex>>();
        static List<List<Edge>> edgeCases = new List<List<Edge>>();
        static void Main(string[] args)
        {
            GetData();
            for(int caseIndex = 1; caseIndex <= vtxCases.Count; caseIndex++)
            {
                
                // Simple
                foreach(Edge edge in edgeCases[caseIndex - 1])
                {
                    SimpleMerge(vtxCases[caseIndex - 1], edge);
                }
                // End of Simple
                PrintResult(vtxCases[caseIndex - 1], caseIndex);
            }
#if LOCAL
            Console.ReadLine();
#endif
        } // Main
        static void SimpleMerge(List<Vertex> vertices, Edge edge)
        {
            if (edge.VtxOne.GroupID == edge.VtxTwo.GroupID) return;
            int searchGroupID = edge.VtxTwo.GroupID;
            int targetGroupID = edge.VtxOne.GroupID;
            foreach(Vertex vtx in vertices)
            {
                if (vtx.GroupID == searchGroupID) vtx.GroupID = targetGroupID;
            }

        }
        static void PrintResult(List<Vertex> vertices, int caseIndex)
        {
            HashSet<int> groupNum = new HashSet<int>();
            foreach(Vertex vtx in vertices)
            {
                groupNum.Add(vtx.GroupID);
            }
            Console.WriteLine(string.Format("Case {0}: {1}", caseIndex, groupNum.Count));
        }
        static void GetData()
        {
#if LOCAL
            TextReader stdin = Console.In;
            Console.SetIn(new StreamReader("graph.txt"));
#endif
            string line = Console.ReadLine().Trim();
            string[] parts;
            int vtxNum;
            int edgeNum;
            int v1;
            int v2;
            List<Vertex> vertices;
            List<Edge> edges;
            while(line != "0 0")
            {
                parts = line.Split(' ');
                vtxNum = int.Parse(parts[0]);
                edgeNum = int.Parse(parts[1]);
                vertices = new List<Vertex>();
                for(int i=0; i < vtxNum; i++)
                {
                    vertices.Add(new Vertex(i + 1));
                }
                vtxCases.Add(vertices);
                // Add edges
                edges = new List<Edge>();
                for(int i = 0; i < edgeNum; i++)
                {
                    line = Console.ReadLine().Trim();
                    parts = line.Split(' ');
                    v1 = int.Parse(parts[0]);
                    v2 = int.Parse(parts[1]);
                    edges.Add(new Edge(vertices[v1 - 1], vertices[v2 - 1]));
                }
                edgeCases.Add(edges);

                line = Console.ReadLine().Trim();
            }
#if LOCAL
            Console.SetIn(stdin);
#endif
        }
    }

    class Vertex
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        public Vertex(int id)
        {
            ID = id;
            GroupID = id;
        }
    }
    class Edge
    {
        public Vertex VtxOne { get; set; }
        public Vertex VtxTwo { get; set; }
        public Edge(Vertex v1, Vertex v2)
        {
            VtxOne = v1;
            VtxTwo = v2;
        }

       
    }
}
