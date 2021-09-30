namespace Quoridor.Model
{
    public enum WallOrientation
    {
        Horizontal = 0,
        Vertical = 1
    }
    public sealed class Wall
    {
        public readonly WallOrientation Orientation;
        public Wall(WallOrientation orientation) => Orientation = orientation;
    }
}
