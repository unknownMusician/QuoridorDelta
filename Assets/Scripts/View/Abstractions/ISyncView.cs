﻿using QuoridorDelta.Model;
using System;
using System.Collections.Generic;

namespace QuoridorDelta.View
{
    public interface ISyncView
    {
        void GetMoveType(PlayerType playerType, Action<MoveType> handler);
        void GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves, Action<Coords> handler);
        void GetPlaceWallCoords(PlayerType playerType, IEnumerable<WallCoords> possibleMoves, Action<WallCoords> handler);
    }
}