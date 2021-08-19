using System.Collections.Generic;

namespace GuitarChordFinder
{
    public class Quality
    {
        public List<int> Formula { get; private set; }
        public string Name { get; private set; }

        public Quality()
        {
            Formula = new List<int> { 0, 4, 7 };
        }

        public bool Contains(int n)
        {
            return Formula.Contains(n);
        }

        public bool Add(int n)
        {
            if (Contains(n))
                return false;
            Formula.Add(n);
            return true;
        }

        public void Remove(int n)
        {
            Formula.Remove(n);
        }

        public void RemoveThird()
        {
            Formula.RemoveAt(1);
        }

        public void AddMask(List<int> mask)
        {
            int i = 0;
            foreach (int m in mask)
                Formula[i++] += m;
        }
    }
}