using System.Collections.Generic;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public interface IPlayerInput
    {
        MoveType ChooseMoveType(PlayerNumber playerNumber);
        Coords MovePawn(PlayerNumber playerNumber, IEnumerable<Coords> possibleMoves);
        WallCoords PlaceWall(PlayerNumber playerNumber, IEnumerable<WallCoords> possibleMoves);
    }
}