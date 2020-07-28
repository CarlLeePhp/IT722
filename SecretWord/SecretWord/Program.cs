using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SecretWord
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Vertex> vertices = GetData();
            int currentTeam = 1;
            Console.WriteLine(string.Format("There are {0} vertices", vertices.Count));
            foreach(Vertex vtx in vertices)
            {
                Console.WriteLine(vtx.Diag());
            }
            // loop from 0 to the end
            // if the vertex is discovered goto next one
            // print the team, currentTeam++, goto next one
            string result = "";
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].Discovered) continue;
                result += GetLetter(i, vertices, currentTeam);
                currentTeam++;
            }

            printAnagram("", result);
            
            Console.ReadLine();
        } // Main

        
        static void printAnagram(string prefix, string word)
        {
            if(word.Length == 1)
            {
                Console.WriteLine(prefix + word);
            }
            else
            {
                for(int i = 0; i < word.Length; i++)
                {
                    string newPrefix = prefix + word[i];
                    String rest = word.Remove(i, 1);
                    printAnagram(newPrefix, rest);
                }
            }
        } // printAnagram
        static char GetLetter(int index, List<Vertex> vertices, int teamNum)
        {
            Queue<Vertex> starting = new Queue<Vertex>();
            List<Vertex> results = new List<Vertex>();
            starting.Enqueue(vertices[index]);
            vertices[index].Discovered = true;
            while (starting.Count > 0)
            {
                Vertex currentVtx = starting.Dequeue();
                results.Add(currentVtx);
                foreach (Vertex vtx in currentVtx.Neighbours)
                {
                    if (!vtx.Discovered)
                    {
                        starting.Enqueue(vtx);
                        vtx.Discovered = true;
                    }
                }
            }

            
            // a => 97
            
            return (char)(results.Count + 96);
        }
        static void PrintTeam(int index, List<Vertex> vertices, int teamNum)
        {
            Queue<Vertex> starting = new Queue<Vertex>();
            List<Vertex> results = new List<Vertex>();
            starting.Enqueue(vertices[index]);
            vertices[index].Discovered = true;
            while (starting.Count > 0)
            {
                Vertex currentVtx = starting.Dequeue();
                results.Add(currentVtx);
                foreach (Vertex vtx in currentVtx.Neighbours)
                {
                    if (!vtx.Discovered)
                    {
                        starting.Enqueue(vtx);
                        vtx.Discovered = true;
                    }
                }
            }

            results.Sort();
            string result = string.Format("{0}: ", teamNum);
            // a => 97
            result += string.Format("{0}", (char)(results.Count + 96)) ;
            
            Console.WriteLine(result);
        } // print team
        static List<Vertex> GetData()
        {
            TextReader stdin = Console.In;
            //Console.SetIn(new StreamReader("graph.txt"));
            Console.SetIn(new StreamReader("bones.txt"));

            // Create Vertices
            List<Vertex> vertices = new List<Vertex>();
            int vertexNum = int.Parse(Console.ReadLine());
            for (int i = 0; i < vertexNum; i++)
            {
                vertices.Add(new Vertex(i));
            }

            string line;
            // Build Graph
            while ((line = Console.ReadLine()) != null)
            {
                string[] parts = line.Split(' ');
                int n1 = int.Parse(parts[0]);
                int n2 = int.Parse(parts[1]);
                vertices[n1].Neighbours.Add(vertices[n2]);
                vertices[n2].Neighbours.Add(vertices[n1]);
            }

            Console.SetIn(stdin);
            return vertices;
        } // GetData
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
