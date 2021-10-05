using PossibleRefactor.DataBaseManagementSystem;

namespace PossibleRefactor.Controller
{
    public interface INotifiable
    {
        void HandleChange(GameState gameState, IDBChangeInfo changeInfo);
    }
}