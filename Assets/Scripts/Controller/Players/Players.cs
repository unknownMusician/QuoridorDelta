using System;
using JetBrains.Annotations;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public class Players
    {
        [NotNull] public readonly IPlayerInput First;
        [NotNull] public readonly IPlayerInput Second;

        public Players([NotNull] IPlayerInput first, [NotNull] IPlayerInput second)
        {
            First = first;
            Second = second;
        }

        [NotNull]
        public IPlayerInput this[PlayerNumber playerNumber] => playerNumber switch
        {
            PlayerNumber.First => First,
            PlayerNumber.Second => Second,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}