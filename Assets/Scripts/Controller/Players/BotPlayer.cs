using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuoridorDelta.DataBaseManagementSystem;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public sealed class BotPlayer : IPlayerInput
    {
        [NotNull] private readonly Bot _bot;

        public BotPlayer([NotNull] Bot bot, ref Action<GameState, IDBChangeInfo> onDBChange)
        {
            _bot = bot;
            onDBChange += _bot.HandleChange;
        }

        public MoveType ChooseMoveType(PlayerNumber playerNumber) => _bot.ChooseMoveType(playerNumber);

        public Coords MovePawn(PlayerNumber playerNumber, IEnumerable<Coords> possibleMoves)
            => _bot.MovePawn(playerNumber, possibleMoves);

        public WallCoords PlaceWall(PlayerNumber playerNumber, IEnumerable<WallCoords> possibleMoves)
            => _bot.PlaceWall(playerNumber, possibleMoves);
    }
}
