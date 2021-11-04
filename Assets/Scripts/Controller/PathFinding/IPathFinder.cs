using System.Net;

#nullable enable

namespace QuoridorDelta.Controller.PathFinding
{
    public interface IPathFinder
    {
        int GetShortestPathLength(in IGraph graph);
    }
}
