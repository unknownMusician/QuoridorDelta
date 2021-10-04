using System;
using QuoridorDelta.Model;
using QuoridorDelta.View;

namespace QuoridorDelta.Controller
{
    public class GameController
    {
        private readonly GameData _gameData;
        private readonly IView _view1;
        private IView _view2;
        private readonly IRules _rules;

        public GameController(GameData gameData, IView view)
        {
            _view1 = view;
            _rules = new Rules();
            _gameData = gameData;
        }

        private void InitializeViews(GameType gameType)
        {
            _view2 = gameType switch
            {
                GameType.PlayerVersusBot => new Bot(),
                GameType.PlayerVersusPlayer => _view1,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private IView GetView(PlayerType playerType) => playerType switch
        {
            PlayerType.First => _view1,
            PlayerType.Second => _view2,
            _ => throw new ArgumentOutOfRangeException(),
        };

        public void Run()
        {
            InitializeViews(_view1.GetGameType());
            bool doWePlay;

            do
            {
                PlayUntilWeHaveWinner();

                doWePlay = _view1.ShouldRestart();

                if (!doWePlay)
                {
                    continue;
                }

                _gameData.ClearAndRegenerateData();
                _view1.MovePlayerPawn(PlayerType.First, _gameData.GetPlayerByType(PlayerType.First).Pawn.Coords);
                _view1.MovePlayerPawn(PlayerType.Second, _gameData.GetPlayerByType(PlayerType.Second).Pawn.Coords);
            } while (doWePlay);
        }

        private void PlayUntilWeHaveWinner()
        {
            var currentPlayer = PlayerType.First;

            bool doWeHaveWinner;

            do
            {
                MakeMove(currentPlayer);
                doWeHaveWinner = _rules.DidPlayerWin(currentPlayer, _gameData.GetPlayerByType(currentPlayer));

                if (doWeHaveWinner)
                {
                    _view1.ShowWinner(currentPlayer);
                }
                else
                {
                    SwapPlayers(ref currentPlayer);
                }
            } while (!doWeHaveWinner);
        }

        private void MakeMove(PlayerType currentPlayer)
        {
            MoveType moveType = GetView(currentPlayer).GetMoveType(currentPlayer);
            Move(currentPlayer, moveType);
        }

        private void Move(PlayerType currentPlayer, MoveType moveType)
        {
            Field currentField = _gameData.Field;

            switch (moveType)
            {
                case MoveType.MovePawn:
                    PlayerMovePawn(currentPlayer, currentField);

                    break;
                case MoveType.PlaceWall:
                    PlayerPlaceWall(currentPlayer, currentField);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlayerMovePawn(PlayerType currentPlayer, Field currentField)
        {
            Pawn pawnOfCurrentPlayer = _gameData.GetPlayerPawn(currentPlayer);
            Coords[] possibleMoves = _rules.GetPossibleMoves(pawnOfCurrentPlayer, currentField);
            bool didMovePawn;

            do
            {
                didMovePawn = TryToMovePawn(currentPlayer, possibleMoves, currentField, pawnOfCurrentPlayer);
            } while (!didMovePawn);
        }


        private bool TryToMovePawn(PlayerType currentPlayer, Coords[] possibleMoves, Field currentField,
                                   Pawn pawnOfCurrentPlayer)
        {
            Coords newPawnCoords = GetView(currentPlayer).GetMovePawnCoords(currentPlayer, possibleMoves);
            bool isMoveRight = _rules.CanMovePawn(pawnOfCurrentPlayer, currentField, newPawnCoords);

            if (isMoveRight)
            {
                MovePawn(currentPlayer, newPawnCoords);
            }
            else
            {
                GetView(currentPlayer).ShowWrongMove(currentPlayer, MoveType.MovePawn);
            }

            return isMoveRight;
        }

        private void MovePawn(PlayerType currentPlayer, Coords newPawnCoords)
        {
            _gameData.MovePlayerPawn(currentPlayer, newPawnCoords);
            _view1.MovePlayerPawn(currentPlayer, newPawnCoords);
        }

        private void PlayerPlaceWall(PlayerType currentPlayer, Field field)
        {
            Player playerObject = _gameData.GetPlayerByType(currentPlayer);
            bool didPlayerPlaceWall;

            do
            {
                didPlayerPlaceWall = TryToPlaceWall(currentPlayer, playerObject, field);
            } while (!didPlayerPlaceWall);
        }

        private bool TryToPlaceWall(PlayerType currentPlayer, Player playerObject, Field field)
        {
            WallCoords wallPlacementCoords = GetView(currentPlayer).GetPlaceWallCoords(currentPlayer);
            bool isMoveRight = _rules.CanPlaceWall(playerObject, field, wallPlacementCoords);

            if (isMoveRight)
            {
                PlaceWall(currentPlayer, wallPlacementCoords);
            }
            else
            {
                GetView(currentPlayer).ShowWrongMove(currentPlayer, MoveType.PlaceWall);
            }

            return isMoveRight;
        }

        private void PlaceWall(PlayerType currentPlayer, WallCoords wallPlacementCoords)
        {
            _gameData.PlacePlayerWall(currentPlayer, wallPlacementCoords);
            _view1.PlacePlayerWall(currentPlayer, wallPlacementCoords);
        }

        private void SwapPlayers(ref PlayerType playerType) => playerType = playerType switch
        {
            PlayerType.First => PlayerType.Second,
            PlayerType.Second => PlayerType.First,
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}