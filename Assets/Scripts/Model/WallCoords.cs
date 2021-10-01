
namespace QuoridorDelta.Model
{
    public readonly struct WallCoords
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
    }
}
