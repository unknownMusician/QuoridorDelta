#nullable enable

namespace QuoridorDelta.Controller.PathFinding
{
    public static class NodeHelper
    {
        public const int FinalYCoord = 8;

        public static bool IsFinal(this INode node) => node.Position.y == NodeHelper.FinalYCoord;
    }
}
