#define LOCALX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MagicalShuttleJourney
{
    class Program
    {
        static int startPoint;
        static int endPoint;
        static void Main(string[] args)
        {
            List<Vertex> vertices = GetData();
            PriorityQueue starting = new PriorityQueue();
            starting.Enqueue(vertices[startPoint]);
            vertices[startPoint].IsDiscovered = true;
            vertices[startPoint].Key = 0;
            while (starting.Count > 0)
            {
                Vertex currentVtx = starting.Dequeue();
                if (currentVtx.IsVisited) continue; // It could be pushed again
                currentVtx.IsVisited = true;
                currentVtx.Adjacencies.Sort();
                foreach(Edge edge in currentVtx.Adjacencies)
                {
                    if (edge.Destination.IsVisited) continue;
                    if (!edge.Destination.IsDiscovered)
                    {
                        edge.Destination.IsDiscovered = true;
                        edge.Destination.Key = currentVtx.Key + edge.Weight;
                        edge.Destination.Predecessor = currentVtx;
                        starting.Enqueue(edge.Destination);
                    }
                    else if (edge.Destination.Key > currentVtx.Key + edge.Weight)
                    {
                        edge.Destination.Key = currentVtx.Key + edge.Weight;
                        edge.Destination.Predecessor = currentVtx;
                        starting.Enqueue(edge.Destination);
                    }
                }
            }
            foreach (Vertex vtx in vertices)
            {
                Console.WriteLine(vtx.Diag());
            }
            Console.WriteLine(string.Format("{0} seconds",vertices[endPoint].Key));

            string shortestPath = endPoint.ToString();
            Vertex preVtx = vertices[endPoint].Predecessor;
            while(preVtx != null)
            {
                shortestPath = string.Format("{0} ", preVtx.ID) + shortestPath;
                preVtx = preVtx.Predecessor;
            }
            Console.WriteLine(shortestPath);
            
#if LOCAL
            Console.ReadLine();
#endif

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
            for(int i=0; i<vtxNum; i++)
            {
                vertices.Add(new Vertex(i));
            }
            for(int i=0; i < edgeNum; i++)
            {
                line = Console.ReadLine().Trim();
                parts = line.Split(' ');
                int v1 = int.Parse(parts[0]);
                int v2 = int.Parse(parts[1]);
                int weight = int.Parse(parts[2]);
                vertices[v1].Adjacencies.Add(new Edge(vertices[v2], weight));
                vertices[v2].Adjacencies.Add(new Edge(vertices[v1], weight));
            }
            line = Console.ReadLine();
            parts = line.Split(' ');
            startPoint = int.Parse(parts[0]);
            endPoint = int.Parse(parts[1]);
#if LOCAL
            Console.SetIn(stdin);
#endif
            return vertices;
        }// Get Data
    } // Class Program
    class Vertex
    {
        public int ID { get; set; }
        public Vertex Predecessor { get; set; }
        public List<Edge> Adjacencies { get; set; }
        public int Key { get; set; }
        public bool IsVisited { get; set; }
        public bool IsDiscovered { get; set; }
        public Vertex(int id)
        {
            ID = id;
            Predecessor = null;
            Adjacencies = new List<Edge>();
            Key = int.MaxValue;
            IsVisited = false;
            IsDiscovered = false;
        }

        public string Diag()
        {
            string result = string.Format("{0}", ID);
            if(Predecessor == null)
            {
                result += string.Format(" (-:{0}):", Key);
            }
            else
            {
                result += string.Format(" ({0}:{1}):",Predecessor.ID, Key);
            }
            foreach(Edge edge in Adjacencies)
            {
                result += string.Format(" {0}({1})", edge.Destination.ID, edge.Weight);
            }
            return result;
        }
    } // Class Vertex
    class Edge:IComparable<Edge>
    {
        public Vertex Destination { get; set; }
        public int Weight { get; set; }

        public Edge(Vertex destination, int weight)
        {
            Destination = destination;
            Weight = weight;
        }

        public int CompareTo(Edge other)
        {
            return Destination.ID - other.Destination.ID;
        }
    }

    class PriorityQueue
    {
        List<Vertex> _data;
        public PriorityQueue()
        {
            _data = new List<Vertex>();
        }
        public int Count
        {
            get { return _data.Count; }
        }
        public void Enqueue(Vertex vtx)
        {
            _data.Add(vtx);
            int ci = _data.Count - 1;
            int pi = (ci - 1) / 2;
            while(ci > 0 && _data[ci].Key < _data[pi].Key)
            {
                Swap(ci, pi);
                ci = pi;
                pi = (ci - 1) / 2;
            }
        }
        public Vertex Dequeue()
        {
            Vertex returnVtx = _data[0];
            _data[0] = _data[_data.Count - 1];
            _data.RemoveAt(_data.Count - 1);
            int pi = 0;
            
            while(pi < _data.Count / 2)
            {
                int ci = pi * 2 + 1; // min one, default is left one
                int cr = pi * 2 + 2;
                if (ci > _data.Count - 1) break; // no child
                if (cr < _data.Count && _data[cr].ID < _data[ci].ID) ci = cr;
                if (_data[pi].ID < _data[ci].ID) break;
                Swap(pi, ci);
            }
            return returnVtx;
        }

        private void Swap(int indexA, int indexB)
        {
            Vertex tmp = _data[indexA];
            _data[indexA] = _data[indexB];
            _data[indexB] = tmp;
        }
    }

    class PriorityQueueLoop
    {
        List<Vertex> _data;
        public PriorityQueueLoop()
        {
            _data = new List<Vertex>();
        }
        public int Count
        {
            get { return _data.Count; }
        }
        public void Enqueue(Vertex vtx)
        {
            _data.Add(vtx);
        }
        public Vertex Dequeue()
        {
            int result = 0;
            for (int i = 1; i < _data.Count; i++)
            {
                if (_data[i].Key < _data[result].Key) result = i;
            }
            Vertex minVertex = _data[result];
            _data.RemoveAt(result);
            return minVertex;
        }
    }
}
