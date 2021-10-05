using System.Collections.Generic;
using JetBrains.Annotations;
using PossibleRefactor.Controller;
using PossibleRefactor.Model;

namespace PossibleRefactor.DataBaseManagementSystem
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