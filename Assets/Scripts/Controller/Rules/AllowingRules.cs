using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public class AllowingRules : Rules
    {
        public override bool CanMovePawn(PlayerNumber playerNumber, in Coords newCoords) => true;

        public override bool CanPlaceWall(PlayerNumber playerNumber, in WallCoords newCoords) => true;

        public override IEnumerable<Coords> GetPossiblePawnMoves(PlayerNumber playerNumber)
            => Array.Empty<Coords>();

        public override IEnumerable<WallCoords> GetPossibleWallPlacements(PlayerNumber playerNumber)
            => Array.Empty<WallCoords>();

        public override bool IsWinner(PlayerNumber playerNumber) => false;
    }
}
