using PossibleRefactor.Model;

namespace PossibleRefactor.DataBaseManagementSystem
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