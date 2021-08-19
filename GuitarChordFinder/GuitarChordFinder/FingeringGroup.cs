using System;
using System.Collections.Generic;
using System.Text;

namespace GuitarChordFinder
{
    public class FingeringGroup
    {
        public Fingering Full { get; private set; }
        public List<Fingering> Subsets { get; } = new List<Fingering>();
        public bool Any { get { return Subsets.Count > 0; } }

        public FingeringGroup(Fingering full)
        {
            Full = full;
        }

        public void Add(Fingering f)
        {
            Subsets.Add(f);
        }

        public bool IsSubset(Pattern p)
        {
            var fullFrets = Full.Pattern.Frets;
            var potSubFrets = p.Frets;
            for (int i = 0; i < fullFrets.Length; i++)
            {
                if (potSubFrets[i] > -1 && fullFrets[i] == potSubFrets[i])
                    return false;
            }
            return true;
        }
    }
}
