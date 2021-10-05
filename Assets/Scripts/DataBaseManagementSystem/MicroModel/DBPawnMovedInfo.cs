using QuoridorDelta.Model;

namespace QuoridorDelta.DataBaseManagementSystem
{
    public readonly struct DBPawnMovedInfo : IDBChangeInfo
    {
        public readonly PlayerNumber PlayerNumber;
        public readonly Coords NewCoords;

        public DBPawnMovedInfo(PlayerNumber playerNumber, Coords newCoords)
        {
            PlayerNumber = playerNumber;
            NewCoords = newCoords;
        }
    }
}