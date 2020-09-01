using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rdm = new Random();
            string line = Console.ReadLine();
            while(line != "done")
            {
                int num = rdm.Next(100);
                Console.WriteLine(num);
                line = Console.ReadLine();
            }
        }
    }
}
