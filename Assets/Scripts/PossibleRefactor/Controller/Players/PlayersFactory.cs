using System;
using JetBrains.Annotations;
using PossibleRefactor.DataBaseManagementSystem;

namespace PossibleRefactor.Controller
{
    public static class PlayersFactory
    {
        [NotNull]
        public static Players CreatePlayers(GameType gameType,
                                            [NotNull] IGameInput input,
                                            [NotNull] INotifiable view,
                                            ref Action<GameState, IDBChangeInfo> onDBChange)
        {
            IPlayerInput player1 = new HumanPlayer(input, view, ref onDBChange);
            IPlayerInput player2 = CreateSecondPlayer(gameType, input, ref onDBChange);

            return new Players(player1, player2);
        }

        [NotNull]
        private static IPlayerInput CreateSecondPlayer(GameType gameType,
                                                       [NotNull] IGameInput input,
                                                       ref Action<GameState, IDBChangeInfo> onDBChange
        ) => gameType switch
        {
            GameType.VersusPlayer => new HumanPlayer(input),
            GameType.VersusBot => new BotPlayer(new RandomBot(), ref onDBChange),
            _ => throw new ArgumentOutOfRangeException(nameof(gameType), gameType, null)
        };
    }
}