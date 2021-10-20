using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace GuitarChordFinder
{
    public class FingeringOptions
    {
        public int[] _openEGuitartuning = new int[] { 4, 9, 14, 19, 23, 28 };
        public int[] _ukuleleTuning = new int[] { 7, 0, 4, 9 };

        public int DefaultFretRange { get; set; } = 4;
        public int DefaultRequiredFingers { get; set; } = 4;
        public int DefaultMaxFret { get; set; } = 20;
        public int[] DefaultTuning { get; set; } = new int[] { 28, 33, 38, 43, 47, 52 };

        public int FretRange { get; set; }
        public int RequiredFingers { get; set; }
        public int MaxFret { get; set; }
        public int[] Tuning { get; set; } = new int[] { 28, 33, 38, 43, 47, 52 };

        public event Action OnOptionsChanged;

        public FingeringOptions()
        {
            LoadPreferences();
        }

        public void NotifyChange()
        {
            OnOptionsChanged?.Invoke();
            SavePreferences();
        }

        public void LoadPreferences()
        {
            if (!Preferences.ContainsKey("exists"))
            {
                ApplyDefault();
                return;
            }
            FretRange = Preferences.Get("FretRange", 4);
            RequiredFingers = Preferences.Get("RequiredFingers", 4);
            MaxFret = Preferences.Get("MaxFret", 20);

            string tuning = Preferences.Get("Tuning", "28,33,38,43,47,52");
            Tuning = tuning.Split(',').ToList().Select(str => int.Parse(str)).ToArray();
        }

        public void SavePreferences()
        {
            Preferences.Set("exists", true);
            Preferences.Set("FretRange", FretRange);
            Preferences.Set("RequiredFingers", RequiredFingers);
            Preferences.Set("MaxFret", MaxFret);

            string tuning = string.Join(",", Tuning.ToList().Select(i => i.ToString()));
            Preferences.Set("Tuning", tuning);
        }

        public void ResetPreferences()
        {
            ApplyDefault();
            NotifyChange();
        }

        void ApplyDefault()
        { 
            FretRange = DefaultFretRange;
            MaxFret = DefaultMaxFret;
            Tuning = DefaultTuning;
            RequiredFingers = DefaultRequiredFingers;
        }
    }
}