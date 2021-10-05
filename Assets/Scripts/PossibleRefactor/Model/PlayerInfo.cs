namespace PossibleRefactor.Model
{
    public readonly struct PlayerInfo
    {
        public readonly Coords PawnCoords;
        public readonly int WallCount;

        public PlayerInfo(Coords pawnCoords, int wallCount)
        {
            PawnCoords = pawnCoords;
            WallCount = wallCount;
        }
    }
}