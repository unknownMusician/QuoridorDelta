using PossibleRefactor.Model;

namespace PossibleRefactor.Controller
{
    public interface IGameView : INotifiable
    {
        void ShowWinner(PlayerNumber winner);
    }
}