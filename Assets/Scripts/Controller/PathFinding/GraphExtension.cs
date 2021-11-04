#nullable enable

namespace QuoridorDelta.Controller.PathFinding
{
    public static class GraphExtension
    {
        public static bool IsFinal(this IGraph graph, in INode node) => node.Position.y == graph.LastNodeYCoord;
    }
}
