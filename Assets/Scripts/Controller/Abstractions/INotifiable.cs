using QuoridorDelta.DataBaseManagementSystem;

namespace QuoridorDelta.Controller
{
    public interface INotifiable
    {
        void HandleChange(GameState gameState, IDBChangeInfo changeInfo);
    }
}
