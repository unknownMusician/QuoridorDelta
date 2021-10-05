using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuoridorDelta.Controller.Abstractions.View;
using QuoridorDelta.DataBaseManagementSystem;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public sealed class HumanPlayer : IPlayerInput
    {
        [NotNull] private readonly IGameInput _input;

        public HumanPlayer([NotNull] IGameInput input,
                           [NotNull] INotifiable view,
                           ref Action<GameState, IDBChangeInfo> onDBChange)
            : this(input) => onDBChange += view.HandleChange;

        public HumanPlayer([NotNull] IGameInput input) => _input = input;

        public MoveType ChooseMoveType(PlayerNumber playerNumber) => _input.ChooseMoveType(playerNumber);

        public Coords MovePawn(PlayerNumber playerNumber, IEnumerable<Coords> possibleMoves)
            => _input.MovePawn(playerNumber, possibleMoves);

        public WallCoords PlaceWall(PlayerNumber playerNumber, IEnumerable<WallCoords> possibleMoves)
            => _input.PlaceWall(playerNumber, possibleMoves);
    }
}