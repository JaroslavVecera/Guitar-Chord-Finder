using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarChordFinder
{
    public class ChordParser
    {
        ChordLexer Lexer { get; set; }
        Token CurrentToken { get; set; }
        bool IsEnd { get { return CurrentToken.Type == TokenType.end; } }
        bool CanSus { get; set; }
        int Root { get; set; }
        int? Bass { get; set; }
        List<TermType> Modifiers { get; set; } = new List<TermType>();
        Quality Quality { get; set; } = new Quality();

        static List<int> ExtendedChords { get; } = new List<int>() { 6, 7, 9, 11, 13 };
        static Dictionary<int, int> NthToSemitones { get; } = new Dictionary<int, int>() {
            { 2, 2}, { 9, 2 }, {3, 4 }, {4, 5 }, {11, 5 }, {5, 7 }, { 6, 9 }, { 13, 9 }, {7, 10 } };
        static ModFinder SeventhMods { get; } = ModFinder.Seventh();
        static ModFinder SimpleMods { get; } = ModFinder.Simple();
        static ModFinder SixthMods { get; } = ModFinder.Sixth();

        public Chord Parse(string symbol)
        {
            Initialize(symbol);
            bool succes = ParseRoot() && ParseQuality() && ParseBass();
            if (succes)
                return new Chord(Root, Quality, Bass);
            return null;
        }

        void Initialize(string symbol)
        { 
            Lexer = new ChordLexer(symbol);
            Bass = null;
            Modifiers.Clear();
            Quality = new Quality();
            CanSus = true;
        }

        bool ParseRoot()
        {
            if (!NextToken() || CurrentToken.Type != TokenType.note)
                return false;
            Root = ((Note)CurrentToken).Value;
            return true;
        }

        bool ParseQuality()
        {
            if (!NextToken())
                return false;
            if ((CurrentToken.Type == TokenType.mod || CurrentToken.Type == TokenType.term) && !ParseMods())
                return false;
            if (CurrentToken.Type == TokenType.num && ExtendedChords.Contains(((Number)CurrentToken).Value))
            {
                if (!ParseExtention())
                    return false;
            }
            else
            {
                if (Modifiers.Count == 2)
                    return false;
                if (CurrentToken.Type == TokenType.num && ((Number)CurrentToken).Value == 5)
                {
                    if (!ParseFifth())
                        return false;
                }
                else if(!ModifeSimpleChord())
                    return false;
            }
            if (CurrentToken.Type == TokenType.sus && !ParseSus())
                return false;
            if (!ParseAlts())
                return false;
            if (!ParseAdds())
                return false;
            return true;
        }

        bool ParseMods()
        {
            TermType t = ((Term)CurrentToken).TermType;
            if (t == TermType.plus)
                t = TermType.aug;
            else if (t == TermType.minus)
                t = TermType.min;
            Modifiers.Add(t);
            if (!NextToken())
                return false;
            if (CurrentToken.Type != TokenType.mod)
                return true;
            t = ((Term)CurrentToken).TermType;
            if (t == TermType.plus)
                t = TermType.aug;
            else if (t == TermType.minus)
                t = TermType.dim;
            Modifiers.Add(t);
            return NextToken();
        }

        bool ParseExtention()
        {
            int nth = ((Number)CurrentToken).Value;
            Quality.Add(NthToSemitones[nth == 6 ? 6 : 7]);
            if (nth > 7 && !Quality.Add(NthToSemitones[nth]))
                return false;
            Tuple<List<int>, bool> val = (nth != 6 ? SeventhMods[Modifiers] : SixthMods[Modifiers]);
            if (val == null)
                return false;
            Quality.AddMask(val.Item1);
            CanSus = val.Item2;
            int e = Math.Max(9, nth);
            if (!NextToken())
                return false;
            List<Token> alts = new List<Token>();
            while(CurrentToken.Type == TokenType.num || CurrentToken.Type == TokenType.term || CurrentToken.Type == TokenType.alt)
            {
                alts.Clear();
                if (CurrentToken.Type != TokenType.num)
                {
                    alts.Add(CurrentToken);
                    if (!NextToken() || !(CurrentToken.Type == TokenType.num || CurrentToken.Type == TokenType.term || CurrentToken.Type == TokenType.alt))
                        return false;
                }
                if (CurrentToken.Type != TokenType.num)
                {
                    if (CurrentToken.Type != alts[0].Type || ((Term)CurrentToken).TermType != ((Term)alts[0]).TermType)
                        return false;
                    alts.Add(CurrentToken);
                    if (!NextToken())
                        return false;
                }
                if (CurrentToken.Type != TokenType.num)
                    return false;
                nth = ((Number)CurrentToken).Value;
                int alt = alts.Count;
                if (alts.Any() && (((Term)alts[0]).TermType == TermType.minus || ((Term)alts[0]).TermType == TermType.flat))
                    alt *= -1;
                if (nth < e)
                {
                    CanSus = false;
                    return ParseAlts(alt);
                }
                e = nth;
                if (alt != 0 && Quality.Contains(NthToSemitones[nth]))
                    Quality.Remove(NthToSemitones[nth]);
                if (!Quality.Add((NthToSemitones[nth] + alt) % 12) || !NextToken())
                    return false;
            }
            return true;
        }

        bool ParseFifth()
        {
            Quality.RemoveThird();
            return NextToken();
        }

        bool ModifeSimpleChord()
        {
            Tuple<List<int>, bool> val = SimpleMods[Modifiers];
            if (val == null)
                return false;
            Quality.AddMask(val.Item1);
            CanSus = val.Item2;
            return true;
        }

        bool ParseSus()
        {
            if (!CanSus || !NextToken() || CurrentToken.Type != TokenType.num)
                return false;
            int sus = ((Number)CurrentToken).Value;
            if (sus == 2)
            {
                Quality.RemoveThird();
                if (!Quality.Add(NthToSemitones[3] - 2))
                    return false;
            }
            else if (sus == 4)
            {
                Quality.RemoveThird();
                if (!Quality.Add(NthToSemitones[3] + 1))
                    return false;
            }
            else
                return false;
            if (!NextToken())
                return false;
            if (CurrentToken.Type != TokenType.sus)
            {
                CanSus = false;
                return true;
            }
            if (!NextToken() || CurrentToken.Type != TokenType.num)
                return false;
            int sus2 = ((Number)CurrentToken).Value;
            if (sus == sus2)
                return false;
            if (sus2 == 2)
            {
                if (!Quality.Add(NthToSemitones[3] - 2))
                    return false;
            }
            else if (sus2 == 4)
            {
                if (!Quality.Add(NthToSemitones[3] + 1))
                    return false;
            }
            else
                return false;
            CanSus = false;
            return NextToken();
        }

        bool ParseAlts(int alt)
        {
            if (alt == 0)
                return false;
            int nth = ((Number)CurrentToken).Value;
            if (Quality.Contains(NthToSemitones[nth]))
                Quality.Remove(NthToSemitones[nth]);
            if (!NthToSemitones.ContainsKey(nth) || !Quality.Add((NthToSemitones[nth] + alt) % 12) || !NextToken())
                return false;
            return ParseAlts();
        }

        bool ParseAlts()
        {
            List<Token> alts = new List<Token>();
            while (CurrentToken.Type == TokenType.term || CurrentToken.Type == TokenType.alt)
            {
                alts.Add(CurrentToken);
                if (!NextToken() || !(CurrentToken.Type == TokenType.num || CurrentToken.Type == TokenType.term || CurrentToken.Type == TokenType.alt))
                    return false;
                if (CurrentToken.Type != TokenType.num)
                {
                    if (CurrentToken.Type != alts[0].Type || ((Term)CurrentToken).TermType != ((Term)alts[0]).TermType)
                        return false;
                    alts.Add(CurrentToken);
                    if (!NextToken())
                        return false;
                }
                if (CurrentToken.Type != TokenType.num)
                    return false;
                int nth = ((Number)CurrentToken).Value;
                int alt = alts.Count;
                if ((((Term)alts[0]).TermType == TermType.minus || ((Term)alts[0]).TermType == TermType.flat))
                    alt *= -1;
                if (Quality.Contains(NthToSemitones[nth]))
                    Quality.Remove(NthToSemitones[nth]);
                if (!NthToSemitones.ContainsKey(nth) || !Quality.Add((NthToSemitones[nth] + alt) % 12) || !NextToken())
                    return false;
            }
            return true;
        }

        bool ParseAdds()
        {
            List<Token> alts = new List<Token>();
            while (CurrentToken.Type == TokenType.add)
            {
                alts.Clear();
                bool add = ((Term)CurrentToken).TermType == TermType.add;
                if (!NextToken() || !(CurrentToken.Type == TokenType.num || CurrentToken.Type == TokenType.term || CurrentToken.Type == TokenType.alt))
                    return false;
                if (CurrentToken.Type != TokenType.num)
                {
                    alts.Add(CurrentToken);
                    if (!NextToken())
                        return false;
                }
                if (CurrentToken.Type != TokenType.num)
                {
                    if (CurrentToken.Type != alts[0].Type || ((Term)CurrentToken).TermType != ((Term)alts[0]).TermType)
                        return false;
                    alts.Add(CurrentToken);
                    if (!NextToken())
                        return false;
                }
                if (CurrentToken.Type != TokenType.num)
                    return false;
                int nth = ((Number)CurrentToken).Value;
                int alt = alts.Count;
                if (alts.Any() && (((Term)alts[0]).TermType == TermType.minus || ((Term)alts[0]).TermType == TermType.flat))
                    alt *= -1;
                if (add)
                {
                    if (!NthToSemitones.ContainsKey(nth) || !Quality.Add((NthToSemitones[nth] + alt) % 12) || !NextToken())
                        return false;
                }
                else
                {
                    if (!NthToSemitones.ContainsKey(nth) || !Quality.Contains((NthToSemitones[nth] + alt) % 12) || !NextToken())
                        return false;
                    Quality.Remove((NthToSemitones[nth] + alt) % 12);
                }
            }
            return true;
        }

        bool ParseBass()
        {
            if (CurrentToken.Type == TokenType.slash)
            {
                if (!NextToken() || CurrentToken.Type != TokenType.note)
                    return false;
                Bass = ((Note)CurrentToken).Value;
                if (!NextToken())
                    return false;
            }
            return CurrentToken.Type == TokenType.end;
        }

        bool NextToken()
        {
            CurrentToken = Lexer.NextToken();
            return CurrentToken != null;
        }
    }
}
