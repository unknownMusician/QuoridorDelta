using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuoridorDelta.DataBaseManagementSystem;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller.Abstractions.DataBase
{
    public interface IDataBase
    {
        PlayerInfos PlayerInfos { get; }
        [NotNull] IEnumerable<WallCoords> Walls { get; }

        void MovePawn(PlayerNumber playerNumber, Coords newCoords);
        void PlaceWall(PlayerNumber playerNumber, WallCoords newCoords);
    }
}