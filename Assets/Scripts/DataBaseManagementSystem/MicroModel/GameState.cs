using System.Collections.Generic;
using JetBrains.Annotations;
using QuoridorDelta.Controller;
using QuoridorDelta.Model;

namespace QuoridorDelta.DataBaseManagementSystem
{
    public sealed class GameState
    {
        public readonly PlayerInfos PlayerInfos;
        [NotNull] public readonly IEnumerable<WallCoords> Walls;

        internal GameState(PlayerInfos playerInfos, [NotNull] IEnumerable<WallCoords> walls)
        {
            PlayerInfos = playerInfos;
            Walls = walls;
        }

        internal GameState(DBMS dbms) : this(dbms.PlayerInfos, dbms.Walls) { }
    }
}