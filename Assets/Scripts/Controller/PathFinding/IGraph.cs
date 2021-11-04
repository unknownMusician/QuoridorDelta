using System;
using System.Collections.Generic;

namespace QuoridorDelta.Controller.PathFinding
{
    public interface IGraph
    {
        INode[] Nodes { get; }
        INode FirstNode { get; }
        int LastNodeYCoord { get; }
    }

    public sealed class Tester<TPathFinder> where TPathFinder : IPathFinder, new()
    {
        public void Test()
        {
            IPathFinder finder = new TPathFinder();

            Node node1 = new Node((5, 7));
            Node node2 = new Node((6, 7));
            Node node3 = new Node((7, 7));
            Node node4 = new Node((7, 8));

            node1.Neighbors = new[] { node2, node4 };
            node2.Neighbors = new[] { node1, node3 };
            node3.Neighbors = new[] { node2, node4 };
            node4.Neighbors = new[] { node3 };

            IGraph graph = new Graph(new INode[] { node1, node2, node3, node4 }, node1, 8);

            int result = finder.GetShortestPathLength(graph);
            
            Console.WriteLine(result);
        }
    }
    
    public sealed class Node : INode
    {
        public IEnumerable<INode> Neighbors { get; set; }

        public (int x, int y) Position { get; set; }
        
        public Node((int x, int y) position)
        {
            Position = position;
            Neighbors = Array.Empty<INode>();
        }

        public override string ToString() => $"Node ({Position.x}, {Position.y})";
    }
        
    public sealed class Graph : IGraph
    {
        public INode[] Nodes { get; }
        public INode FirstNode { get; }
        public int LastNodeYCoord { get; }

        public Graph(INode[] nodes, INode firstNode, int lastNodeYCoord)
        {
            Nodes = nodes;
            FirstNode = firstNode;
            LastNodeYCoord = lastNodeYCoord;
        }
    }

}
