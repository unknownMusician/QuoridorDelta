using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public interface IView : IInput
    {
        GameType GetGameType();

        void MovePlayerPawn(PlayerType playerType, Coords newCoords);
        void PlacePlayerWall(PlayerType playerType, WallCoords newCoords);
        void ShowWrongMove(PlayerType playerType, MoveType moveType);

        void ShowWinner(PlayerType playerType);
        bool ShouldRestart();
    }
}