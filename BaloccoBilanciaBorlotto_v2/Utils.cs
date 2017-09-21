using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaloccoBilanciaBorlotto_v2
{
    public enum Tire
    {
        ANT_SX = 0,
        ANT_DX = 1,
        POST_SX = 2,
        POST_DX = 3
    }

    public class FifoList : LinkedList<double>
    {
        public int MaxSize { get { return MaxSize; } set { MaxSize = value; while (Count > MaxSize) RemoveLast(); } }
        public bool IsFull { get { return MaxSize == Count; } }
        public double Average { get { return Math.Round(Enumerable.Average(this), 1); } }

        public FifoList(int maxSize)
        {
            this.MaxSize = maxSize;
        }

        public void Insert(double item)
        {
            AddFirst(item);
            while (Count > MaxSize)
                RemoveLast();
        }
    }
}
