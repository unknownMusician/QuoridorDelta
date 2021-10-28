#nullable enable

using QuoridorDelta.Model;

namespace QuoridorDelta.Controller.Abstractions.View
{
    public interface IGameView : INotifiable
    {
        void ShowWinner(PlayerNumber winner);
        void ShowWrongMove(MoveType moveType);
    }
}
