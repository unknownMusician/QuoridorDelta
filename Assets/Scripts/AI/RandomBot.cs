using QuoridorDelta.Model;
using System;
using System.Collections.Generic;

namespace QuoridorDelta.View
{
    public sealed class RandomBot : IBot
    {
        public Coords GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves)
        {
            return GetRandomWallCoords(possibleMoves);
        }

        public MoveType GetMoveType(PlayerType playerType) // Should be removed
        {
            throw new System.NotImplementedException();
        }

        public WallCoords GetPlaceWallCoords(PlayerType playerType, IEnumerable<WallCoords> possibleWallPlacements)
        {
            return GetRandomWallCoords(possibleWallPlacements);
        }

        public Coords GetRandomPawnCoords(Coords[] possibleCoords)
        {
            Random random = new Random();
            int rInt = random.Next(0, possibleCoords.Length);
            return possibleCoords[rInt];
        }

        public WallCoords GetRandomWallCoords(WallCoords[] possibleWallPlacementCoords)
        {
            Random random = new Random();
            int rInt = random.Next(0, possibleWallPlacementCoords.Length);
            return possibleWallPlacementCoords[rInt];
        }
    }
}