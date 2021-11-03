#nullable enable

namespace QuoridorDelta.Controller.PathFinding
{
    public interface IGraph
    {
        INode[] Nodes { get; }
        INode FirstNode { get; }
    }

    public sealed class Test<TPathFinder> where TPathFinder : IPathFinder, new()
    {
        public class Graph : IGraph
        {
            
        }
        
        public void Test()
        {
            IPathFinder finder = new TPathFinder();
            
            
            finder.GetShortestPathLength()
        }
    }
}
