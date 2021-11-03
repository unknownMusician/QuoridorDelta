#nullable enable

namespace QuoridorDelta.Controller.PathFinding
{
    public sealed class NodeInfo
    {
        private NodeInfo? _parent;
        private readonly int _heuristicPathLength;

        public readonly INode Node;
        public int PathLengthToFirst { get; private set; }
        public int HeuristicFunctionValue => PathLengthToFirst + _heuristicPathLength;

        public NodeInfo(in INode node, NodeInfo? parent, int g, int h)
        {
            Node = node;
            _parent = parent;
            PathLengthToFirst = g;
            _heuristicPathLength = h;
        }
        
        public void ChangeNodeConnection(NodeInfo? newParent, int newG)
        {
            _parent = newParent;
            PathLengthToFirst = newG;
        }
    }
}
