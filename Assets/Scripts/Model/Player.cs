namespace Quoridor.Model
{
    public class Player
    {
        public readonly Pawn PlayerPawn;
        public int _amountOfWallsCanSet { get; private set; }

        public Player(Pawn pawn,int amountOfWalls)
        {
            PlayerPawn = pawn;
            _amountOfWallsCanSet = amountOfWalls;
        }

        public void DecremntAmountOfWalls()
        {
            _amountOfWallsCanSet--;
        }
    }
}
