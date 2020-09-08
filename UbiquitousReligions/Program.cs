#define LOCALX
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
                //foreach(Edge edge in edgeCases[caseIndex - 1])
                //{
                //    SimpleMerge(vtxCases[caseIndex - 1], edge);
                //}
                //PrintResult(vtxCases[caseIndex - 1], caseIndex);
                // End of Simple

                // Union Pro
                int treeNum = vtxCases[caseIndex - 1].Count;
                foreach (Edge edge in edgeCases[caseIndex - 1])
                {
                    
                    int leftGroupID = GetGroupID(vtxCases[caseIndex -1], edge.VtxOne);
                    int rightGroupID = GetGroupID(vtxCases[caseIndex - 1], edge.VtxTwo);
                    if (leftGroupID == rightGroupID) continue;
                    // otherwise should be merge
                    if(edge.VtxTwo.ID != rightGroupID)  // it is not the root
                    // change the group ID of the root, the height of the tree must less than 3
                    {
                        vtxCases[caseIndex - 1][rightGroupID - 1].GroupID = leftGroupID;
                    }
                    
                    edge.VtxTwo.GroupID = leftGroupID;
                    treeNum--;
                }
                Console.WriteLine(string.Format("Case {0}: {1}", caseIndex, treeNum));
                // PrintResultPro(vtxCases[caseIndex - 1], caseIndex);
                // End of Union Pro

            }
#if LOCAL
            Console.ReadLine();
#endif
        } // Main
        static void PrintResultPro(List<Vertex> vertices,int caseIndex)
        {
            Vertex parentVtx = null;
            HashSet<int> groupNum = new HashSet<int>();
            foreach (Vertex vtx in vertices)
            {
                parentVtx = vertices[vtx.GroupID - 1];
                if(vtx.GroupID == vtx.ID || parentVtx.GroupID == parentVtx.ID)
                groupNum.Add(vtx.GroupID);
                while(parentVtx.GroupID != parentVtx.ID)
                {
                    parentVtx = vertices[parentVtx.GroupID - 1];
                }
                vtx.GroupID = parentVtx.GroupID;
                groupNum.Add(vtx.GroupID);
            }
            Console.WriteLine(string.Format("Case {0}: {1}", caseIndex, groupNum.Count));
        }
        static int GetGroupID(List<Vertex> vertices, Vertex vtx)
        {
            Vertex parentVtx = vertices[vtx.GroupID - 1];
            if (vtx.GroupID == vtx.ID || parentVtx.GroupID == parentVtx.ID) return vtx.GroupID;
            while(parentVtx.GroupID != parentVtx.ID)
            {
                parentVtx = vertices[parentVtx.GroupID - 1];
            }
            int rootID = parentVtx.GroupID;
            parentVtx = vtx;
            while (parentVtx.GroupID != parentVtx.ID)
            {
                Vertex tmp = parentVtx;
                parentVtx = vertices[parentVtx.GroupID];
                tmp.GroupID = rootID;
            }
                
            return rootID;
        }
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
