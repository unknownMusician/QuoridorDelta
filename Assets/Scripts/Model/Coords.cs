
namespace QuoridorDelta.Model
{
    public readonly struct Coords : System.IEquatable<Coords>
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

        public bool Equals(Coords other) => other.X == X && other.Y == Y;

        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => $"Coords ({X}, {Y})";

        public static bool operator ==(Coords c1, Coords c2) => c1.Equals(c2);
        public static bool operator !=(Coords c1, Coords c2) => !c1.Equals(c2);

        public static Coords operator +(Coords c1, Coords c2) => (c1.X + c2.X, c1.Y + c2.Y);
        public static Coords operator -(Coords c1, Coords c2) => (c1.X - c2.X, c1.Y - c2.Y);
    }
}
