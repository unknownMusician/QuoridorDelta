
namespace QuoridorDelta.Model
{
    public class Player
    {
        public readonly Pawn Pawn;
        public int WallCount { get; private set; }

        public Player(Pawn pawn, int wallCount)
        {
            Pawn = pawn;
            WallCount = wallCount;
        }

        public void DecremntWallCount()
        {
            if (WallCount > 0)
            {
                WallCount--;
            }
            else
            {
                throw new System.InvalidOperationException();
            }
        }
    }
}
