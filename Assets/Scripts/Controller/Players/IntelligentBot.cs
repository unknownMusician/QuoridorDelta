using System;
using System.Collections.Generic;
using System.Linq;
using Dev;
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
            int currentBotPath =
                _pathFinder.GetShortestPathLength(GraphHelper.ToGraph(LastGameState, playerNumber));

            int currentEnemyPath =
                _pathFinder.GetShortestPathLength(GraphHelper.ToGraph(LastGameState, playerNumber.Changed()));

            _computedCoords = GetBestMove(playerNumber, out int moveLength);
            _computedWallCoords = GetBestPlace(playerNumber, out int placeLength);

            return moveLength - currentBotPath <= currentEnemyPath - placeLength ?
                MoveType.MovePawn :
                MoveType.PlaceWall;
        }

        private Coords GetBestMove(PlayerNumber playerNumber, out int length)
        {
            IEnumerable<Coords> possiblePawnMoves = OldRules.GetPossiblePawnMoves(playerNumber, LastGameState);

            var movePaths = new Dictionary<Coords, int>();

            foreach (Coords move in possiblePawnMoves)
            {
                PlayerInfo newPlayerInfo = (move, LastGameState.PlayerInfoContainer[playerNumber].WallCount);

                PlayerInfoContainer<PlayerInfo> newPlayerInfoContainer =
                    LastGameState.PlayerInfoContainer.With(playerNumber, newPlayerInfo);

                IGraph graph = GraphHelper.ToGraph(LastGameState.With(newPlayerInfoContainer), playerNumber);

                movePaths[move] = _pathFinder.GetShortestPathLength(graph);
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

                placePaths[place] = _pathFinder.GetShortestPathLength(graph);
            }

            KeyValuePair<WallCoords, int> maxPlace = placePaths.OrderByDescending(path => path.Value).First();

            length = maxPlace.Value;

            return maxPlace.Key;
        }

        public override Coords MovePawn(PlayerNumber playerNumber, IEnumerable<Coords> possibleMoves)
        {
            Coords result = _computedCoords ?? GetBestMove(playerNumber, out int _);

            _computedCoords = null;

            return result;
        }

        public override WallCoords PlaceWall(PlayerNumber playerNumber, IEnumerable<WallCoords> possibleMoves)
        {
            WallCoords result =  _computedWallCoords ?? GetBestPlace(playerNumber, out int _);

            _computedWallCoords = null;

            return result;
        }
    }
}
