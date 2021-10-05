using System.Collections.Generic;
using QuoridorDelta.Model;

namespace QuoridorDelta.DataBase
{
    public class DB
    {
        public PlayerInfos PlayerInfos { get; set; }
        public List<WallCoords> Walls { get; set; }

        public DB() { }

        public DB(PlayerInfos playerInfos, List<WallCoords> walls)
        {
            PlayerInfos = playerInfos;
            Walls = walls;
        }
    }
}