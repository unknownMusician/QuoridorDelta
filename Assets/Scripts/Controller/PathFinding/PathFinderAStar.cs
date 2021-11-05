using System;
using System.Collections.Generic;
using System.Linq;

namespace QuoridorDelta.Controller.PathFinding
{
    public sealed class PathFinderAStar : IPathFinder
    {
        public int GetShortestPathLength(in IGraph graph)
        {
            if (!TryGetShortestPathLength(graph, out int length))
            {
                throw new Exception("Program cannot find path to win");
            }

            return length;
        }

        public bool TryGetShortestPathLength(in IGraph graph, out int length)
        {
            var openSet = new List<NodeInfo>();
            var closedSet = new List<NodeInfo>();

            openSet.Add(new NodeInfo(graph.FirstNode, 0, GetDirectPathLength(graph.FirstNode, graph)));

            return TryMainCycle(graph, openSet, closedSet, out length);
        }

        private bool TryMainCycle(in IGraph graph, List<NodeInfo> openSet, List<NodeInfo> closedSet, out int length)
        {
            while (openSet.Count > 0)
            {
                NodeInfo currentNodeInfo = openSet.OrderBy(nodeInfo => nodeInfo.HeuristicFunctionValue).First();

                if (graph.IsFinal(currentNodeInfo.Node))
                {
                    length = currentNodeInfo.PathLengthToFirst;

                    return true;
                }

                openSet.Remove(currentNodeInfo);
                closedSet.Add(currentNodeInfo);

                HandleNeighbours(openSet, closedSet, currentNodeInfo, graph);
            }

            length = int.MaxValue;

            return false;
        }

        private void HandleNeighbours(
            List<NodeInfo> openSet, List<NodeInfo> closedSet, NodeInfo currentNodeInfo, in IGraph graph
        )
        {
            foreach (var neighbourNode in currentNodeInfo.Node.Neighbors)
            {
                if (closedSet.Count(nodeInfo => nodeInfo.Node.Position == neighbourNode.Position) <= 0)
                {
                    TryAddNodeToOpenSet(openSet, currentNodeInfo, neighbourNode, graph);
                }
            }
        }

        private bool TryAddNodeToOpenSet(
            List<NodeInfo> openSet, NodeInfo parentNodeInfo, INode neighbourNode, in IGraph graph
        )
        {
            NodeInfo? openNodeInfo =
                openSet.FirstOrDefault(nodeInfo => nodeInfo.Node.Position == neighbourNode.Position);

            if (openNodeInfo == null)
            {
                openSet.Add(new NodeInfo(neighbourNode,
                                         parentNodeInfo.PathLengthToFirst + 1,
                                         GetDirectPathLength(neighbourNode, graph)));

                return true;
            }

            TryChangeNodeConnection(openNodeInfo, parentNodeInfo);

            return false;
        }

        private bool TryChangeNodeConnection(NodeInfo currentNodeInfo, NodeInfo newParentNodeInfo)
        {
            if (newParentNodeInfo.PathLengthToFirst + 1 < currentNodeInfo.PathLengthToFirst)
            {
                currentNodeInfo.ChangeNodeConnection(newParentNodeInfo.PathLengthToFirst + 1);

                return true;
            }

            return false;
        }

        private int GetDirectPathLength(in INode from, in IGraph graph) => graph.LastNodeYCoord - from.Position.y;
    }
}
