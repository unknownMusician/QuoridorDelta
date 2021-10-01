
namespace QuoridorDelta.Model
{
    public readonly struct Coords
    {
        public readonly int X;
        public readonly int Y;

        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }

        public static implicit operator Coords((int x, int y) tuple) => new Coords(tuple.x, tuple.y);
    }
}
