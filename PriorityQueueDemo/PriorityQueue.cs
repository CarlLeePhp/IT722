using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorityQueueDemo
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> _data;

        public PriorityQueue()
        {
            _data = new List<T>();
        }
        public void Enqueue(T item)
        {
            _data.Add(item);
            int currentPosition = _data.Count - 1;
            while(currentPosition > 0)
            {
                int parent = (currentPosition - 1) / 2;
                if (_data[currentPosition].CompareTo(_data[parent]) >= 0) break;
                
                T tmp = _data[currentPosition];
                _data[currentPosition] = _data[parent];
                _data[parent] = tmp;
                currentPosition = parent;
            }
            
        } // Enqueue

        public T Dequeue()
        {
            T result = _data[0];
            _data[0] = _data[_data.Count - 1];
            _data.RemoveAt(_data.Count - 1);
            int pi = 0;
            while(pi <= _data.Count / 2)
            {
                int cil = pi * 2 + 1;
                int cir = pi * 2 + 2;
                int ci = cil;
                if (cil > _data.Count - 1) break;
                if (cir > _data.Count - 1 && _data[pi].CompareTo(_data[cil]) <=0) break;
                if (_data[pi].CompareTo(_data[cil]) <= 0 && _data[pi].CompareTo(_data[cir]) <= 0) break;
                
                if (cir < _data.Count && _data[cil].CompareTo(_data[cir]) > 0) ci = cir;
                T tmp = _data[pi];
                _data[pi] = _data[ci];
                _data[ci] = tmp;
            }
            return result;
        } // Dequeue

        public override string ToString()
        {
            string result = "";
            foreach(T item in _data)
            {
                result += string.Format(" {0}", item.ToString());
            }
            return result;
        } // To String
    }
}
