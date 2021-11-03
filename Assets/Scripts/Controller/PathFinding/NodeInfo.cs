#nullable enable

namespace QuoridorDelta.Controller.PathFinding
{
    public sealed class NodeInfo
    {
        private NodeInfo? _parent;
        private readonly int _directPathLength;

        public readonly INode Node;
        public int PathLengthToFirst { get; private set; }
        public int HeuristicFunctionValue => PathLengthToFirst + _directPathLength;

        public NodeInfo(in INode node, NodeInfo? parent, int pathLengthToFirst, int directPathLength)
        {
            Node = node;
            _parent = parent;
            PathLengthToFirst = pathLengthToFirst;
            _directPathLength = directPathLength;
        }
        
        public void ChangeNodeConnection(NodeInfo? newParent, int newG)
        {
            _parent = newParent;
            PathLengthToFirst = newG;
        }
    }
}
