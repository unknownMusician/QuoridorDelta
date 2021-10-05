namespace QuoridorDelta.Model
{
    public readonly struct PlayerInfo : System.IEquatable<PlayerInfo>
    {
        public readonly Coords PawnCoords;
        public readonly int WallCount;

        public PlayerInfo(Coords pawnCoords, int wallCount)
        {
            PawnCoords = pawnCoords;
            WallCount = wallCount;
        }

        public void Deconstruct(out Coords pawnCoords, out int wallCount)
        {
            pawnCoords = PawnCoords;
            wallCount = WallCount;
        }

        public static implicit operator PlayerInfo((Coords pawnCoords, int wallCount) tuple)
            => new PlayerInfo(tuple.pawnCoords, tuple.wallCount);

        public bool Equals(PlayerInfo other)
            => other.PawnCoords == PawnCoords && other.WallCount == WallCount;

        public override bool Equals(object obj) => obj is PlayerInfo other && Equals(other);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => $"PlayerInfo ({PawnCoords}, {WallCount})";

        public static bool operator ==(PlayerInfo p1, PlayerInfo p2) => p1.Equals(p2);
        public static bool operator !=(PlayerInfo p1, PlayerInfo p2) => !p1.Equals(p2);
    }
}