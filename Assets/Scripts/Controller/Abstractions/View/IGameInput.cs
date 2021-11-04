using QuoridorDelta.Model;

namespace QuoridorDelta.Controller.Abstractions.View
{
    public interface IGameInput : IPlayerInput
    {
        GameType ChooseGameType();
        PlayerNumber ChoosePlayerNumber();
        bool ShouldRestart();
    }
}
