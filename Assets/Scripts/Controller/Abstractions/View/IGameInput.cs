namespace QuoridorDelta.Controller.Abstractions.View
{
    public interface IGameInput : IPlayerInput
    {
        GameType ChooseGameType();
        bool ShouldRestart();
    }
}
