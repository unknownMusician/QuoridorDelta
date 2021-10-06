using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuoridorDelta.Controller;
using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public interface ISyncInput
    {
        void GetGameType([NotNull] Action<GameType> handler);

        void GetMoveType(PlayerNumber playerNumber, [NotNull] Action<MoveType> handler);

        void GetMovePawnCoords(
            PlayerNumber playerNumber,
            [NotNull] IEnumerable<Coords> possibleMoves,
            [NotNull] Action<Coords> handler
        );

        void GetPlaceWallCoords(
            PlayerNumber playerNumber,
            [NotNull] IEnumerable<WallCoords> possibleWallPlacements,
            [NotNull] Action<WallCoords> handler
        );


        void ShouldRestart([NotNull] Action<bool> handler);
    }
}
