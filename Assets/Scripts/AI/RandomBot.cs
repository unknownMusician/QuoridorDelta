using QuoridorDelta.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuoridorDelta.View
{
    public sealed class RandomBot : IBot
    {
        public Coords GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves) 
            => GetRandomPawnCoords(possibleMoves.ToArray());

        public MoveType GetMoveType(PlayerType playerType) // Should be removed
        {
            throw new System.NotImplementedException();
        }

        public WallCoords GetPlaceWallCoords(PlayerType playerType, IEnumerable<WallCoords> possibleWallPlacements) 
            => GetRandomWallCoords(possibleWallPlacements.ToArray());

        public Coords GetRandomPawnCoords(Coords[] possibleCoords)
        {
            Random random = new Random();
            int rInt = random.Next(0, possibleCoords.Length);
            return possibleCoords[rInt];
        }

        public WallCoords GetRandomWallCoords(WallCoords[] possibleWallPlacementCoords)
        {
            Random random = new Random();
            int rInt = random.Next(0, possibleWallPlacementCoords.Count());
            return possibleWallPlacementCoords[rInt];
        }
    }
}