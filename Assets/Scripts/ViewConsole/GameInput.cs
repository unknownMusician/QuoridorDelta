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
        private string? _command = null;
        internal PlayerNumber BotNumber { get; private set; }

        public GameType ChooseGameType() => GameType.VersusBot;
        
        public PlayerNumber ChoosePlayerNumber()
        {
            ReadCommandIfNull();

            BotNumber = _parser.ParsePlayerNumber(_command!);
            _command = null;

            return BotNumber.Changed();
        }

        public MoveType ChooseMoveType(PlayerNumber playerNumber)
        {
            ReadCommandIfNull();

            return _parser.ParseMoveType(_command!);
        }

        public Coords MovePawn(PlayerNumber playerNumber, IEnumerable<Coords> possibleMoves)
        {
            ReadCommandIfNull();

            Coords result = _parser.ParsePawnCoords(_command!);

            _command = null;

            return result;
        }


        public WallCoords PlaceWall(PlayerNumber playerNumber, IEnumerable<WallCoords> possibleMoves)
        {
            ReadCommandIfNull();

            WallCoords result = _parser.ParseStringToWallCoords(_command!);

            _command = null;

            return result;
        }

        public bool ShouldRestart() => false;

        private string ReadCommandIfNull() => _command ??= (Console.ReadLine() ?? "");
    }
}
