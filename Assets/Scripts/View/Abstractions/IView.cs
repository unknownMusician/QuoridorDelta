using QuoridorDelta.Model;
using System.Collections.Generic;

namespace QuoridorDelta.View
{
    public interface IView
    {
        MoveType GetMoveType(PlayerType playerType);
        Coords GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves);
        WallCoords GetPlaceWallCoords(PlayerType playerType, IEnumerable<WallCoords> possibleMoves);
    }
}