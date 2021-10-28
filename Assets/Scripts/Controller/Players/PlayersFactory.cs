#nullable enable

using System;
using QuoridorDelta.Controller.Abstractions.View;
using QuoridorDelta.DataBaseManagementSystem;

namespace QuoridorDelta.Controller
{
    public static class PlayersFactory
    {
        public static Players CreatePlayers(
            GameType gameType, IGameInput input, INotifiable view, ref Action<GameState, IDBChangeInfo>? onDBChange
        )
        {
            IPlayerInput player1 = new HumanPlayer(input, view, ref onDBChange);
            IPlayerInput player2 = PlayersFactory.CreateSecondPlayer(gameType, input, ref onDBChange);

            return new Players(player1, player2);
        }

        private static IPlayerInput CreateSecondPlayer(
            GameType gameType, IGameInput input, ref Action<GameState, IDBChangeInfo>? onDBChange
        )
            => gameType switch
            {
                GameType.VersusPlayer => new HumanPlayer(input),
                GameType.VersusBot => new RandomBot(ref onDBChange),
                _ => throw new ArgumentOutOfRangeException(nameof(gameType), gameType, null)
            };
    }
}
