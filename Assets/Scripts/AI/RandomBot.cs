using QuoridorDelta.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuoridorDelta.View
{
    public sealed class RandomBot : IBot
    {
        private readonly Random _random = new Random();

        public Coords GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves)
            => GetRandomPawnCoords(possibleMoves.ToArray());

        public MoveType GetMoveType(PlayerType playerType) => (MoveType) _random.Next(0, 2);

        public WallCoords GetPlaceWallCoords(PlayerType playerType, IEnumerable<WallCoords> possibleWallPlacements)
            => GetRandomWallCoords(possibleWallPlacements.ToArray());

        public Coords GetRandomPawnCoords(Coords[] possibleCoords)
        {
            int rInt = _random.Next(0, possibleCoords.Length);

            return possibleCoords[rInt];
        }

        public WallCoords GetRandomWallCoords(WallCoords[] possibleWallPlacementCoords)
        {
            int rInt = _random.Next(0, possibleWallPlacementCoords.Length);

            return possibleWallPlacementCoords[rInt];
        }
    }
}