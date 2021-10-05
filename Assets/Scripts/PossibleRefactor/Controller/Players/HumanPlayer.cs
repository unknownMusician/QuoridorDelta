using System;
using JetBrains.Annotations;
using PossibleRefactor.DataBaseManagementSystem;
using PossibleRefactor.Model;

namespace PossibleRefactor.Controller
{
    public sealed class HumanPlayer : IPlayerInput
    {
        [NotNull] private readonly IGameInput _input;

        public HumanPlayer([NotNull] IGameInput input,
                           [NotNull] INotifiable view,
                           ref Action<GameState, IDBChangeInfo> onDBChange)
            : this(input) => onDBChange += view.HandleChange;

        public HumanPlayer([NotNull] IGameInput input) => _input = input;

        public MoveType ChooseMoveType() => _input.ChooseMoveType();

        public WallCoords PlaceWall() => _input.PlaceWall();

        public Coords MovePawn() => _input.MovePawn();
    }
}