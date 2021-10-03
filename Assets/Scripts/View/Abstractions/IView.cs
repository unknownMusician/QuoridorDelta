using QuoridorDelta.Model;
using System.Collections.Generic;

namespace QuoridorDelta.View
{
    public interface IView
    {
        GameType GetGameType();

        MoveType GetMoveType(PlayerType playerType);
        Coords GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves);
        WallCoords GetPlaceWallCoords(PlayerType playerType);

        void MovePlayerPawn(PlayerType playerType, Coords newCoords);
        void PlacePlayerWall(PlayerType playerType, Coords newCoords);
        void ShowWrongMove(PlayerType playerType, MoveType moveType);

        void ShowWinner(PlayerType playerType);
        bool ShouldRestart();
    }
}