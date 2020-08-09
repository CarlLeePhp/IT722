#define LOCALX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShortestWeightedPath
{
    
    class Program
    {
        static int startPoint;
        static int endPoint;
        static void Main(string[] args)
        {
            List<Vertex> vertices = GetData();
            PriorityQueue<Vertex> starting = new PriorityQueue<Vertex>();
            starting.Enqueue(vertices[startPoint]);
            vertices[startPoint].Key = 0;
            vertices[startPoint].Discovered = true;

            while(starting.Count > 0)
            {
                Vertex currentVtx = starting.Dequeue();
                foreach(Edge edge in currentVtx.Adjacents)
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

            PrintResult(vertices);
            //foreach(Vertex vtx in vertices)
            //{
            //    Console.WriteLine(vtx.Diag());
            //}
            //Console.ReadLine();
        } // Main
        static void PrintResult(List<Vertex> vertices)
        {
            Stack<Vertex> results = new Stack<Vertex>();
            Vertex currentVtx = vertices[endPoint];
            while(currentVtx != vertices[startPoint])
            {
                results.Push(currentVtx);
                currentVtx = currentVtx.Parent;
            }
            string result = string.Format("{0}", vertices[startPoint].Id);
            
            while(results.Count > 0)
            {
                Vertex vtx = results.Pop();
                result += string.Format(" {0}", vtx.Id);
            }
            Console.WriteLine(result);
        } // Print Result
        static List<Vertex> GetData()
        {
#if LOCAL
            TextReader stdin = Console.In;
            Console.SetIn(new StreamReader("graph.txt"));
#endif
            List<Vertex> vertices = new List<Vertex>();

            string line = Console.ReadLine().Trim();
            int vtxNum = int.Parse(line);

            for (int i = 0; i < vtxNum; i++)
            {
                vertices.Add(new Vertex(i));
            }

            line = Console.ReadLine().Trim();
            string[] parts = line.Split(' ');
            startPoint = int.Parse(parts[0]);
            endPoint = int.Parse(parts[1]);

            while ((line = Console.ReadLine()) != null)
            {
                parts = line.Split(' ');
                int n1 = int.Parse(parts[0]);
                int n2 = int.Parse(parts[1]);
                int n3 = int.Parse(parts[2]);
                vertices[n1].Adjacents.Add(new Edge(vertices[n2], n3));
            }
#if LOCAL
            Console.SetIn(stdin);
#endif
            return vertices;
        } // Get Data
    } // Class Program
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
            for(int i=_data.Count / 2; i>=0; i--)
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
}
