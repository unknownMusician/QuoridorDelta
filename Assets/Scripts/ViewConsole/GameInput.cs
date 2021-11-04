#nullable enable

using System;
using System.Collections.Generic;
using QuoridorDelta.Controller;
using QuoridorDelta.Controller.Abstractions.View;
using QuoridorDelta.Model;

namespace QuoridorDelta.ViewConsole
{
    public sealed class GameInput : IGameInput
    {
        private readonly Parser _parser = new Parser();
        private string? _command;

        public GameType ChooseGameType() => GameType.VersusBot;
        public PlayerNumber ChoosePlayerNumber() => throw new NotImplementedException();

        public MoveType ChooseMoveType(PlayerNumber playerNumber)
        {
            _command = Console.ReadLine() ?? "";

            return _parser.ParseStringToMoveType(_command);
        }

        public Coords MovePawn(PlayerNumber playerNumber, IEnumerable<Coords> possibleMoves)
        {
            return _parser.ParseStringToPawnCoords(_command!);
        }


        public WallCoords PlaceWall(PlayerNumber playerNumber, IEnumerable<WallCoords> possibleMoves)
        {
            return _parser.ParseStringToWallCoords(_command!);
        }

        public bool ShouldRestart() => false;
    }
}
