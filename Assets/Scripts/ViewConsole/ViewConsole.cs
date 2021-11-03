using System;
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
            /*Tester tester = new Tester();
            tester.test();*/
            new Game().Start(new GameInput(), new GameView());
        }
    }

    public class GameInput : IGameInput
    {
        private Parser _parser = new Parser();
        private String _command;
        
        public GameType ChooseGameType() => GameType.VersusBot;

        public MoveType ChooseMoveType(PlayerNumber playerNumber)
        {
            _command = Console.ReadLine();
            return _parser.ParseStringToMoveType(_command);
        }

        public Coords MovePawn(PlayerNumber playerNumber, IEnumerable<Coords> possibleMoves)
        {
            return _parser.ParseStringToPawnCoords(_command);
        }
        

        public WallCoords PlaceWall(PlayerNumber playerNumber, IEnumerable<WallCoords> possibleMoves)
        {
            return _parser.ParseStringToWallCoords(_command);
        }

        public bool ShouldRestart() => false;

        
        
    }

    public class GameView : IGameView
    {
        private Parser _parser = new Parser();
        public void HandleChange(GameState gameState, IDBChangeInfo changeInfo)
        {
            switch (changeInfo)
            {
                case DBPawnMovedInfo dbPawnMovedInfo:
                    Console.WriteLine(_parser.ParseNewCoordsAndMoveTypeToCommandString
                        (dbPawnMovedInfo.NewCoords));
                    break;
                case DBWallPlacedInfo dbWallPlacedInfo:
                    Console.WriteLine(_parser.ParseNewCoordsAndMoveTypeToCommandString
                        (dbWallPlacedInfo.NewCoords));
                    break;
            }
            
            
            
        }

        public void ShowWinner(PlayerNumber winner) => throw new System.NotImplementedException();

        public void ShowWrongMove(MoveType moveType) => Console.WriteLine("Wrong move");
    }
}