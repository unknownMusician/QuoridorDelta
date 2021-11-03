#nullable enable

using System.Collections.Generic;

namespace QuoridorDelta.Controller.PathFinding
{
    public interface INode
    {
        IEnumerable<INode> Neighbors { get; }
        (int x, int y) Position { get; }
    }
}
