#define LOCALX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Freckles
{
    class Program
    {
        static List<List<Vertex>> caseVtx = new List<List<Vertex>>();
        static List<List<Edge>> caseEdge = new List<List<Edge>>();
        static void Main(string[] args)
        {

            GetData();
#if LOCAL
            Stopwatch watch = new Stopwatch();
            watch.Start();
#endif
            for (int caseIndex = 0; caseIndex < caseVtx.Count; caseIndex++)
            {
                caseEdge.Add(new List<Edge>());
                for(int m = 0; m < caseVtx[caseIndex].Count - 1; m++)
                {
                    for(int n=m+1; n<caseVtx[caseIndex].Count; n++)
                    {
                        caseEdge[caseIndex].Add(new Edge(caseVtx[caseIndex][m], caseVtx[caseIndex][n], 
                            Math.Sqrt(
                                Math.Pow(caseVtx[caseIndex][m].CoordinateX - caseVtx[caseIndex][n].CoordinateX, 2) +
                                Math.Pow(caseVtx[caseIndex][m].CoordinateY - caseVtx[caseIndex][n].CoordinateY, 2)
                                )));
                    }
                }

                // minimum spanning tree
                caseEdge[caseIndex].Sort();
                List<Edge> selectedEdges = new List<Edge>();
                foreach(Edge edge in caseEdge[caseIndex])
                {
                    int leftGroupID = GetGroupID(caseVtx[caseIndex], edge.VtxOne);
                    int rightGroupID = GetGroupID(caseVtx[caseIndex], edge.VtxTwo);
                    if (leftGroupID == rightGroupID) continue;
                    if(edge.VtxTwo.ID != rightGroupID) // it is not the root
                    {
                        caseVtx[caseIndex][rightGroupID].GroupID = leftGroupID;
                    }
                    edge.VtxTwo.GroupID = leftGroupID;
                    selectedEdges.Add(edge);
                    if (selectedEdges.Count == caseVtx[caseIndex].Count - 1) break;
                    // Console.WriteLine(string.Format("{0}->{1} : {2}", edge.VtxOne.ID, edge.VtxTwo.ID, edge.Distance));
                }

                // print result
                double result = 0.0;
                foreach(Edge edge in selectedEdges)
                {
                    result += edge.Distance;
                }

                Console.WriteLine(string.Format("{0:0.00}", result));
                Console.WriteLine("");
            }
#if LOCAL
            watch.Stop();
            TimeSpan ts = watch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
#endif
        }
        static int GetGroupID(List<Vertex> vertices, Vertex vtx)
        {
            Vertex parentVtx = vertices[vtx.GroupID];
            if (vtx.GroupID == vtx.ID || parentVtx.GroupID == parentVtx.ID) return vtx.GroupID;
            while (parentVtx.GroupID != parentVtx.ID)
            {
                parentVtx = vertices[parentVtx.GroupID];
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
        public int GroupID { get; set; }
        public double CoordinateX { get; set; }
        public double CoordinateY { get; set; }
        
        public Vertex(int id, double x, double y)
        {
            ID = id;
            GroupID = id;
            CoordinateX = x;
            CoordinateY = y;
        }
        public string Diag()
        {
            return string.Format("{0} : {1:0.00} {2:0.00}", ID, CoordinateX, CoordinateY);
            
        }
    } // Vertex
    class Edge : IComparable<Edge>
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

        public int CompareTo(Edge other)
        {
            return this.Distance.CompareTo(other.Distance);
        }
    }
}
