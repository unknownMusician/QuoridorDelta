using System;

namespace QuoridorDelta.Model
{
    public readonly struct PlayerInfos : IEquatable<PlayerInfos>
    {
        public readonly PlayerInfo First;
        public readonly PlayerInfo Second;

        public PlayerInfos(PlayerInfo first, PlayerInfo second)
        {
            First = first;
            Second = second;
        }

        public void Deconstruct(out PlayerInfo first, out PlayerInfo second)
        {
            first = First;
            second = Second;
        }

        public static implicit operator PlayerInfos((PlayerInfo first, PlayerInfo second) tuple)
            => new PlayerInfos(tuple.first, tuple.second);

        public bool Equals(PlayerInfos other)
            => other.First == First && other.Second == Second;

        public override bool Equals(object obj) => obj is PlayerInfos other && Equals(other);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => $"PlayerInfos ({First}, {Second})";

        public static bool operator ==(PlayerInfos p1, PlayerInfos p2) => p1.Equals(p2);
        public static bool operator !=(PlayerInfos p1, PlayerInfos p2) => !p1.Equals(p2);

        public PlayerInfo this[PlayerNumber playerNumber] => playerNumber switch
        {
            PlayerNumber.First => First,
            PlayerNumber.Second => Second,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
