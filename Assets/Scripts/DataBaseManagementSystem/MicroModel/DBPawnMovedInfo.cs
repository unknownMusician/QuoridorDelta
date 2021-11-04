using QuoridorDelta.Model;

namespace QuoridorDelta.DataBaseManagementSystem
{
    public readonly struct DBPawnMovedInfo : IDBChangeInfo
    {
        public readonly PlayerNumber PlayerNumber;
        public readonly Coords NewCoords;
        public readonly bool IsJump;

        public DBPawnMovedInfo(PlayerNumber playerNumber, in Coords newCoords, bool isJump)
        {
            PlayerNumber = playerNumber;
            NewCoords = newCoords;
            IsJump = isJump;
        }
    }
}
