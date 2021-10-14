using System;
using System.Collections.Generic;
using System.Text;

namespace GuitarChordFinder
{
    public class Tone
    {
        public int Octave { get; set; }
        public int Name { get; set; }

        List<string> Names { get; } = new List<string>() { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        public Tone(int name, int octave)
        {
            Name = name;
            Octave = octave;
        }

        public Tone() { }

        public Tone(int i)
        {
            Name = i % 12;
            Octave = i / 12;
        }

        public string OctaveToString()
        {
            return Octave.ToString();
        }

        public string NameToString()
        {
            return Names[Name];
        }

        public override string ToString()
        {
            return NameToString() + OctaveToString();
        }

        public int ToIndex()
        {
            return 12 * Octave + Name;
        }
    }
}
