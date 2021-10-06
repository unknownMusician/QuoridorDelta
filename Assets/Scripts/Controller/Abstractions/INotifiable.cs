using JetBrains.Annotations;
using QuoridorDelta.DataBaseManagementSystem;

namespace QuoridorDelta.Controller
{
    public interface INotifiable
    {
        void HandleChange([NotNull] GameState gameState, IDBChangeInfo changeInfo);
    }
}
