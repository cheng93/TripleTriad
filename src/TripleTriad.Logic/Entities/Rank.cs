namespace TripleTriad.Logic.Entities
{
    public class Rank
    {
        public Rank(int top, int right, int bottom, int left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public int Top { get; }

        public int Right { get; }

        public int Bottom { get; }

        public int Left { get; }
    }
}