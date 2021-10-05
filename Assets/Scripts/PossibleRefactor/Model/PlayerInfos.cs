using System;

namespace PossibleRefactor.Model
{
    public readonly struct PlayerInfos
    {
        public readonly PlayerInfo First;
        public readonly PlayerInfo Second;
        
        public PlayerInfos(PlayerInfo first, PlayerInfo second)
        {
            First = first;
            Second = second;
        }

        public PlayerInfo this[PlayerNumber playerNumber] => playerNumber switch
        {
            PlayerNumber.First => First,
            PlayerNumber.Second => Second,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}