namespace Quoridor.Model
{
    public enum WallOrientation
    {
        Horizontal = 0,
        Vertical = 1
    }
    public sealed class Wall
    {
        public readonly Coords WallCoords;
        public readonly WallOrientation Orientation;
        public Wall(WallOrientation orientation, Coords coords) {
            Orientation = orientation;
            WallCoords = coords;
        }
        
    }
}
