namespace QuoridorDelta.Model
{
    public readonly struct WallCoords : System.IEquatable<WallCoords>
    {
        public readonly Coords Coords;
        public readonly WallRotation Rotation;

        public WallCoords(Coords coords, WallRotation rotation)
        {
            Coords = coords;
            Rotation = rotation;
        }

        public void Deconstruct(out Coords coords, out WallRotation rotation)
        {
            coords = Coords;
            rotation = Rotation;
        }

        public static implicit operator WallCoords((Coords coords, WallRotation orientation) tuple)
            => new WallCoords(tuple.coords, tuple.orientation);

        public override bool Equals(object obj) => obj is WallCoords other && Equals(other);
        public bool Equals(WallCoords other) => other.Coords == Coords && other.Rotation == Rotation;

        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => $"WallCoords ({Coords}, {Rotation})";

        public static bool operator ==(WallCoords c1, WallCoords c2) => c1.Equals(c2);
        public static bool operator !=(WallCoords c1, WallCoords c2) => !c1.Equals(c2);

        public static WallCoords operator +(WallCoords c1, Coords c2) => (c1.Coords + c2, c1.Rotation);
        public static WallCoords operator -(WallCoords c1, Coords c2) => (c1.Coords - c2, c1.Rotation);
    }
}
