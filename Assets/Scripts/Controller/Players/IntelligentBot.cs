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
        private Coords? _computedCoords;
        private WallCoords? _computedWallCoords;
        private readonly IPathFinder _pathFinder = new PathFinderAStar();

        public IntelligentBot(ref Action<GameState, IDBChangeInfo>? onDBChange) => onDBChange += HandleChange;

        public override MoveType ChooseMoveType(PlayerNumber playerNumber)
        {
            _pathFinder.TryGetShortestPathLength(GraphHelper.ToGraph(LastGameState, playerNumber),
                                                 out int currentBotPath);

            _pathFinder.TryGetShortestPathLength(GraphHelper.ToGraph(LastGameState, playerNumber.Changed()),
                                                 out int currentEnemyPath);

            _computedCoords = GetBestMove(playerNumber, out int moveLength);
            _computedWallCoords = GetBestPlace(playerNumber, out int placeLength);

            return (moveLength - currentBotPath) <= (currentEnemyPath - placeLength) ?
                MoveType.MovePawn :
                MoveType.PlaceWall;
        }

        private Coords GetBestMove(PlayerNumber playerNumber, out int length)
        {
            IEnumerable<Coords> possiblePawnMoves = OldRules.GetPossiblePawnMoves(playerNumber, LastGameState);

            var movePaths = new Dictionary<Coords, int>();

            foreach (Coords move in possiblePawnMoves)
            {
                PlayerInfo newPlayerInfo = LastGameState.PlayerInfoContainer[playerNumber].With(move);

                PlayerInfoContainer<PlayerInfo> newPlayerInfoContainer =
                    LastGameState.PlayerInfoContainer.With(playerNumber, newPlayerInfo);

                IGraph graph = GraphHelper.ToGraph(LastGameState.With(newPlayerInfoContainer), playerNumber);

                if (!_pathFinder.TryGetShortestPathLength(graph, out int pathLength))
                {
                    // todo

                    length = pathLength;

                    return possiblePawnMoves.First();
                }

                movePaths[move] = pathLength;
            }

            KeyValuePair<Coords, int> minMove = movePaths.OrderBy(path => path.Value).First();
            length = minMove.Value;

            return minMove.Key;
        }

        private WallCoords GetBestPlace(PlayerNumber playerNumber, out int length)
        {
            IEnumerable<WallCoords> possibleWallPlacements = OldRules.GetPossibleWallPlacements(LastGameState);

            var placePaths = new Dictionary<WallCoords, int>();

            foreach (WallCoords place in possibleWallPlacements)
            {
                List<WallCoords> newWallsList = LastGameState.Walls.ToList();
                newWallsList.Add(place);

                GameState newGameState = LastGameState.With(newWallsList);

                IGraph graph = GraphHelper.ToGraph(newGameState, playerNumber.Changed());

                if (!_pathFinder.TryGetShortestPathLength(graph, out int pathLength))
                {
                    // todo

                    length = pathLength;

                    return possibleWallPlacements.First();
                }

                placePaths[place] = pathLength;
            }

            KeyValuePair<WallCoords, int> maxPlace = placePaths.OrderByDescending(path => path.Value).First();

            length = maxPlace.Value;

            return maxPlace.Key;
        }

        public override Coords MovePawn(PlayerNumber playerNumber, IEnumerable<Coords> possibleMoves)
        {
            Coords result = _computedCoords ?? GetBestMove(playerNumber, out int _);

            _computedCoords = null;
            _computedWallCoords = null;

            return result;
        }

        public override WallCoords PlaceWall(PlayerNumber playerNumber, IEnumerable<WallCoords> possibleMoves)
        {
            WallCoords result = _computedWallCoords ?? GetBestPlace(playerNumber, out int _);

            _computedCoords = null;
            _computedWallCoords = null;

            return result;
        }
    }
}
