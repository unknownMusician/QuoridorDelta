using QuoridorDelta.Model;

namespace QuoridorDelta.DataBaseManagementSystem
{
    public readonly struct DBInitializedInfo : IDBChangeInfo
    {
        public readonly PlayerInfos PlayerInfos;

        public DBInitializedInfo(PlayerInfos playerInfos)
        {
            PlayerInfos = playerInfos;
        }
    }
}