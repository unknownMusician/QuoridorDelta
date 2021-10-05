using System;
using System.Collections.Generic;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public class AllowingRules : Rules
    {
        // todo
        public override bool CanMovePawn(PlayerNumber playerNumber, Coords newCoords) => true;

        // todo
        public override bool CanPlaceWall(PlayerNumber playerNumber, WallCoords newCoords) => true;
        
        // todo
        public override IEnumerable<Coords> GetPossiblePawnMoves(PlayerNumber playerNumber) 
            => Array.Empty<Coords>();

        // todo
        public override IEnumerable<WallCoords> GetPossibleWallPlacements(PlayerNumber playerNumber) 
            => Array.Empty<WallCoords>();

        // todo
        public override bool IsWinner(PlayerNumber playerNumber) => false;
    }
}