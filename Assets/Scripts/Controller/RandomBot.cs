using System;
using System.Collections.Generic;
using System.Linq;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public sealed class RandomBot : Bot
    {
        private readonly Random _random = new Random();

        public override MoveType ChooseMoveType(PlayerNumber playerNumber) => (MoveType) _random.Next(0, 2);

        public override Coords MovePawn(PlayerNumber playerNumber, IEnumerable<Coords> possibleMoves)
            => GetRandom(possibleMoves.ToArray());

        public override WallCoords PlaceWall(PlayerNumber playerNumber, IEnumerable<WallCoords> possibleMoves)
            => GetRandom(possibleMoves.ToArray());

        private T GetRandom<T>(IReadOnlyList<T> possibleCoords)
            => possibleCoords[_random.Next(0, possibleCoords.Count)];
    }
}