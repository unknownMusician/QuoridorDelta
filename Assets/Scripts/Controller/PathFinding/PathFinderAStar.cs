#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace QuoridorDelta.Controller.PathFinding
{
    public sealed class PathFinderAStar : IPathFinder
    {
        //private NodeDistanceToFinalComparer _nodeComparer = new NodeDistanceToFinalComparer();
        public int GetShortestPathLength(in IGraph graph)
        {
            var openSet = new List<NodeInfo>();
            var closedSet = new List<NodeInfo>();

            openSet.Add(new NodeInfo(graph.FirstNode, null, 0, GetHeuristicPathLength(graph.FirstNode, graph)));

            while (openSet.Count > 0)
            {
                NodeInfo currentNodeInfo = openSet.OrderBy(nodeInfo => nodeInfo.HeuristicFunctionValue).First();

                if (graph.IsFinal(currentNodeInfo.Node))
                {
                    return currentNodeInfo.PathLengthToFirst;
                }

                openSet.Remove(currentNodeInfo);
                closedSet.Add(currentNodeInfo);
                
                foreach (var neighbourNode in currentNodeInfo.Node.Neighbors)
                {
                    if (closedSet.Count(nodeInfo => nodeInfo.Node.Position == neighbourNode.Position) > 0)
                    {
                        continue;
                    }
                    
                    NodeInfo openNodeInfo = openSet.FirstOrDefault(nodeInfo => nodeInfo.Node.Position == neighbourNode.Position);

                    if (openNodeInfo == null)
                    {
                        openSet.Add(new NodeInfo(
                           neighbourNode,
                           currentNodeInfo,
                           currentNodeInfo.PathLengthToFirst + 1,
                           GetHeuristicPathLength(neighbourNode, graph)));
                    }
                    else if (currentNodeInfo.PathLengthToFirst + 1 < openNodeInfo.PathLengthToFirst)
                    {
                        openNodeInfo.ChangeNodeConnection(currentNodeInfo, currentNodeInfo.PathLengthToFirst + 1);
                    }
                }
            }
            
            throw new Exception("Program cannot find path to win");
        }

        private static int GetHeuristicPathLength(in INode from, in IGraph graph) => graph.LastNodeYCoord - from.Position.y;
    }
}
