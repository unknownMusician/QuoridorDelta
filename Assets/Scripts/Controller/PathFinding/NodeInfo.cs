#nullable enable

namespace QuoridorDelta.Controller.PathFinding
{
    public sealed class NodeInfo
    {
        private readonly int _directPathLength;

        public readonly INode Node;
        public int PathLengthToFirst { get; private set; }
        public int HeuristicFunctionValue => PathLengthToFirst + _directPathLength;

        public NodeInfo(in INode node, int pathLengthToFirst, int directPathLength)
        {
            Node = node;
            PathLengthToFirst = pathLengthToFirst;
            _directPathLength = directPathLength;
        }

        public void ChangeNodeConnection(int newLengthToFirst) => PathLengthToFirst = newLengthToFirst;
    }
}
