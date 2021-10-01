using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public interface IView
    {
        MoveType GetMoveType(PlayerType playerType);
        Coords GetMovePawnCoords(PlayerType playerType);
        WallCoords GetPlaceWallCoords(PlayerType playerType);
    }
}