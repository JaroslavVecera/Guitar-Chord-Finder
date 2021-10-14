using System;
using System.Collections.Generic;

namespace GuitarChordFinder
{
    public  class FingeringOptions
    {
        public int[] _openEGuitartuning = new int[] { 4, 9, 14, 19, 23, 28 };
        public int[] _ukuleleTuning = new int[] { 7, 0, 4, 9 };

        public int FretRange { get; set; }
        public int RequiredFingers { get; set; }
        public int MaxFret { get; set; }
        public int[] Tuning { get; set; } = new int[] { 28, 33, 38, 43, 47, 52 };

        public event Action OnOptionsChanged;

        public void NotifyChange()
        {
            OnOptionsChanged?.Invoke();
        }
    }
}