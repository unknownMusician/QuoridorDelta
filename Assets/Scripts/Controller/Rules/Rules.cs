using System.Collections.Generic;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public abstract class Rules : GameStateRememberer
    {
        public abstract bool CanMovePawn(PlayerNumber playerNumber, in Coords newCoords);
        public abstract bool CanPlaceWall(PlayerNumber playerNumber, in WallCoords newCoords);

        public abstract IEnumerable<Coords> GetPossiblePawnMoves(PlayerNumber playerNumber);

        public abstract IEnumerable<WallCoords> GetPossibleWallPlacements(PlayerNumber playerNumber);

        public abstract bool IsWinner(PlayerNumber playerNumber);
    }
}
