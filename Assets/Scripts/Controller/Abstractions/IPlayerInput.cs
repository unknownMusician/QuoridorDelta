using System.Collections.Generic;
using JetBrains.Annotations;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public interface IPlayerInput
    {
        MoveType ChooseMoveType(PlayerNumber playerNumber);
        Coords MovePawn(PlayerNumber playerNumber, [NotNull] IEnumerable<Coords> possibleMoves);
        WallCoords PlaceWall(PlayerNumber playerNumber, [NotNull] IEnumerable<WallCoords> possibleMoves);
    }
}
