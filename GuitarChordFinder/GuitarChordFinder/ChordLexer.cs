using System.Collections.Generic;

namespace GuitarChordFinder
{
    internal class ChordLexer
    {
        string _symbol;
        int _index;
        bool _isBNote;
        static Dictionary<char, int> _notes = new Dictionary<char, int>() { { 'c', 0 }, { 'd', 2 }, { 'e', 4 }, { 'f', 5 }, { 'g', 7 }, { 'a', 9 }, { 'b', 11 } };

        public bool IsEnd { get { return _index == _symbol.Length; } }

        public ChordLexer(string symbol)
        {
            _symbol = symbol;
            _index = 0;
            _isBNote = true;
            SkipWhitespaces();
        }

        public Token NextToken()
        {
            if (IsEnd)
                return new Token(TokenType.end);
            int move = 0;
            char c = _symbol[_index];
            if (char.IsNumber(c))
            {
                _index++;
                int i = c - '0';
                if (c == '1')
                {
                    if (IsEnd)
                        return null;
                    i *= 10;
                    i += _symbol[_index++] - '0';
                }

                SkipWhitespaces();
                _isBNote = false;
                return new Number(i);
            }
            if (c == '/')
            {
                _isBNote = true;
                _index++;
                SkipWhitespaces();
                return new Token(TokenType.slash);
            }
            if (c == 'b')
            {
                _index++;
                if (_isBNote)
                {
                    _isBNote = false;
                    move = 0;
                    if (!IsEnd && _symbol[_index] == '#')
                    {
                        move = 1;
                        _index++;
                    }
                    else if (!IsEnd && _symbol[_index] == 'b')
                    {
                        move = -1;
                        _index++;
                    }
                    SkipWhitespaces();
                    return new Note((_notes['b'] + move) % 12);
                }
                SkipWhitespaces();
                return new Term('b');
            }
            if (c == '#' || c == '+' || c == '-')
            {
                _index++;
                SkipWhitespaces();
                return new Term(c);
            }
            if (char.ToLower(c) == 'o')
            {
                _index++;
                if (_index + 3 <= _symbol.Length && char.ToLower(_symbol[_index]) == 'm' && char.ToLower(_symbol[_index + 1]) == 'i' && char.ToLower(_symbol[_index + 2]) == 't')
                {
                    _index += 3;
                    SkipWhitespaces();
                    return new Term("omit");
                }
                if (c == 'o')
                {
                    SkipWhitespaces();
                    return new Term('o');
                }
                return null;
            }
            if (char.ToLower(c) == 'm')
            {
                _index++;
                if (_index + 2 <= _symbol.Length)
                {
                    if (char.ToLower(_symbol[_index]) == 'a' && char.ToLower(_symbol[_index + 1]) == 'j')
                    {
                        _index += 2;
                        SkipWhitespaces();
                        return new Term("maj");
                    }
                    else if (char.ToLower(_symbol[_index]) == 'i' && char.ToLower(_symbol[_index + 1]) == 'n')
                    {
                        _index += 2;
                        SkipWhitespaces();
                        return new Term("min");
                    }
                }
                SkipWhitespaces();
                return new Term(c);
            }
            if (char.ToLower(c) == 'a')
            {
                _index++;
                if (_index + 2 <= _symbol.Length)
                {
                    if (char.ToLower(_symbol[_index]) == 'd' && char.ToLower(_symbol[_index + 1]) == 'd')
                    {
                        _index += 2;
                        SkipWhitespaces();
                        return new Term("add");
                    }
                    else if (char.ToLower(_symbol[_index]) == 'u' && char.ToLower(_symbol[_index + 1]) == 'g')
                    {
                        _index += 2;
                        SkipWhitespaces();
                        return new Term("aug");
                    }
                }
                _isBNote = false;
                move = 0;
                if (!IsEnd && _symbol[_index] == '#')
                {
                    move = 1;
                    _index++;
                }
                else if (!IsEnd && _symbol[_index] == 'b')
                {
                    move = -1;
                    _index++;
                }
                SkipWhitespaces();
                return new Note((_notes['a'] + move) % 12);
                
            }
            if (char.ToLower(c) == 'd')
            {
                _index++;
                if (_index + 2 <= _symbol.Length && char.ToLower(_symbol[_index]) == 'i' && char.ToLower(_symbol[_index + 1]) == 'm')
                {
                    _index += 2;
                    SkipWhitespaces();
                    return new Term("dim");
                }
                else
                {
                    _isBNote = false;
                    move = 0;
                    if (!IsEnd && _symbol[_index] == '#')
                    {
                        move = 1;
                        _index++;
                    }
                    else if (!IsEnd && _symbol[_index] == 'b')
                    {
                        move = -1;
                        _index++;
                    }
                    SkipWhitespaces();
                    return new Note((_notes['d'] + move) % 12);
                }
            }
            if (_index + 3 <= _symbol.Length && char.ToLower(_symbol[_index]) == 's' && char.ToLower(_symbol[_index + 1]) == 'u' && char.ToLower(_symbol[_index + 2]) == 's')
            {
                _index += 3;
                SkipWhitespaces();
                return new Token(TokenType.sus);
            }
            _index++;
            _isBNote = false;
            if (!_notes.ContainsKey(char.ToLower(c)))
                return null;
            move = 0;
            if (!IsEnd && _symbol[_index] == '#')
            {
                move = 1;
                _index++;
            }
            else if (!IsEnd && _symbol[_index] == 'b')
            {
                move = -1;
                _index++;
            }
            SkipWhitespaces();
            return new Note((_notes[char.ToLower(c)] + move) % 12);
        }

        void SkipWhitespaces()
        {
            while (!IsEnd && char.IsWhiteSpace(_symbol[_index]))
                _index++;
        }
    }
}