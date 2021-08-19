namespace GuitarChordFinder
{
    internal class Number : Token
    {
        public int Value { get; private set; }

        public Number(int i) : base(TokenType.num)
        {
            Value = i;
        }
    }
}