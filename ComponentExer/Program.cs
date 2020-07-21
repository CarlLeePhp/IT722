using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentExer
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    } // Class Program

    class Vertex : IComparable<Vertex>
    {
        public int Id { get; set; }
        public List<Vertex> Neighbours { get; set; }
        public Vertex Parent { get; set; }

        public bool Discovered { get; set; }

        public Vertex(int id)
        {
            Id = id;
            Discovered = false;
            Neighbours = new List<Vertex>();
        }

        public void AddNeighbour(Vertex vtx)
        {
            Neighbours.Add(vtx);
            for(int i=Neighbours.Count-2; i>=0; i--)
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
