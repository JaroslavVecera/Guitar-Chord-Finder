using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarChordFinder
{
    public class Pattern
    {
        public int[] Frets { get; private set; }
        public int Unmuted { get { return Frets.Count(x => x != -1); } }
        public int Max { get { return Frets.Max(); } }
        public int Min { get { return GetMin(); } }
        public bool Barre { get; set; } = false;

        public Pattern(int[] frets)
        {
            Frets = frets;
        }

        int GetMin()
        {
            int min = Max;
            foreach (int pos in Frets)
                if (pos > 0 && pos < min)
                    min = pos;
            return min;
        }

        public override string ToString()
        {
            string res = "{ " + FretToString(Frets[0]);
            for (int i = 1; i < 6; i++)
                res += ", " + FretToString(Frets[i]);
            return res + " }";
        }

        string FretToString(int fret)
        {
            return fret == -1 ? "x" : fret.ToString();
        }
    }
}
