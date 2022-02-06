using System;
namespace TahsinsLibrary.Collections
{
    public struct Frequency<T>
    {
        public T item { get; private set; }
        public int min { get; private set; }
        public int max { get; private set; }
        public int decised { get; private set; }
        public void DeciseCount(int seed)
        {
            Random random = new Random(seed);
            decised = random.Next(min, max);
        }
        public Frequency(T item, int min, int max)
        {
            this.item = item;
            if (max > min)
            {
                this.max = max;
                this.min = min;
            }
            else
            {
                this.max = min;
                this.min = max;
            }
            decised = this.min;
        }
        public Frequency(T item, int min, int max, int seed)
        {
            this.item = item;
            if (max > min)
            {
                this.max = max;
                this.min = min;
            }
            else
            {
                this.max = min;
                this.min = max;
            }
            decised = this.min;
            DeciseCount(seed);
        }
    }
}