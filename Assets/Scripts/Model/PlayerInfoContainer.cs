#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;

namespace QuoridorDelta.Model
{
    public readonly struct PlayerInfoContainer<TInfo> : IEnumerable<TInfo>
    {
        public readonly TInfo First;
        public readonly TInfo Second;

        public (PlayerNumber number, TInfo element)[] Pairs
            => new[] { (PlayerNumber.First, First), (PlayerNumber.Second, Second) };

        public PlayerInfoContainer(in TInfo first, in TInfo second)
        {
            First = first;
            Second = second;
        }

        public void Deconstruct(out TInfo first, out TInfo second)
        {
            first = First;
            second = Second;
        }

        public static implicit operator PlayerInfoContainer<TInfo>(in (TInfo first, TInfo second) tuple)
            => new PlayerInfoContainer<TInfo>(tuple.first, tuple.second);

        public bool Equals(PlayerInfoContainer<TInfo> other) => Equals(in other);

        public bool Equals(in PlayerInfoContainer<TInfo> other)
            => other.First!.Equals(First) && other.Second!.Equals(Second);

        public IEnumerator<TInfo> GetEnumerator()
        {
            yield return First;
            yield return Second;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override bool Equals(object obj) => obj is PlayerInfoContainer<TInfo> other && Equals(in other);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => $"PlayerInfos ({First}, {Second})";

        public static bool operator ==(in PlayerInfoContainer<TInfo> p1, in PlayerInfoContainer<TInfo> p2)
            => p1.Equals(in p2);

        public static bool operator !=(in PlayerInfoContainer<TInfo> p1, in PlayerInfoContainer<TInfo> p2)
            => !p1.Equals(in p2);

        public TInfo this[PlayerNumber playerNumber]
            => playerNumber switch
            {
                PlayerNumber.First => First,
                PlayerNumber.Second => Second,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}
