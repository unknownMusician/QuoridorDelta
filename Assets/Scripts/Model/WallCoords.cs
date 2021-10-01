
namespace QuoridorDelta.Model
{
    public struct WallCoords
    {
        public readonly Coords Coords;
        public readonly WallOrientation Orientation;

        public WallCoords(Coords coords, WallOrientation orientation)
        {
            Coords = coords;
            Orientation = orientation;
        }
    }
}
