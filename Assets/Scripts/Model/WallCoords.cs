#nullable enable

namespace QuoridorDelta.Model
{
    public readonly struct WallCoords : System.IEquatable<WallCoords>
    {
        public readonly Coords Coords;
        public readonly WallRotation Rotation;

        public WallCoords(in Coords coords, WallRotation rotation)
        {
            Coords = coords;
            Rotation = rotation;
        }

        public void Deconstruct(out Coords coords, out WallRotation rotation)
        {
            coords = Coords;
            rotation = Rotation;
        }

        public static implicit operator WallCoords(in (Coords coords, WallRotation orientation) tuple)
            => new WallCoords(tuple.coords, tuple.orientation);

        public override bool Equals(object obj) => obj is WallCoords other && Equals(in other);
        public bool Equals(in WallCoords other) => other.Coords == Coords && other.Rotation == Rotation;
        public bool Equals(WallCoords other) => Equals(in other);

        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => $"WallCoords ({Coords}, {Rotation})";

        public static bool operator ==(in WallCoords c1, in WallCoords c2) => c1.Equals(in c2);
        public static bool operator !=(in WallCoords c1, in WallCoords c2) => !c1.Equals(in c2);

        public static WallCoords operator +(in WallCoords c1, in Coords c2) => (c1.Coords + c2, c1.Rotation);
        public static WallCoords operator -(in WallCoords c1, in Coords c2) => (c1.Coords - c2, c1.Rotation);
    }
}
