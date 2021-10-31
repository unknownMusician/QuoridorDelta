#nullable enable

using PathFinding.Model;
using System.Collections.Generic;

namespace PathFinding
{
    namespace Model
    {
        public interface INode
        {
            INode[] Neighbors { get; }
            (int x, int y) Position { get; }
        }

        public interface IGraph
        {
            INode[] Nodes { get; }
            INode FirstNode { get; }
        }

        public static class NodeHelper
        {
            public const int FinalYCoord = 8;
            public static bool IsFinal(this INode node)
            {
                return node.Position.y == FinalYCoord;
            }
        }

        // refactor: remove
        public sealed class NodeDistanceToFinalComparer : IComparer<INode>
        {
            public int Compare(INode node1, INode node2)
            {
                return node1.Position.y.CompareTo(node2.Position.y);
            }
        }
    }

    public interface IPathFinder
    {
        int GetShortestPathLength(in IGraph graph);
    }
}