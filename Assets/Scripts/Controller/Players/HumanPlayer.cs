using System;
using System.Collections.Generic;
using QuoridorDelta.Controller.Abstractions.View;
using QuoridorDelta.DataBaseManagementSystem;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public sealed class HumanPlayer : IPlayerInput
    {
        private readonly IGameInput _input;

        public HumanPlayer(
            IGameInput input,
            INotifiable view,
            ref Action<GameState, IDBChangeInfo>? onDBChange
        ) : this(input)
            => onDBChange += view.HandleChange;

        public HumanPlayer(IGameInput input) => _input = input;

        public MoveType ChooseMoveType(PlayerNumber playerNumber) => _input.ChooseMoveType(playerNumber);

        public Coords MovePawn(PlayerNumber playerNumber, IEnumerable<Coords> possibleMoves)
            => _input.MovePawn(playerNumber, possibleMoves);

        public WallCoords PlaceWall(PlayerNumber playerNumber, IEnumerable<WallCoords> possibleMoves)
            => _input.PlaceWall(playerNumber, possibleMoves);
    }
}
