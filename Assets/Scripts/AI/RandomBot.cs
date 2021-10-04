using QuoridorDelta.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuoridorDelta.View
{
    public sealed class RandomBot : IInput
    {
        private readonly Random _random = new Random();

        public Coords GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves)
            => GetRandom(possibleMoves.ToArray());

        public MoveType GetMoveType(PlayerType playerType) => (MoveType) _random.Next(0, 2);

        public WallCoords GetPlaceWallCoords(PlayerType playerType, IEnumerable<WallCoords> possibleWallPlacements)
            => GetRandom(possibleWallPlacements.ToArray());

        private T GetRandom<T>(IReadOnlyList<T> possibleCoords)
            => possibleCoords[_random.Next(0, possibleCoords.Count)];
    }
}