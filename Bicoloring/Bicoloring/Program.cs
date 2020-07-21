using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Bicoloring
{
    class Program
    {
        static void Main(string[] args)
        {
            //TextReader stdin = Console.In;
            //Console.SetIn(new StreamReader("graph.txt"));
            string line = Console.ReadLine().Trim();
            while(line != "0")
            {
                int vtxNum = int.Parse(line);
                List<Vertex> vertices = GetMap(vtxNum);
                ColorMap(vertices);
                line = Console.ReadLine().Trim();
            }


            //Console.SetIn(stdin);
            //Console.ReadLine();
        } // Main
        static void ColorMap(List<Vertex> vertices)
        {
            Queue<Vertex> starting = new Queue<Vertex>();
            vertices[0].Discovered = true;
            vertices[0].IsBlack = true;
            Vertex currentVertex;
            starting.Enqueue(vertices[0]);
            bool result = true;
            while (starting.Count > 0)
            {
                
                currentVertex = starting.Dequeue();
                foreach(Vertex vtx in currentVertex.Neighbours)
                {
                    if (vtx.Discovered && vtx.IsBlack == currentVertex.IsBlack)  // conflict
                    {
                        result = false;
                        break;
                    }
                    else
                    {
                        vtx.Discovered = true;
                        vtx.IsBlack = !currentVertex.IsBlack;
                        if(!vtx.Processed) starting.Enqueue(vtx);
                    }
                }
                if (!result) break;
                currentVertex.Processed = true;
            }

            if (result)
            {
                Console.WriteLine("BICOLORABLE.");
            }
            else
            {
                Console.WriteLine("NOT BICOLORABLE.");
            }
        } // Color Map
        static List<Vertex> GetMap(int vtxNum)
        {
            List<Vertex> vertices = new List<Vertex>();
            for(int i=0; i < vtxNum;i++)
            {
                vertices.Add(new Vertex(i));
            }
            int lineNum = int.Parse(Console.ReadLine());
            for(int i=0; i<lineNum; i++)
            {
                string line = Console.ReadLine();
                string[] parts = line.Split(' ');
                int n1 = int.Parse(parts[0]);
                int n2 = int.Parse(parts[1]);
                vertices[n1].Neighbours.Add(vertices[n2]);
                vertices[n2].Neighbours.Add(vertices[n1]);
            }
            return vertices;
        } // Get Map
    } // Class Program

    class Vertex : IComparable<Vertex>
    {
        public int Id { get; set; }
        public List<Vertex> Neighbours { get; set; }
        public Vertex Parent { get; set; }

        public bool Discovered { get; set; }
        public bool Processed { get; set; }
        public bool IsBlack { get; set; }

        public Vertex(int id)
        {
            Id = id;
            Discovered = false;
            Processed = false;
            Neighbours = new List<Vertex>();
        }

        public void AddNeighbour(Vertex vtx)
        {
            Neighbours.Add(vtx);
            for (int i = Neighbours.Count - 2; i >= 0; i--)
            {
                if (Neighbours[i].Id <= vtx.Id) break;
                Neighbours[i + 1] = Neighbours[i];
                Neighbours[i] = vtx;
            }
        } // Add Neighbour
        public string Diag()
        {
            string result;
            if (Parent != null)
            {
                result = string.Format("{0} {1}:", Id, Parent.Id);
            }
            else
            {
                result = string.Format("{0} Null:", Id);
            }
            foreach (Vertex vtx in Neighbours)
            {
                result += string.Format(" {0}", vtx.Id);
            }
            return result;
        } // Diag

        public int CompareTo(Vertex other)
        {
            if (other == null) return 1;
            return Id.CompareTo(other.Id);
        } // Compare to
    } // Class Vertex
}
