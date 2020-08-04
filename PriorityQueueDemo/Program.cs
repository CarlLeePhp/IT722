using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorityQueueDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            PriorityQueue<int> numbers = new PriorityQueue<int>();
            numbers.Enqueue(6);
            numbers.Enqueue(8);
            numbers.Enqueue(9);
            numbers.Enqueue(4);
            Console.WriteLine(numbers);
            Console.WriteLine("Dequeue Test");
            numbers.Dequeue();
            Console.WriteLine(numbers);
            numbers.Dequeue();
            Console.WriteLine(numbers);
            Console.ReadLine();
        }
    }
}
