using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GuitarChordFinder
{
    public class FingeringGroup : INotifyPropertyChanged
    {
        public Fingering Full { get; set; }
        public List<Fingering> Subsets { get; set; } = new List<Fingering>();
        public bool AnySubsets { get { return Subsets.Count > 1; } }

        public FingeringGroup(Fingering full)
        {
            Full = full;
            Add(Full);
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
                if (potSubFrets[i] > -1 && fullFrets[i] != potSubFrets[i])
                    return false;
            }
            return true;
        }
    }
}
