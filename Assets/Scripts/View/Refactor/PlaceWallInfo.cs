namespace QuoridorDelta.View.Refactor
{
    public readonly struct PlaceWallInfo : IMoveInfo
    {
        public readonly int WallIndex;

        public PlaceWallInfo(int wallIndex) => WallIndex = wallIndex;
    }
}
