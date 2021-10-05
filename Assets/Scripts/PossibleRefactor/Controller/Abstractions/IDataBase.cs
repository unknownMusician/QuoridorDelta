using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using PossibleRefactor.DataBaseManagementSystem;
using PossibleRefactor.Model;

namespace PossibleRefactor.Controller
{
    public interface IDataBase
    {
        PlayerInfos PlayerInfos { get; }
        [NotNull] IEnumerable<WallCoords> Walls { get; }

        void MovePawn(PlayerNumber playerNumber, Coords newCoords);
        void PlaceWall(PlayerNumber playerNumber, WallCoords newCoords);
    }
}