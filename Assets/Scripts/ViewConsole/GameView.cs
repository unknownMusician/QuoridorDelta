using System;
using QuoridorDelta.Controller;
using QuoridorDelta.Controller.Abstractions.View;
using QuoridorDelta.DataBaseManagementSystem;
using QuoridorDelta.Model;

namespace QuoridorDelta.ViewConsole
{
    public sealed class GameView : IGameView
    {
        private readonly GameInput _gameInput;
        private readonly Parser _parser = new Parser();

        public GameView(GameInput gameInput)
        {
            _gameInput = gameInput;
        }

        public void HandleChange(GameState gameState, IDBChangeInfo changeInfo)
        {
            switch (changeInfo)
            {
                case DBPawnMovedInfo dbPawnMovedInfo:
                    TryOutputPawnMove(dbPawnMovedInfo);

                    break;
                case DBWallPlacedInfo dbWallPlacedInfo:
                    TryOutputWallPlace(dbWallPlacedInfo);

                    break;
            }
        }

        private void TryOutputPawnMove(DBPawnMovedInfo dbPawnMovedInfo)
        {
            if (dbPawnMovedInfo.PlayerNumber == _gameInput.BotNumber)
            {
                Console.WriteLine(_parser.ParseNewCoordsAndMoveTypeToCommandString(dbPawnMovedInfo.NewCoords, dbPawnMovedInfo.IsJump));
            }
        }

        private void TryOutputWallPlace(DBWallPlacedInfo dbWallPlacedInfo)
        {
            if (dbWallPlacedInfo.PlayerNumber == _gameInput.BotNumber)
            {
                Console.WriteLine(_parser.ParseNewCoordsAndMoveTypeToCommandString(dbWallPlacedInfo.NewCoords));
            }
        }

        public void ShowWinner(PlayerNumber winner) { }

        public void ShowWrongMove(MoveType moveType) => Console.WriteLine("Delta: Wrong move");
    }
}
