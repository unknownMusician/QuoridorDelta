using System;
using System.Collections.Generic;
using System.Linq;
using PathFinding;
using PathFinding.Model;

namespace QuoridorDelta.Controller
{
    public sealed class PathFinderAstar : IPathFinder
    {
        //private NodeDistanceToFinalComparer _nodeComparer = new NodeDistanceToFinalComparer();
        public int GetShortestPathLength(in IGraph graph)
        {
            var openSet = new List<NodeInfo>();
            var closedSet = new List<NodeInfo>();

            openSet.Add(new NodeInfo(graph.FirstNode, null, 0, GetHeuristicPathLength(graph.FirstNode)));

            while (openSet.Count > 0)
            {
                NodeInfo currentNodeInfo = openSet.OrderBy(nodeInfo => nodeInfo.HeuristicFunctionValue).First();

                if (currentNodeInfo.Node.IsFinal())
                    return currentNodeInfo.PathLengthToFirst;

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
                           GetHeuristicPathLength(neighbourNode)));
                    }
                    else if (currentNodeInfo.PathLengthToFirst + 1 < openNodeInfo.PathLengthToFirst)
                    {
                        openNodeInfo.ChangeNodeConnection(currentNodeInfo, currentNodeInfo.PathLengthToFirst + 1);
                    }
                }
            }
            throw new Exception("Program cannot find path to win");
        }

        private static int GetHeuristicPathLength(in INode from) => NodeHelper.FinalYCoord - from.Position.y;
    }
    public class NodeInfo
    {
        private NodeInfo _parent;
        private int _heuristicPathLength;

        public readonly INode Node;
        public int PathLengthToFirst { get; private set; }
        public int HeuristicFunctionValue => PathLengthToFirst + _heuristicPathLength;

        public NodeInfo(in INode node, NodeInfo parent, int g, int h)
        {
            Node = node;
            _parent = parent;
            PathLengthToFirst = g;
            _heuristicPathLength = h;
        }
        public void ChangeNodeConnection(NodeInfo newParent, int newG)
        {
            _parent = newParent;
            PathLengthToFirst = newG;
        }
    }
}
