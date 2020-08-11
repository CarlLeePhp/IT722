#define LOCALX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace ComparingPriorityQueues
{
    class Program
    {
        Stopwatch stopwatch = new Stopwatch();
        
        static Queue<int> startPoints = new Queue<int>();
        static Queue<int> endPoints = new Queue<int>();
        static void Main(string[] args)
        {
            List<List<Vertex>> cases = GetData();
            for (int i=0; i < cases.Count; i++)
            {
                FindShortestPath(cases[i], i, startPoints.Dequeue(), endPoints.Dequeue());
            }

            Console.ReadLine();
        } // Main
        static void FindShortestPath(List<Vertex> vertices, int caseIndex, int startPoint, int endPoint)
        {
            // PriorityQueue<Vertex> starting = new PriorityQueue<Vertex>();
            SortedQueue<Vertex> starting = new SortedQueue<Vertex>();
            starting.Enqueue(vertices[startPoint]);
            vertices[startPoint].Key = 0;
            vertices[startPoint].Discovered = true;

            while (starting.Count > 0)
            {
                Vertex currentVtx = starting.Dequeue();
                foreach (Edge edge in currentVtx.Adjacents)
                {
                    if (!edge.Dest.Discovered)
                    {
                        starting.Enqueue(edge.Dest);
                        edge.Dest.Discovered = true;
                    }
                    if (edge.Dest.Key <= (currentVtx.Key + edge.Weight)) continue;
                    if (!edge.Dest.Visted)
                    {
                        edge.Dest.Key = (currentVtx.Key + edge.Weight);
                        edge.Dest.Parent = currentVtx;
                        starting.UpdateTree();
                    }
                }
                currentVtx.Visted = true;
            }

            if (vertices[endPoint].Key == int.MaxValue)
            {
                Console.WriteLine("No path");
            }
            else
            {
                Console.WriteLine("Case {0}: {1}",caseIndex, vertices[endPoint].Key);
            }

        }// find shortest path
        static List<List<Vertex>> GetData()
        {
#if LOCAL
            TextReader stdin = Console.In;
            Console.SetIn(new StreamReader("graph.txt"));
#endif
            List<List<Vertex>> cases = new List<List<Vertex>>();
            List<Vertex> vertices;
            string line = Console.ReadLine().Trim();
            int caseNum = int.Parse(line);
            int vtxNum = 0;
            int edgeNum = 0;
            string[] parts;
            for (int i = 0; i < caseNum; i++)
            {
                line = Console.ReadLine().Trim();
                parts = line.Split(' ');
                vtxNum = int.Parse(parts[0]);
                edgeNum = int.Parse(parts[1]);
                line = Console.ReadLine().Trim();
                parts = line.Split(' ');
                startPoints.Enqueue(int.Parse(parts[0]));
                endPoints.Enqueue(int.Parse(parts[1]));

                vertices = new List<Vertex>();
                for (int j = 0; j < vtxNum; j++)
                {
                    vertices.Add(new Vertex(j));
                }
                for (int j = 0; j < edgeNum; j++)
                {
                    line = Console.ReadLine().Trim();
                    parts = line.Split(' ');
                    int n1 = int.Parse(parts[0]);
                    int n2 = int.Parse(parts[1]);
                    int n3 = int.Parse(parts[2]);
                    vertices[n1].Adjacents.Add(new Edge(vertices[n2], n3));
                    vertices[n2].Adjacents.Add(new Edge(vertices[n1], n3));
                }
                cases.Add(vertices);
                
            }

#if LOCAL
            Console.SetIn(stdin);
#endif
            return cases;
        } // Get Data
    } // class program
    class Vertex : IComparable<Vertex>
    {
        public int Id { get; set; }
        public List<Edge> Adjacents { get; set; }
        public Vertex Parent { get; set; }
        public int Key { get; set; }
        public bool Discovered { get; set; }
        public bool Visted { get; set; }

        public Vertex(int id)
        {
            Id = id;
            Discovered = false;
            Visted = false;
            Adjacents = new List<Edge>();
            Key = int.MaxValue;
        }

        public string Diag()
        {
            string result = "";
            if (Parent != null)
            {
                result = string.Format("{0}|{1}|{2}:", Id, Parent.Id, Key);
            }
            else
            {
                result = string.Format("{0}|{1}|{2}:", Id, "null", Key);
            }
            foreach (Edge nb in Adjacents)
            {
                result += string.Format(" {0}", nb);
            }
            return result;
        } // Diag

        public int CompareTo(Vertex vtx)
        {
            return this.Key - vtx.Key;
        }
    } // Class Vertex

    class Edge
    {
        public Vertex Dest { get; set; }
        public int Weight { get; set; }
        public Edge(Vertex dst, int wt)
        {
            Dest = dst;
            Weight = wt;
        }
        public override string ToString()
        {
            return string.Format("{0}({1})", Dest.Id, Weight);
        }
    } // Class Edge
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> _data;
        public PriorityQueue()
        {
            _data = new List<T>();
        }
        public int Count
        {
            get
            {
                return _data.Count;
            }
        }
        // Enqueue
        public void Enqueue(T item)
        {
            // 1. add it at the end
            // 2. compare with its parent
            //    if it is smaller then its parent -> swap
            // 3. repeat step 2
            //    until its parent is smaller or it at position 0
            _data.Add(item);
            int currentIndex = _data.Count - 1;
            int parentIndex = (currentIndex - 1) / 2;
            while (currentIndex > 0)
            {
                if (_data[parentIndex].CompareTo(_data[currentIndex]) <= 0) break;
                Swap(currentIndex, parentIndex);
                currentIndex = parentIndex;
                parentIndex = (currentIndex - 1) / 2;
            }

        }

        public T Dequeue()
        {
            T result = _data[0];
            _data[0] = _data[_data.Count - 1];
            _data.RemoveAt(_data.Count - 1);
            int currentIndex = 0;
            int childLeft;
            int childRight;
            int childMin;
            while (currentIndex < _data.Count)
            {
                childLeft = currentIndex * 2 + 1;
                childRight = currentIndex * 2 + 2;
                childMin = childLeft; // no right child or R >= L
                if (childLeft >= _data.Count) break; // no child
                if (childRight < _data.Count && _data[childRight].CompareTo(_data[childLeft]) < 0) childMin = childRight; // has right child && R < L
                if (_data[currentIndex].CompareTo(_data[childMin]) <= 0) break;
                Swap(currentIndex, childMin);
                currentIndex = childMin;
            }

            return result;
        }
        // update the tree
        public void UpdateTree()
        {
            for (int i = _data.Count / 2; i >= 0; i--)
            {
                SwipDown(i);
            }
        }
        // swip down
        private void SwipDown(int index)
        {
            int childLeft = index * 2 + 1;
            int childRight = index * 2 + 1;
            int childMin = childLeft;
            if (childLeft > _data.Count - 1) return;
            if (childRight < _data.Count && _data[childRight].CompareTo(_data[childLeft]) < 0) childMin = childRight;
            if (_data[index].CompareTo(_data[childMin]) <= 0) return;
            Swap(index, childMin);
            SwipDown(childMin);
        }
        // swap two items
        private void Swap(int index1, int index2)
        {
            if (index1 > (_data.Count - 1) || index2 > (_data.Count - 1)) return;
            T tmp = _data[index1];
            _data[index1] = _data[index2];
            _data[index2] = tmp;
        }
    } // Class PriorityQueue
    public class SortedQueue<T> where T : IComparable<T>
    {
        private List<T> _data;
        public SortedQueue()
        {
            _data = new List<T>();
        }
        public int Count
        {
            get
            {
                return _data.Count;
            }
        }
        // Enqueue
        public void Enqueue(T item)
        {
            _data.Add(item);
            UpdateTree();
        }

        public T Dequeue()
        {
            T result = _data[0];
            _data.RemoveAt(0);
            return result;
        }
        // update the tree
        public void UpdateTree()
        {
            _data.Sort();
        }
        
    } // Class SortedQueue
}
