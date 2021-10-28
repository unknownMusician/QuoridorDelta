using System.Collections.Generic;
using QuoridorDelta.Controller;
using QuoridorDelta.Controller.Abstractions.View;
using QuoridorDelta.DataBaseManagementSystem;
using QuoridorDelta.Model;

namespace ViewConsole
{
    public static class ViewConsole
    {
        public static void Main()
        {
            new Game().Start(new GameInput(), new GameView());
        }
    }

    public class GameInput : IGameInput
    {
        public GameType ChooseGameType() => GameType.VersusBot;

        public MoveType ChooseMoveType(PlayerNumber playerNumber) => throw new System.NotImplementedException();

        public Coords MovePawn(PlayerNumber playerNumber, IEnumerable<Coords> possibleMoves) => throw new System.NotImplementedException();

        public WallCoords PlaceWall(PlayerNumber playerNumber, IEnumerable<WallCoords> possibleMoves) => throw new System.NotImplementedException();

        public bool ShouldRestart() => false;
    }

    public class GameView : IGameView
    {
        public void HandleChange(GameState gameState, IDBChangeInfo changeInfo) => throw new System.NotImplementedException();

        public void ShowWinner(PlayerNumber winner) => throw new System.NotImplementedException();

        public void ShowWrongMove(MoveType moveType) => throw new System.NotImplementedException();
    }
}