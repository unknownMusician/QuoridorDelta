using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public static class InitialRules
    {
        public const int WallCountPerPlayer = 10;
        public static readonly Coords Player1Coords = (4, 0);
        public static readonly Coords Player2Coords = (4, 8);

        public static PlayerInfoContainer<PlayerInfo> PlayerInfoContainer
            => ((Player1Coords, WallCountPerPlayer), (Player2Coords, WallCountPerPlayer));
    }
}
