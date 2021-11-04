#nullable enable

using System;
using QuoridorDelta.Controller.Abstractions.View;
using QuoridorDelta.DataBaseManagementSystem;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public static class PlayersFactory
    {
        public static Players CreatePlayers(
            GameType gameType, PlayerNumber humanNumber, IGameInput input, INotifiable view, ref Action<GameState, IDBChangeInfo>? onDBChange
        )
        {
            IPlayerInput player1 = new HumanPlayer(input, view, ref onDBChange);
            IPlayerInput player2 = PlayersFactory.CreateSecondPlayer(gameType, input, ref onDBChange);

            return humanNumber switch
            {
                PlayerNumber.First => new Players(player1, player2),
                PlayerNumber.Second => new Players(player2, player1),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static IPlayerInput CreateSecondPlayer(
            GameType gameType, IGameInput input, ref Action<GameState, IDBChangeInfo>? onDBChange
        )
            => gameType switch
            {
                GameType.VersusPlayer => new HumanPlayer(input),
                GameType.VersusBot => new IntelligentBot(ref onDBChange),
                _ => throw new ArgumentOutOfRangeException(nameof(gameType), gameType, null)
            };
    }
}
