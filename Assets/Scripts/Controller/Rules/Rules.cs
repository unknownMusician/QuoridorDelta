using System.Collections.Generic;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public abstract class Rules : GameStateRememberer
    {
        public abstract bool CanMovePawn(PlayerNumber playerNumber, Coords newCoords);
        public abstract bool CanPlaceWall(PlayerNumber playerNumber, WallCoords newCoords);
        public abstract IEnumerable<Coords> GetPossiblePawnMoves(PlayerNumber playerNumber);
        public abstract IEnumerable<WallCoords> GetPossibleWallPlacements(PlayerNumber playerNumber);
        public abstract bool IsWinner(PlayerNumber playerNumber);
    }
}