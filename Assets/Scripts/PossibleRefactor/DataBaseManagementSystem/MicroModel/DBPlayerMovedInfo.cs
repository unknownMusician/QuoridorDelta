using PossibleRefactor.Model;

namespace PossibleRefactor.DataBaseManagementSystem
{
    public readonly struct DBPlayerMovedInfo : IDBChangeInfo
    {
        public readonly PlayerNumber PlayerNumber;
        public readonly Coords NewCoords;

        public DBPlayerMovedInfo(PlayerNumber playerNumber, Coords newCoords)
        {
            PlayerNumber = playerNumber;
            NewCoords = newCoords;
        }
    }
}