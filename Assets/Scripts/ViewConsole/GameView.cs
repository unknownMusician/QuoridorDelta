#nullable enable

using System;
using QuoridorDelta.Controller;
using QuoridorDelta.Controller.Abstractions.View;
using QuoridorDelta.DataBaseManagementSystem;
using QuoridorDelta.Model;

namespace QuoridorDelta.ViewConsole
{
    public sealed class GameView : IGameView
    {
        private readonly Parser _parser = new Parser();

        public void HandleChange(GameState gameState, IDBChangeInfo changeInfo)
        {
            switch (changeInfo)
            {
                case DBPawnMovedInfo dbPawnMovedInfo:
                    Console.WriteLine(_parser.ParseNewCoordsAndMoveTypeToCommandString(dbPawnMovedInfo.NewCoords));

                    break;
                case DBWallPlacedInfo dbWallPlacedInfo:
                    Console.WriteLine(_parser.ParseNewCoordsAndMoveTypeToCommandString(dbWallPlacedInfo.NewCoords));

                    break;
            }
        }

        public void ShowWinner(PlayerNumber winner) => throw new NotImplementedException();

        public void ShowWrongMove(MoveType moveType) => Console.WriteLine("Wrong move");
    }
}
