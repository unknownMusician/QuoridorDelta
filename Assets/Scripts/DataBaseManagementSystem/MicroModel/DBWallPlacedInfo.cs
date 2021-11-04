using QuoridorDelta.Model;

namespace QuoridorDelta.DataBaseManagementSystem
{
    public readonly struct DBWallPlacedInfo : IDBChangeInfo
    {
        public readonly PlayerNumber PlayerNumber;
        public readonly WallCoords NewCoords;

        public DBWallPlacedInfo(PlayerNumber playerNumber, in WallCoords newCoords)
        {
            PlayerNumber = playerNumber;
            NewCoords = newCoords;
        }
    }
}
