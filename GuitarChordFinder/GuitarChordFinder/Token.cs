using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarChordFinder
{
    public enum TokenType
    {
        num,
        note,
        slash,
        term,
        sus,
        mod,
        add,
        alt,
        ext,
        end
    }

    public class Token
    {
        public TokenType Type { get; private set; }
        
        public Token(TokenType type)
        {
            Type = type;
        }
    }
}
