namespace Quoridor.Model
{
    public class Player
    {
        public readonly Pawn PlayerPawn;
        public int AmountOfWallsCanSet { get; private set; }

        public Player(Pawn pawn,int amountOfWalls)
        {
            PlayerPawn = pawn;
            AmountOfWallsCanSet = amountOfWalls;
        }

        public void DecremntAmountOfWalls()
        {
            AmountOfWallsCanSet--;
        }
    }
}
