#nullable enable

using System.Collections.Generic;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller.Abstractions.DataBase
{
    public interface IDataBase
    {
        PlayerInfoContainer<PlayerInfo> PlayerInfoContainer { get; }
        IEnumerable<WallCoords> Walls { get; }

        void MovePawn(PlayerNumber playerNumber, in Coords newCoords);
        void PlaceWall(PlayerNumber playerNumber, in WallCoords newCoords);
    }
}
