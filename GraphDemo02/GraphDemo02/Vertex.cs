using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphDemo02
{
    class Vertex
    {
        public int ID { get; set; }
        public List<Vertex> Edges { get; set; }
        public Vertex(int id)
        {
            ID = id;
            Edges = new List<Vertex>();
        }

        public void AddEdge(Vertex vertex)
        {
            Edges.Add(vertex);

        }

        public override string ToString()
        {
            string result = "";
            foreach(Vertex vertex in Edges)
            {
                result = result + vertex.ID + " ";
            }
            result = result.Trim();
            return result;
        }
    }
}
