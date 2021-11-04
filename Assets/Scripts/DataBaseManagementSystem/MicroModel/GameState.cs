#nullable enable

using System;
using System.Collections.Generic;
using QuoridorDelta.Controller.Abstractions.DataBase;
using QuoridorDelta.Model;

namespace QuoridorDelta.DataBaseManagementSystem
{
    public sealed class GameState
    {
        public static GameState Empty => new GameState(default, Array.Empty<WallCoords>());

        public readonly PlayerInfoContainer<PlayerInfo> PlayerInfoContainer;
        public readonly IEnumerable<WallCoords> Walls;

        internal GameState(in PlayerInfoContainer<PlayerInfo> playerInfos, IEnumerable<WallCoords> walls)
        {
            PlayerInfoContainer = playerInfos;
            Walls = walls;
        }

        internal GameState(IDataBase dbms) : this(dbms.PlayerInfoContainer, dbms.Walls) { }

        public GameState With(in PlayerInfoContainer<PlayerInfo> playerInfoContainer)
            => new GameState(playerInfoContainer, Walls);

        public GameState With(IEnumerable<WallCoords> walls)
            => new GameState(PlayerInfoContainer, walls);
    }
}
