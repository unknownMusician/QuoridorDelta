using System;
using System.Collections.Generic;
using System.Linq;
using QuoridorDelta.Controller.PathFinding;
using QuoridorDelta.DataBaseManagementSystem;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public sealed class IntelligentBot : Bot
    {
        public IntelligentBot(ref Action<GameState, IDBChangeInfo>? onDBChange) => onDBChange += HandleChange;

        public override MoveType ChooseMoveType(PlayerNumber playerNumber) => MoveType.MovePawn;

        public override Coords MovePawn(PlayerNumber playerNumber, IEnumerable<Coords> possibleMoves)
        {
            IPathFinder pathFinder = new PathFinderAStar();

            var paths = new Dictionary<Coords, int>();

            foreach (Coords move in possibleMoves)
            {
                paths[move] =
                    pathFinder.GetShortestPathLength(GraphHelper.ToGraph(LastGameState, move, playerNumber));
            }

            Coords result = paths.OrderBy(path => path.Value).First().Key;

            return result;
        }

        public override WallCoords PlaceWall(PlayerNumber playerNumber, IEnumerable<WallCoords> possibleMoves)
            => throw new NotImplementedException();
    }
}
