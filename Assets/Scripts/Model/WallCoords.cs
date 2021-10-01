
namespace QuoridorDelta.Model
{
    public readonly struct WallCoords : System.IEquatable<WallCoords>
    {
        public readonly Coords Coords;
        public readonly WallOrientation Orientation;

        public WallCoords(Coords coords, WallOrientation orientation)
        {
            Coords = coords;
            Orientation = orientation;
        }

        public void Deconstruct(out Coords coords, out WallOrientation orientation)
        {
            coords = Coords;
            orientation = Orientation;
        }

        public static implicit operator WallCoords((Coords coords, WallOrientation orientation) tuple) => new WallCoords(tuple.coords, tuple.orientation);

        public bool Equals(WallCoords other) => other.Coords == Coords && other.Orientation == Orientation;

        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => $"WallCoords ({Coords}, {Orientation})";

        public static bool operator ==(WallCoords c1, WallCoords c2) => c1.Equals(c2);
        public static bool operator !=(WallCoords c1, WallCoords c2) => !c1.Equals(c2);

        public static WallCoords operator +(WallCoords c1, Coords c2) => (c1.Coords + c2, c1.Orientation);
        public static WallCoords operator -(WallCoords c1, Coords c2) => (c1.Coords - c2, c1.Orientation);
    }
}
