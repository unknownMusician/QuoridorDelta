using QuoridorDelta.Model;
using System;

namespace QuoridorDelta.View
{
    public interface ISyncView
    {
        void GetMoveType(PlayerType playerType, Action<MoveType> handler);
        void GetMovePawnCoords(PlayerType playerType, Action<Coords> handler);
        void GetPlaceWallCoords(PlayerType playerType, Action<WallCoords> handler);
    }
}