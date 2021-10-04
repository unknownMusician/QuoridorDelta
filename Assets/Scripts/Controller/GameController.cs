using System;
using QuoridorDelta.Model;
using QuoridorDelta.View;

namespace QuoridorDelta.Controller
{
    public class GameController
    {
        private readonly GameData _gameData;
        private readonly IView _view;
        private IInput _input1;
        private IInput _input2;
        private readonly IRules _rules;

        public GameController(GameData gameData, IView view)
        {
            _view = view;
            _rules = new Rules();
            _gameData = gameData;
        }

        private void InitializeInputs(GameType gameType)
        {
            _input1 = _view;
            _input2 = gameType switch
            {
                GameType.PlayerVersusBot => new Bot(),
                GameType.PlayerVersusPlayer => _view,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private IInput GetInput(PlayerType playerType) => playerType switch
        {
            PlayerType.First => _input1,
            PlayerType.Second => _input2,
            _ => throw new ArgumentOutOfRangeException(),
        };

        public void Run()
        {
            InitializeInputs(_view.GetGameType());
            bool doWePlay;

            do
            {
                PlayUntilWeHaveWinner();

                doWePlay = _view.ShouldRestart();

                if (!doWePlay)
                {
                    continue;
                }

                _gameData.ClearAndRegenerateData();
                _view.MovePlayerPawn(PlayerType.First, _gameData.GetPlayerByType(PlayerType.First).Pawn.Coords);
                _view.MovePlayerPawn(PlayerType.Second, _gameData.GetPlayerByType(PlayerType.Second).Pawn.Coords);
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
                    _view.ShowWinner(currentPlayer);
                }
                else
                {
                    SwapPlayers(ref currentPlayer);
                }
            } while (!doWeHaveWinner);
        }

        private void MakeMove(PlayerType currentPlayer)
        {

            Player player = _gameData.GetPlayerByType(currentPlayer);
            MoveType moveType = MoveType.MovePawn;
            if (player.WallCount > 0)
            {
                moveType = GetInput(currentPlayer).GetMoveType(currentPlayer);
            }
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
            Coords newPawnCoords = GetInput(currentPlayer).GetMovePawnCoords(currentPlayer, possibleMoves);
            bool isMoveRight = _rules.CanMovePawn(pawnOfCurrentPlayer, currentField, newPawnCoords);

            if (isMoveRight)
            {
                MovePawn(currentPlayer, newPawnCoords);
            }
            else
            {
                _view.ShowWrongMove(currentPlayer, MoveType.MovePawn);
            }

            return isMoveRight;
        }

        private void MovePawn(PlayerType currentPlayer, Coords newPawnCoords)
        {
            _gameData.MovePlayerPawn(currentPlayer, newPawnCoords);
            _view.MovePlayerPawn(currentPlayer, newPawnCoords);
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
            WallCoords[] possbileWallPlacements = _rules.GetPossibleWallPlacements(field.Walls);
            WallCoords wallPlacementCoords = GetInput(currentPlayer).GetPlaceWallCoords(currentPlayer,possbileWallPlacements);
            bool isMoveRight = _rules.CanPlaceWall(playerObject, field, wallPlacementCoords);

            if (isMoveRight)
            {
                PlaceWall(currentPlayer, wallPlacementCoords);
            }
            else
            {
                _view.ShowWrongMove(currentPlayer, MoveType.PlaceWall);
            }

            return isMoveRight;
        }

        private void PlaceWall(PlayerType currentPlayer, WallCoords wallPlacementCoords)
        {
            _gameData.PlacePlayerWall(currentPlayer, wallPlacementCoords);
            _view.PlacePlayerWall(currentPlayer, wallPlacementCoords);
        }

        private void SwapPlayers(ref PlayerType playerType) => playerType = playerType switch
        {
            PlayerType.First => PlayerType.Second,
            PlayerType.Second => PlayerType.First,
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}