using QuoridorDelta.DataBaseManagementSystem;

namespace QuoridorDelta.Controller
{
    public abstract class GameStateRememberer : INotifiable
    {
        protected GameState LastGameState { get; private set; }

        public void HandleChange(GameState gameState, IDBChangeInfo changeInfo) => LastGameState = gameState;
    }
}