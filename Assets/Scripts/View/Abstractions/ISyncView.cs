using System.Collections.Generic;
using QuoridorDelta.Controller;
using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public interface ISyncView
    {
        void ShowWinner(PlayerNumber playerNumber);
        void ShowWrongMove(MoveType moveType);

        void InitializeField(PlayerInfos playerInfos, IEnumerable<WallCoords> wallCoords);
        void MovePawn(PlayerInfos playerInfos, IEnumerable<WallCoords> wallCoords, PlayerNumber playerNumber, Coords newCoords);
        void PlaceWall(PlayerInfos playerInfos, IEnumerable<WallCoords> wallCoords, PlayerNumber playerNumber, WallCoords newCoords);
    }
}