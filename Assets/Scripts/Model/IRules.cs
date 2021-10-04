using System.Collections.Generic;

namespace QuoridorDelta.Model
{
    public interface IRules
    {
        bool CanPlaceWall(Player player, ICollection<WallCoords> placedWallCoords, WallCoords newWallCoords);
        bool CanMovePawn(Pawn pawn, Field field, Coords newCoords);
        Coords[] GetPossibleMoves(Pawn pawn, Field field);
        bool DidPlayerWin(PlayerType playerType, Player player);

        WallCoords[] GetPossibleWallPlacements(ICollection<WallCoords> placedWallCoords);
    }
}