using System.Collections.Generic;

namespace GuitarChordFinder
{
    public enum TermType
    {
        sharp,
        flat,
        add,
        omit,
        aug,
        dim,
        sus,
        min,
        maj,
        minus,
        plus
    }

    internal class Term : Token
    {
        public TermType TermType { get; private set; }
        static Dictionary<string, TermType> _stringToTermType = new Dictionary<string, TermType>()
        {
            { "#", TermType.sharp },
            { "b", TermType.flat },
            { "add", TermType.add },
            { "aug", TermType.aug },
            { "+", TermType.plus },
            { "-", TermType.minus },
            { "dim", TermType.dim },
            { "o", TermType.dim },
            { "min", TermType.min },
            { "maj", TermType.maj },
            { "m", TermType.min },
            { "M", TermType.maj },
            { "omit", TermType.omit }
        };

        static Dictionary<string, TokenType> _stringToTokenType = new Dictionary<string, TokenType>()
        {
            { "#", TokenType.alt },
            { "b", TokenType.alt },
            { "add", TokenType.add },
            { "omit", TokenType.add },
            { "aug", TokenType.mod },
            { "+", TokenType.term },
            { "-", TokenType.term },
            { "dim", TokenType.mod },
            { "o", TokenType.mod },
            { "min", TokenType.mod },
            { "maj", TokenType.mod },
            { "m", TokenType.mod },
            { "M", TokenType.mod },
        };

        public Term(string s) : base(_stringToTokenType[s])
        {
            TermType = _stringToTermType[s];
        }

        public Term(char c) : this(c.ToString()) {  }
    }
}