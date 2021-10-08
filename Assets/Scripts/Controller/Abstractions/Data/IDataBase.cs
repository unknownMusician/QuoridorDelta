using System.Collections.Generic;
using JetBrains.Annotations;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller.Abstractions.DataBase
{
    public interface IDataBase
    {
        PlayerInfoContainer<PlayerInfo> PlayerInfoContainer { get; }
        [NotNull] IEnumerable<WallCoords> Walls { get; }

        void MovePawn(PlayerNumber playerNumber, Coords newCoords);
        void PlaceWall(PlayerNumber playerNumber, WallCoords newCoords);
    }
}
