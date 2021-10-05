using PossibleRefactor.Model;

namespace PossibleRefactor.Controller
{
    public interface IPlayerInput
    {
        MoveType ChooseMoveType();
        Coords MovePawn();
        WallCoords PlaceWall();
    }
}