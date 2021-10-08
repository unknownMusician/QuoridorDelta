using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public static class InitialRules
    {
        public const int WallCountPerPlayer = 10;
        public static readonly Coords Player1Coords = new Coords(4, 0);
        public static readonly Coords Player2Coords = new Coords(4, 8);

        public static PlayerInfoContainer<PlayerInfo> PlayerInfoContainer
            => new PlayerInfoContainer<PlayerInfo>(new PlayerInfo(Player1Coords, WallCountPerPlayer),
                                                   new PlayerInfo(Player2Coords, WallCountPerPlayer));
    }
}
