#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace QuoridorDelta.Controller.PathFinding
{
    public sealed class PathFinderAStar : IPathFinder
    {
        public int GetShortestPathLength(in IGraph graph)
        {
            var openSet = new List<NodeInfo>();
            var closedSet = new List<NodeInfo>();

            openSet.Add(new NodeInfo(graph.FirstNode, null, 0, GetDirectPathLength(graph.FirstNode)));
            return MainCycle(openSet, closedSet);
        }

        private int MainCycle(List<NodeInfo> openSet, List<NodeInfo> closedSet)
        {
            while (openSet.Count > 0)
            {
                NodeInfo currentNodeInfo = openSet.OrderBy(nodeInfo => nodeInfo.HeuristicFunctionValue).First();

                if (currentNodeInfo.Node.IsFinal())
                {
                    return currentNodeInfo.PathLengthToFirst;
                }

                openSet.Remove(currentNodeInfo);
                closedSet.Add(currentNodeInfo);

                HandleNeighbours(openSet, closedSet, currentNodeInfo);

            }
            
            throw new Exception("Program cannot find path to win");
        }

        private void HandleNeighbours(List<NodeInfo> openSet, List<NodeInfo> closedSet, NodeInfo currentNodeInfo)
        {
            foreach (var neighbourNode in currentNodeInfo.Node.Neighbors)
            {
                if (closedSet.Count(nodeInfo => nodeInfo.Node.Position == neighbourNode.Position) <= 0)
                {
                    TryAddNodeToOpenSet(openSet, currentNodeInfo, neighbourNode);
                }
            }
        }
        
        private bool TryAddNodeToOpenSet(List<NodeInfo> openSet, NodeInfo parentNodeInfo, INode neighbourNode)
        {
            NodeInfo openNodeInfo = openSet.FirstOrDefault(nodeInfo => nodeInfo.Node.Position == neighbourNode.Position);

            if (openNodeInfo == null)
            {
                openSet.Add(new NodeInfo(
                   neighbourNode,
                   parentNodeInfo,
                   parentNodeInfo.PathLengthToFirst + 1,
                   GetDirectPathLength(neighbourNode)));
                return true;
            }
            TryChangeNodeConnection(openNodeInfo, parentNodeInfo);
            return false;
        }
        
        private bool TryChangeNodeConnection(NodeInfo currentNodeInfo, NodeInfo newParentNodeInfo)
        {
            if (newParentNodeInfo.PathLengthToFirst + 1 < currentNodeInfo.PathLengthToFirst)
            {
                currentNodeInfo.ChangeNodeConnection(newParentNodeInfo, newParentNodeInfo.PathLengthToFirst + 1);
                return true;
            }
            return false;
        }

        private static int GetDirectPathLength(in INode from) => NodeHelper.FinalYCoord - from.Position.y;
    }
}
