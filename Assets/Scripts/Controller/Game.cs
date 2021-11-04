using System;
using QuoridorDelta.Controller.Abstractions.DataBase;
using QuoridorDelta.Controller.Abstractions.View;
using QuoridorDelta.DataBaseManagementSystem;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public sealed class Game
    {
        private readonly DisposableStartHandler _startHandler = new DisposableStartHandler();

        private IDataBase? _dataBase;
        private Rules? _rules;
        private Players? _players;
        private IGameView? _view;

        public void Start(IGameInput input, IGameView view)
        {
            _startHandler.HandleStart();
            _view = view;
            _rules = new OldRules();

            PlayerNumber humanNumber = input.ChoosePlayerNumber();
            
            do
            {
                Action<GameState, IDBChangeInfo> onChange = _rules.HandleChange;
                _players = PlayersFactory.CreatePlayers(input.ChooseGameType(), humanNumber, input, view, ref onChange!);
                _dataBase = new Dbms(InitialRules.PlayerInfoContainer, onChange);

                Loop(out PlayerNumber winner);
                view.ShowWinner(winner);
            }
            while (input.ShouldRestart());
        }

        private void Loop(out PlayerNumber winner)
        {
            for (winner = PlayerNumber.Second; !_rules!.IsWinner(winner);)
            {
                winner.Change();

                MoveType moveType = _dataBase!.PlayerInfoContainer[winner].WallCount > 0 ?
                    _players![winner].ChooseMoveType(winner) :
                    MoveType.MovePawn;

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
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void MovePawn(PlayerNumber playerNumber)
        {
            Coords chosenCoords;

            while (true)
            {
                chosenCoords = _players![playerNumber].MovePawn(playerNumber, _rules!.GetPossiblePawnMoves(playerNumber));

                if (_rules.CanMovePawn(playerNumber, chosenCoords))
                {
                    break;
                }

                _view!.ShowWrongMove(MoveType.MovePawn);
            }

            _dataBase!.MovePawn(playerNumber, chosenCoords);
        }

        private void PlaceWall(PlayerNumber playerNumber)
        {
            WallCoords chosenCoords;

            while (true)
            {
                chosenCoords = _players![playerNumber]
                    .PlaceWall(playerNumber, _rules!.GetPossibleWallPlacements(playerNumber));

                if (_rules.CanPlaceWall(playerNumber, chosenCoords))
                {
                    break;
                }

                _view!.ShowWrongMove(MoveType.PlaceWall);
            }

            _dataBase!.PlaceWall(playerNumber, chosenCoords);
        }
    }
}
