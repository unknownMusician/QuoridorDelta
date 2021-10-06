using System.Collections.Generic;
using JetBrains.Annotations;
using QuoridorDelta.Controller;
using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public interface ISyncView
    {
        void ShowWinner(PlayerNumber playerNumber);
        void ShowWrongMove(MoveType moveType);

        void InitializeField(PlayerInfos playerInfos, [NotNull] IEnumerable<WallCoords> wallCoords);

        void MovePawn(
            PlayerInfos playerInfos,
            [NotNull] IEnumerable<WallCoords> wallCoords,
            PlayerNumber playerNumber,
            Coords newCoords
        );

        void PlaceWall(
            PlayerInfos playerInfos,
            [NotNull] IEnumerable<WallCoords> wallCoords,
            PlayerNumber playerNumber,
            WallCoords newCoords
        );
    }
}
