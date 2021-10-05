using System;
using JetBrains.Annotations;
using PossibleRefactor.DataBaseManagementSystem;
using PossibleRefactor.Model;

namespace PossibleRefactor.Controller
{
    public sealed class Game
    {
        private readonly DisposableStartHandler _startHandler = new DisposableStartHandler();
        
        private IDataBase _dataBase;
        private Rules _rules;
        private Players _players;

        public void Start([NotNull] IGameInput input, [NotNull] IGameView view)
        {
            _startHandler.HandleStart();
            
            _rules = new AllowingRules();

            do
            {
                Action<GameState, IDBChangeInfo> onChange = _rules.HandleChange;
                _players = PlayersFactory.CreatePlayers(input.ChooseGameType(), input, view, ref onChange);
                _dataBase = new DBMS(InitialRules.PlayerInfos, onChange);

                Loop(out PlayerNumber winner);
                view.ShowWinner(winner);
            } while (input.ShouldRestart());
        }

        private void Loop(out PlayerNumber winner)
        {
            for (winner = PlayerNumber.First; !_rules.IsWinner(winner); winner.Change())
            {
                MoveType moveType = _dataBase.PlayerInfos[winner].WallCount > 0
                    ? _players[winner].ChooseMoveType()
                    : MoveType.PlaceWall;

                MakeMove(moveType, winner);
            }
        }

        private void MakeMove(MoveType moveType, PlayerNumber playerNumber)
        {
            switch (moveType)
            {
                case MoveType.MovePawn:
                    MovePawn(playerNumber);

                    break;
                case MoveType.PlaceWall:
                    PlaceWall(playerNumber);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void MovePawn(PlayerNumber playerNumber)
        {
            Coords chosenCoords;

            do
            {
                chosenCoords = _players[playerNumber].MovePawn();
            } while (!_rules.CanMovePawn(playerNumber, chosenCoords));

            _dataBase.MovePawn(playerNumber, chosenCoords);
        }

        private void PlaceWall(PlayerNumber playerNumber)
        {
            WallCoords chosenCoords;

            do
            {
                chosenCoords = _players[playerNumber].PlaceWall();
            } while (!_rules.CanPlaceWall(playerNumber, chosenCoords));

            _dataBase.PlaceWall(playerNumber, chosenCoords);
        }
    }
}