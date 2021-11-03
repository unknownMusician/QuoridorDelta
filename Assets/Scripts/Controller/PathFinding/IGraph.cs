using System;
using System.Collections.Generic;

#nullable enable

namespace QuoridorDelta.Controller.PathFinding
{
    public interface IGraph
    {
        INode[] Nodes { get; }
        INode FirstNode { get; }
    }

    public sealed class Tester<TPathFinder> where TPathFinder : IPathFinder, new()
    {
        public class Node : INode
        {
            public IEnumerable<INode> Neighbors { get; set; }
            public (int x, int y) Position { get; set; }
        }
        
        public class Graph : IGraph
        {
            public INode[] Nodes { get; }
            public INode FirstNode { get; }
            
            public Graph(INode[] nodes, INode firstNode)
            {
                Nodes = nodes;
                FirstNode = firstNode;
            }
        }
        
        public void Test()
        {
            IPathFinder finder = new TPathFinder();

            Node node1 = new Node();
            Node node2 = new Node();
            Node node3 = new Node();
            Node node4 = new Node();

            node1.Neighbors = new[] { node2, node4 };
            node1.Position = (5, 7);
            node2.Neighbors = new[] { node1, node3 };
            node2.Position = (6, 7);
            node3.Neighbors = new[] { node2, node4 };
            node3.Position = (7, 7);
            node4.Neighbors = new[] { node3 };
            node4.Position = (7, 8);

            IGraph graph = new Graph(new INode[] { node1, node2, node3, node4 }, node1);

            int result = finder.GetShortestPathLength(graph);
            
            Console.WriteLine(result);
        }
    }
}
