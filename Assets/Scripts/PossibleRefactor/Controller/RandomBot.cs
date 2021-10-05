using System;
using PossibleRefactor.Model;

namespace PossibleRefactor.Controller
{
    public sealed class RandomBot : Bot
    {
        public override MoveType ChooseMoveType() => throw new NotImplementedException();

        public override Coords MovePawn() => throw new NotImplementedException();

        public override WallCoords PlaceWall() => throw new NotImplementedException();
    }
}