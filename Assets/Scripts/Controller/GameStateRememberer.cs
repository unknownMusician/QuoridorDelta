using System;
using JetBrains.Annotations;
using QuoridorDelta.DataBaseManagementSystem;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public abstract class GameStateRememberer : INotifiable
    {
        [NotNull] protected GameState LastGameState { get; private set; } = GameState.Empty;

        public void HandleChange(GameState gameState, IDBChangeInfo changeInfo) => LastGameState = gameState;
    }
}
