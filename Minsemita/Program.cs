#define LOCALX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Minsemita
{
    class Program
    {
        static List<List<Vertex>> caseVtx = new List<List<Vertex>>();
        static void Main(string[] args)
        {
            GetData();
            foreach(List<Vertex> vertices in caseVtx)
            {
                FindShortestPath(vertices);
            }
#if LOCAL
            Console.ReadLine();
#endif
        } // Main
        static void FindShortestPath(List<Vertex> vertices)
        {
            PriorityQueue starting = new PriorityQueue();
            starting.Enqueue(vertices[0]);
            vertices[0].IsDiscovered = true;
            vertices[0].Key = 0;
            while(starting.Count > 0)
            {
                Vertex currentVtx = starting.Dequeue();
                if (currentVtx.ID == 42) break;
                if (currentVtx.IsVisited) continue;
                foreach(Edge edge in currentVtx.Transitions)
                {
                    if (edge.EndVtx.IsVisited) continue;
                    if(edge.EndVtx.Key > currentVtx.Key + edge.Weight)
                    {
                        edge.EndVtx.IsDiscovered = true;
                        edge.EndVtx.Parent = currentVtx;
                        edge.EndVtx.Key = currentVtx.Key + edge.Weight;
                        starting.Enqueue(edge.EndVtx);
                    }
                }
                currentVtx.IsVisited = true;
            }
            Console.WriteLine(string.Format("{0} tokens", vertices[42].Key));
            string result = "42";
            Vertex lastVtx = vertices[42];
            while(lastVtx.Parent != null)
            {
                lastVtx = lastVtx.Parent;
                result = string.Format("{0}->", lastVtx.ID) + result;
            }
            Console.WriteLine(result);
        }
        static void GetData()
        {
#if LOCAL
            TextReader stdin = Console.In;
            Console.SetIn(new StreamReader("graph.txt"));
#endif
            string line = Console.ReadLine().Trim();
            int transitionNum;
            string[] parts;
            List<Vertex> vertices;
            while(line != "0")
            {
                transitionNum = int.Parse(line);
                vertices = new List<Vertex>();
                for(int i=0; i<= 42; i++)
                {
                    vertices.Add(new Vertex(i));
                }
                for(int i=0; i < transitionNum; i++)
                {
                    line = Console.ReadLine().Trim();
                    parts = line.Split(' ');
                    int v1 = int.Parse(parts[0]);
                    int v2 = int.Parse(parts[1]);
                    int weight = int.Parse(parts[2]);
                    vertices[v1].Transitions.Add(new Edge(vertices[v2], weight));
                }
                caseVtx.Add(vertices);
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
        public bool IsDiscovered { get; set; }
        public bool IsVisited { get; set; }
        public int Key { get; set; }
        public List<Edge> Transitions { get; set; }
        public Vertex Parent { get; set; }
        public Vertex(int id)
        {
            ID = id;
            IsDiscovered = false;
            IsVisited = false;
            Key = int.MaxValue;
            Parent = null;
            Transitions = new List<Edge>();
        }
        public string Diag()
        {
            string result;
            if(Parent == null)
            {
                result = string.Format("{0} null:", ID);
            }
            else
            {
                result = string.Format("{0} {1}:", ID, Parent.ID);
            }
            foreach(Edge edge in Transitions)
            {
                result += string.Format(" {0}({1})", edge.EndVtx.ID, edge.Weight);
            }
            return result;
        }
    } // Vertex

    class Edge
    {
        
        public Vertex EndVtx { get; set; }
        public int Weight { get; set; }
        public Edge(Vertex endVtx, int weight)
        {
            EndVtx = endVtx;
            Weight = weight;
        }
    } // Edge

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
            while (ci > 0 && _data[ci].Key < _data[pi].Key)
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

            while (pi < _data.Count / 2)
            {
                int ci = pi * 2 + 1; // min one, default is left one
                int cr = pi * 2 + 2;
                if (ci > _data.Count - 1) break; // no child
                if (cr < _data.Count && _data[cr].ID < _data[ci].ID) ci = cr;
                if (_data[pi].Key <= _data[ci].Key) break;
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
}
