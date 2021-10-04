namespace QuoridorDelta.Model
{
    public class Player
    {
        public readonly Pawn Pawn;
        public int WallCount { get; set; }

        public Player(Pawn pawn, int wallCount)
        {
            Pawn = pawn;
            WallCount = wallCount;
        }
    }
}