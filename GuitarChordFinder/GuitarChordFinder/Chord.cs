using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarChordFinder
{
    public class Chord
    {
        public int Root { get; private set; }
        public Quality Quality { get; private set; }
        public int? Bass { get; private set; }

        public int Lowest { get { return Bass.HasValue ? Bass.Value : Root; } }
        static string[] Notes { get; } = new string[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        public int[] ConcreteFormula {
            get
            {
                List<int> res = new List<int>(Quality.Formula);
                if (Bass.HasValue && !Quality.Contains((((Bass.Value - Root) % 12) + 12) % 12))
                    res.Add((((Bass.Value - Root) % 12) + 12) % 12);
                return res.Select(x => (x + Root) % 12).ToArray(); } }

        public Chord(int root, Quality quality, int? bass)
        {
            Root = root;
            Quality = quality;
            Bass = bass;
        }

        public string GetNotes()
        {
            List<int> notes = ConcreteFormula.ToList();
            notes.Sort();
            string res = "";
            notes.ForEach(n => res += Notes[n] + " ");
            return res;
        }
    }
}
