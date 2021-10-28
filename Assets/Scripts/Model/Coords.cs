#nullable enable

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

        public static implicit operator Coords(in (int x, int y) tuple) => new Coords(tuple.x, tuple.y);

        public bool Equals(Coords other) => Equals(in other);
        public bool Equals(in Coords other) => other.X == X && other.Y == Y;

        public override bool Equals(object obj) => obj is Coords other && Equals(in other);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => $"Coords ({X}, {Y})";

        public static bool operator ==(in Coords c1, in Coords c2) => c1.Equals(in c2);
        public static bool operator !=(in Coords c1, in Coords c2) => !c1.Equals(in c2);

        public static Coords operator +(in Coords c1, in Coords c2) => (c1.X + c2.X, c1.Y + c2.Y);
        public static Coords operator -(in Coords c1, in Coords c2) => (c1.X - c2.X, c1.Y - c2.Y);
        public static Coords operator *(in Coords c1, int v) => (c1.X * v, c1.Y * v);

        public int this[int index]
            => index switch
            {
                0 => X,
                1 => Y,
                _ => throw new System.ArgumentOutOfRangeException()
            };
    }
}
