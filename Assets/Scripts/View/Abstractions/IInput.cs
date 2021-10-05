using System.Collections.Generic;
using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public interface IInput
    {
        MoveType GetMoveType(PlayerType playerType);
        Coords GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves);
        WallCoords GetPlaceWallCoords(PlayerType playerType, IEnumerable<WallCoords> possibleWallPlacements);
    }
}