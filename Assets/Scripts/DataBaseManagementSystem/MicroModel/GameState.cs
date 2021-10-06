using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuoridorDelta.Controller;
using QuoridorDelta.Controller.Abstractions.DataBase;
using QuoridorDelta.Model;

namespace QuoridorDelta.DataBaseManagementSystem
{
    public sealed class GameState
    {
        [NotNull] public static GameState Empty => new GameState(default, Array.Empty<WallCoords>());

        public readonly PlayerInfos PlayerInfos;
        [NotNull] public readonly IEnumerable<WallCoords> Walls;

        internal GameState(PlayerInfos playerInfos, [NotNull] IEnumerable<WallCoords> walls)
        {
            PlayerInfos = playerInfos;
            Walls = walls;
        }

        internal GameState([NotNull] IDataBase dbms) : this(dbms.PlayerInfos, dbms.Walls) { }
    }
}
