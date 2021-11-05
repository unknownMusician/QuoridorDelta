using System.Net;

namespace QuoridorDelta.Controller.PathFinding
{
    public interface IPathFinder
    {
        int GetShortestPathLength(in IGraph graph);
        bool TryGetShortestPathLength(in IGraph graph, out int length);
    }
}
