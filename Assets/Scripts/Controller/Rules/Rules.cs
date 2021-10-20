using System.Collections.Generic;
using JetBrains.Annotations;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public abstract class Rules : GameStateRememberer
    {
        public abstract bool CanMovePawn(PlayerNumber playerNumber, in Coords newCoords);
        public abstract bool CanPlaceWall(PlayerNumber playerNumber, in WallCoords newCoords);

        [NotNull]
        public abstract IEnumerable<Coords> GetPossiblePawnMoves(PlayerNumber playerNumber);

        [NotNull]
        public abstract IEnumerable<WallCoords> GetPossibleWallPlacements(PlayerNumber playerNumber);

        public abstract bool IsWinner(PlayerNumber playerNumber);
    }
}
