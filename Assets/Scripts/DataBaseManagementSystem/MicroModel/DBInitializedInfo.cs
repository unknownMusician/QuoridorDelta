using QuoridorDelta.Model;

namespace QuoridorDelta.DataBaseManagementSystem
{
    public readonly struct DBInitializedInfo : IDBChangeInfo
    {
        public readonly PlayerInfoContainer<PlayerInfo> PlayerInfoContainer;

        public DBInitializedInfo(PlayerInfoContainer<PlayerInfo> playerInfos) => PlayerInfoContainer = playerInfos;
    }
}
