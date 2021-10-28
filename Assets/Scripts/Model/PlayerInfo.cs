#nullable enable

namespace QuoridorDelta.Model
{
    public readonly struct PlayerInfo : System.IEquatable<PlayerInfo>
    {
        public readonly Coords PawnCoords;
        public readonly int WallCount;

        public PlayerInfo(in Coords pawnCoords, int wallCount)
        {
            PawnCoords = pawnCoords;
            WallCount = wallCount;
        }

        public void Deconstruct(out Coords pawnCoords, out int wallCount)
        {
            pawnCoords = PawnCoords;
            wallCount = WallCount;
        }

        public static implicit operator PlayerInfo(in (Coords pawnCoords, int wallCount) tuple)
            => new PlayerInfo(tuple.pawnCoords, tuple.wallCount);

        public bool Equals(PlayerInfo other) => Equals(in other);

        public bool Equals(in PlayerInfo other)
            => other.PawnCoords == PawnCoords && other.WallCount == WallCount;

        public override bool Equals(object obj) => obj is PlayerInfo other && Equals(in other);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => $"PlayerInfo ({PawnCoords}, {WallCount})";

        public static bool operator ==(in PlayerInfo p1, in PlayerInfo p2) => p1.Equals(in p2);
        public static bool operator !=(in PlayerInfo p1, in PlayerInfo p2) => !p1.Equals(in p2);
    }
}
