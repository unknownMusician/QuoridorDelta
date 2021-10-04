using QuoridorDelta.Model;
using System.Collections.Generic;

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

    public sealed class Bot : IInput
    {
        public Bot() { }

        public GameType GetGameType()
        {
            var random = new System.Random();

            return (GameType) random.Next(0, 1);
        }

        public Coords GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves) =>
            throw new System.NotImplementedException();

        public MoveType GetMoveType(PlayerType playerType) => throw new System.NotImplementedException();
        public WallCoords GetPlaceWallCoords(PlayerType playerType) => throw new System.NotImplementedException();
    }
}