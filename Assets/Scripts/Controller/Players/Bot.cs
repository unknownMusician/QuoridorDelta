#nullable enable

using System.Collections.Generic;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public abstract class Bot : GameStateRememberer, IPlayerInput
    {
        public abstract MoveType ChooseMoveType(PlayerNumber playerNumber);
        public abstract Coords MovePawn(PlayerNumber playerNumber, IEnumerable<Coords> possibleMoves);
        public abstract WallCoords PlaceWall(PlayerNumber playerNumber, IEnumerable<WallCoords> possibleMoves);
    }
}
