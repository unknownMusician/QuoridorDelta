#nullable enable

using System.Collections.Generic;
using QuoridorDelta.Model;

namespace QuoridorDelta.DataBase
{
    public sealed class DB
    {
        public PlayerInfoContainer<PlayerInfo> PlayerInfoContainer { get; set; }
        public List<WallCoords> Walls { get; set; }

        public DB(PlayerInfoContainer<PlayerInfo> playerInfos, List<WallCoords> walls)
        {
            PlayerInfoContainer = playerInfos;
            Walls = walls;
        }
    }
}
