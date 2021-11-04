using QuoridorDelta.Model;

#nullable enable

namespace QuoridorDelta.Controller.Abstractions.View
{
    public interface IGameInput : IPlayerInput
    {
        GameType ChooseGameType();
        PlayerNumber ChoosePlayerNumber();
        bool ShouldRestart();
    }
}
