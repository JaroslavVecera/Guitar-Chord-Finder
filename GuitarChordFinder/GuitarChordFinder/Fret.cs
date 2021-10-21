using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarChordFinder
{
    class Fret
    {
        public FingeringOptions Options { get; private set; }

        ChordParser Parser { get; } = new ChordParser();

        public Fret(FingeringOptions opt)
        {
            Options = opt;
        }
         
        public Tuple<List<Pattern>, string> ListPatterns(string symbol)
        {
            Chord chord = Parser.Parse(symbol);
            if (chord == null)
                return null;
            List<int>[] allPositions = new List<int>[Options.Tuning.Length];

            for (int i = 0; i < Options.Tuning.Length; i++)
            {
                allPositions[i] = new List<int>() { -1 };
                for (int pos = 0; pos <= Options.MaxFret; pos++)
                    if (chord.ConcreteFormula.Contains((pos + Options.Tuning[i]) % 12))
                        allPositions[i].Add(pos);
            }
            return new Tuple<List<Pattern>, string>(GeneratePatterns(chord, allPositions), chord.GetNotes());
        }

        List<Pattern> GeneratePatterns(Chord c, List<int>[] positions)
        { 
            List<Pattern> res = new List<Pattern>();
            GeneratePatternsRec(-1, -1, new List<int>(), positions, res);
            res.RemoveAll(pattern => !ProperChordPattern(c, pattern));
            res.Sort(new Comparison<Pattern>(ComparePatterns));
            return res;
        }

        int ComparePatterns(Pattern p1, Pattern p2)
        {
            if (p1.Min < p2.Min)
                return -1;
            else if (p1.Min > p2.Min)
                return 1;
            else if (p1.Unmuted > p2.Unmuted)
                return -1;
            else return 1;
        }

        void GeneratePatternsRec(int min, int max, List<int> pattern, List<int>[] positions, List<Pattern> result)
        {
            int newmin = min, newmax = max;
            if (pattern.Count == Options.Tuning.Length)
                result.Add(new Pattern(pattern.ToArray()));
            else
            {
                foreach (int p in positions[pattern.Count])
                {
                    if (p > 0)
                    {
                        if (min == -1)
                            newmax = newmin = p;
                        else
                        {
                            newmin = Math.Min(min, p);
                            newmax = Math.Max(max, p);
                        }
                        if (Math.Abs(newmin - newmax) > Options.FretRange)
                            continue;
                    }
                    pattern.Add(p);
                    GeneratePatternsRec(newmin, newmax, pattern, positions, result);
                    pattern.RemoveAt(pattern.Count - 1);
                }
            }
        }

        bool ProperChordPattern(Chord c, Pattern p)
        {
            return p.Unmuted > 2 && IsInRange(p)  && ContainsAllTones(c, p) && IsUniform(p) && IsRootLowest(c.Lowest, p) && SetFingersAreEnaught(p);
        }

        bool ContainsAllTones(Chord c, Pattern p)
        {
            return c.ConcreteFormula.All(tone => PatternToNotes(p).Select(x => x % 12).Contains(tone));
        }

        bool IsInRange(Pattern p)
        {
            return p.Max - p.Min + 1 <= Options.FretRange;
        }

        bool IsUniform(Pattern p)
        {
            int state = 0;
            for(int i = 0; i < Options.Tuning.Length; i++)
                if ((state % 2 == 0 && p.Frets[i] > -1) || (state % 2 == 1 && p.Frets[i] == -1))
                    state++;
            return state < 3;
        }

        bool IsRootLowest(int desiredLowest, Pattern p)
        {
            int lowestNote = PatternToNotes(p).Min() % 12;
            return lowestNote == desiredLowest;
        }

        bool SetFingersAreEnaught(Pattern p)
        {
            int count = p.Frets.Count(f => f > 0);
            if (count <= Options.RequiredFingers)
            {
                if (count >= 4 && SetFingersAreEnaughtWithBarre(p) && p.Frets.Count(f => f == p.Min) > 1)
                    p.Barre = true;
                return true;
            }
            p.Barre = true;
            return SetFingersAreEnaughtWithBarre(p);
        }

        bool SetFingersAreEnaughtWithBarre(Pattern p)
        {
            int min = p.Min;
            int barreIndex = p.Frets.ToList().IndexOf(min);
            return !p.Frets.ToList().GetRange(barreIndex, p.Frets.Count() - barreIndex).Contains(0) && (p.Frets.Count(f => f > min) + 1 <= Options.RequiredFingers);
        }

        List<int> PatternToNotes(Pattern p)
        {
            List<int> notes = new List<int>();
            for (int i = 0; i < p.Frets.Count(); i++)
                if (p.Frets[i] > -1)
                    notes.Add(p.Frets[i] + Options.Tuning[i]);
            return notes;
        }
    }
}
