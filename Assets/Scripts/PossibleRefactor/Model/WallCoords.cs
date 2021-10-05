namespace PossibleRefactor.Model
{
    public readonly struct WallCoords
    {
        public readonly Coords Coords;
        public readonly WallRotation Rotation;

        public WallCoords(Coords coords, WallRotation rotation)
        {
            Coords = coords;
            Rotation = rotation;
        }
    }
}