using System;
using System.Collections.Generic;
using QuoridorDelta.Controller;
using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public interface ISyncInput
    {
        void GetGameType(Action<GameType> handler);

        void GetMoveType(PlayerNumber playerNumber, Action<MoveType> handler);
        void GetMovePawnCoords(PlayerNumber playerNumber, IEnumerable<Coords> possibleMoves, Action<Coords> handler);
        void GetPlaceWallCoords(PlayerNumber playerNumber, IEnumerable<WallCoords> possibleWallPlacements, Action<WallCoords> handler);

        
        void ShouldRestart(Action<bool> handler);
    }
}