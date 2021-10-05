using System;
using System.Collections.Generic;
using QuoridorDelta.Controller;
using QuoridorDelta.Controller.Abstractions.View;
using QuoridorDelta.DataBaseManagementSystem;
using QuoridorDelta.Model;

namespace QuoridorDelta.View.Proxy
{
    public class QuoridorProxy : GameProxy, IGameInput, IGameView
    {
        private readonly UnityProxy _proxy;

        public QuoridorProxy()
        {
            Start(() => new Game().Start(this, this));
        }

        public GameType ChooseGameType()
            => Wait<GameType>();

        public MoveType ChooseMoveType(PlayerNumber playerNumber)
            => Wait<PlayerNumber, MoveType>(playerNumber);

        public Coords MovePawn(PlayerNumber playerNumber, IEnumerable<Coords> possibleMoves)
            => Wait<(PlayerNumber, IEnumerable<Coords>), Coords>((playerNumber, possibleMoves));

        public WallCoords PlaceWall(PlayerNumber playerNumber, IEnumerable<WallCoords> possibleMoves)
            => Wait<(PlayerNumber, IEnumerable<WallCoords>), WallCoords>((playerNumber, possibleMoves));

        public void ShowWinner(PlayerNumber winner)
            => Send(winner);

        public void ShowWrongMove(MoveType moveType) 
            => Send(moveType);

        public bool ShouldRestart()
            => Wait<bool>();

        public void HandleChange(GameState gameState, IDBChangeInfo changeInfo)
        {
            switch (changeInfo)
            {
                case DBInitializedInfo _:
                    Send((gameState.PlayerInfos, 
                         gameState.Walls));
                    break;
                case DBPawnMovedInfo dbPawnMovedInfo:
                    Send((gameState.PlayerInfos, 
                         gameState.Walls,
                         dbPawnMovedInfo.PlayerNumber,
                         dbPawnMovedInfo.NewCoords));
                    break;
                case DBWallPlacedInfo dbWallPlacedInfo:
                    Send((gameState.PlayerInfos, 
                         gameState.Walls, 
                         dbWallPlacedInfo.PlayerNumber,
                         dbWallPlacedInfo.NewCoords));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}