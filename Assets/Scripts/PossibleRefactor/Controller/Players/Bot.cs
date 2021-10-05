using PossibleRefactor.Model;

namespace PossibleRefactor.Controller
{
    public abstract class Bot : GameStateRememberer, IPlayerInput
    {
        public abstract MoveType ChooseMoveType();

        public abstract Coords MovePawn();

        public abstract WallCoords PlaceWall();
    }
}