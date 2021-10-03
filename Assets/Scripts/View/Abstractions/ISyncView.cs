using QuoridorDelta.Model;
using System;
using System.Collections.Generic;

namespace QuoridorDelta.View
{
    public interface ISyncView
    {
        void GetGameType(Action<GameType> handler);

        void GetMoveType(PlayerType playerType, Action<MoveType> handler);
        void GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves, Action<Coords> handler);
        void GetPlaceWallCoords(PlayerType playerType, Action<WallCoords> handler);

        void MovePawn(PlayerType playerType, Coords newCoords);
        void PlaceWall(PlayerType playerType, Coords newCoords);
        void ShowWrongMove(PlayerType playerType, MoveType moveType);

        void ShowWinner(PlayerType playerType);
        void ShouldRestart(Action<bool> handler);
    }
}