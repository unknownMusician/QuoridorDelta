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
                PlayerNumber.White => First,
                PlayerNumber.Black => Second,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}
