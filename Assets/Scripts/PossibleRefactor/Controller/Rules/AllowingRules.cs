using PossibleRefactor.Model;

namespace PossibleRefactor.Controller
{
    public class AllowingRules : Rules
    {
        // todo
        public override bool CanMovePawn(PlayerNumber playerNumber, Coords newCoords) => true;

        // todo
        public override bool CanPlaceWall(PlayerNumber playerNumber, WallCoords newCoords) => true;

        // todo
        public override bool IsWinner(PlayerNumber playerNumber) => false;
    }
}