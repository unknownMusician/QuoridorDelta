using QuoridorDelta.Model;
using System.Collections.Generic;

namespace QuoridorDelta.View
{
    public interface IView
    {
        GameType GetGameType();

        MoveType GetMoveType(PlayerType playerType);
        Coords GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves);
        WallCoords GetPlaceWallCoords(PlayerType playerType);

        void MovePlayerPawn(PlayerType playerType, Coords newCoords);
        void PlacePlayerWall(PlayerType playerType, WallCoords newCoords);
        void ShowWrongMove(PlayerType playerType, MoveType moveType);

        void ShowWinner(PlayerType playerType);
        bool ShouldRestart();
    }

    public sealed class Bot : IView
    {
        public Bot()
        {

        }
        public GameType GetGameType()
        {
            var random = new System.Random();
            return (GameType)random.Next(0, 1);
        }

        public Coords GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves) => throw new System.NotImplementedException();
        public MoveType GetMoveType(PlayerType playerType) => throw new System.NotImplementedException();
        public WallCoords GetPlaceWallCoords(PlayerType playerType) => throw new System.NotImplementedException();
        public void MovePlayerPawn(PlayerType playerType, Coords newCoords) => throw new System.NotImplementedException();
        public void PlacePlayerWall(PlayerType playerType, WallCoords newCoords) => throw new System.NotImplementedException();
        public bool ShouldRestart() => throw new System.NotImplementedException();
        public void ShowWinner(PlayerType playerType) => throw new System.NotImplementedException();
        public void ShowWrongMove(PlayerType playerType, MoveType moveType) => throw new System.NotImplementedException();
    }
}