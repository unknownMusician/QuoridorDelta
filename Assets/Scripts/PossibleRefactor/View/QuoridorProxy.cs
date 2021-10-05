using PossibleRefactor.Controller;
using PossibleRefactor.Model;

namespace PossibleRefactor.View
{
    public class QuoridorProxy : IGameInput
    {
        public MoveType ChooseMoveType() => throw new System.NotImplementedException();

        public Coords MovePawn() => throw new System.NotImplementedException();

        public WallCoords PlaceWall() => throw new System.NotImplementedException();
        public GameType ChooseGameType() => throw new System.NotImplementedException();

        public bool ShouldRestart() => throw new System.NotImplementedException();
    }
}