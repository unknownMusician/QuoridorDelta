#nullable enable

using System;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public sealed class Players
    {
        public readonly IPlayerInput First;
        public readonly IPlayerInput Second;

        public Players(IPlayerInput first, IPlayerInput second)
        {
            First = first;
            Second = second;
        }

        public IPlayerInput this[PlayerNumber playerNumber]
            => playerNumber switch
            {
                PlayerNumber.First => First,
                PlayerNumber.Second => Second,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}
