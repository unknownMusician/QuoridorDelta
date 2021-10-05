namespace PossibleRefactor.Controller
{
    public interface IGameInput : IPlayerInput
    {
        GameType ChooseGameType();
        bool ShouldRestart();
    }
}