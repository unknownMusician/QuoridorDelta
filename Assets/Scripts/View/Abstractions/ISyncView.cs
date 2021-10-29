using System.Collections.Generic;
using QuoridorDelta.Controller;
using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public interface ISyncView
    {
        void ShowWinner(PlayerNumber playerNumber);
        void ShowWrongMove(MoveType moveType);

        void InitializeField(in PlayerInfoContainer<PlayerInfo> playerInfos, IEnumerable<WallCoords> wallCoords);

        void MovePawn(
            in PlayerInfoContainer<PlayerInfo> playerInfos,
            IEnumerable<WallCoords> wallCoords,
            PlayerNumber playerNumber,
            in Coords newCoords
        );

        void PlaceWall(
            in PlayerInfoContainer<PlayerInfo> playerInfos,
            IEnumerable<WallCoords> wallCoords,
            PlayerNumber playerNumber,
            in WallCoords newCoords
        );
    }
}
