using PossibleRefactor.Model;

namespace PossibleRefactor.Controller
{
    public abstract class Rules : GameStateRememberer
    {
        public abstract bool CanMovePawn(PlayerNumber playerNumber, Coords newCoords);
        public abstract bool CanPlaceWall(PlayerNumber playerNumber, WallCoords newCoords);
        public abstract bool IsWinner(PlayerNumber playerNumber);
    }
}