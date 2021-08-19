namespace GuitarChordFinder
{
    internal class Note : Token
    {
        public int Value { get; private set; }

        public Note(int n) : base(TokenType.note)
        {
            Value = n;
        }
    }
}