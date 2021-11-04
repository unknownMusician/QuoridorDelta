using System.Collections.Generic;
using System.Linq;
using QuoridorDelta.DataBaseManagementSystem;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller.PathFinding
{
    public static class GraphHelper
    {
        public static IGraph ToGraph(GameState gameState, PlayerNumber playerNumber)
            => GraphHelper.ToGraph(gameState, gameState.PlayerInfoContainer[playerNumber].PawnCoords, playerNumber);

        public static IGraph ToGraph(GameState gameState, Coords startCoords, PlayerNumber playerNumber)
        {
            Dictionary<Coords, INode> nodes = GraphHelper.CreateNodes();

            for (int y = OldRules.MinCoords; y <= OldRules.MaxCoords; y++)
            {
                for (int x = OldRules.MinCoords; x <= OldRules.MaxCoords; x++)
                {
                    Coords position = (x, y);
                    Node node = (Node)nodes[position];

                    GameState testGameState =
                        gameState.With(gameState.PlayerInfoContainer.With(
                                           playerNumber,
                                           (position, gameState.PlayerInfoContainer[playerNumber].WallCount)));

                    IEnumerable<Coords> neighbors = OldRules.GetPossiblePawnMoves(playerNumber, testGameState);

                    node.Neighbors = neighbors.Select(neighborCoords => nodes[neighborCoords]).ToArray();
                }
            }

            return new Graph(nodes.Values.ToArray(), nodes[startCoords], OldRules.GetWinCoordsY(playerNumber));
        }

        private static Dictionary<Coords, INode> CreateNodes()
        {
            var nodes = new Dictionary<Coords, INode>();

            for (int y = OldRules.MinCoords; y <= OldRules.MaxCoords; y++)
            {
                for (int x = OldRules.MinCoords; x <= OldRules.MaxCoords; x++)
                {
                    var position = (x, y);

                    nodes[position] = new Node(position);
                }
            }

            return nodes;
        }
    }
}
