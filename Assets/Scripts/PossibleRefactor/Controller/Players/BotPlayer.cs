using System;
using JetBrains.Annotations;
using PossibleRefactor.DataBaseManagementSystem;
using PossibleRefactor.Model;

namespace PossibleRefactor.Controller
{
    public sealed class BotPlayer : IPlayerInput
    {
        [NotNull] private readonly Bot _bot;

        public BotPlayer([NotNull] Bot bot, ref Action<GameState, IDBChangeInfo> onDBChange)
        {
            _bot = bot;
            onDBChange += _bot.HandleChange;
        }

        public MoveType ChooseMoveType() => _bot.ChooseMoveType();
        public Coords MovePawn() => _bot.MovePawn();
        public WallCoords PlaceWall() => _bot.PlaceWall();
    }
}