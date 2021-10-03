using QuoridorDelta.Model;
using QuoridorDelta.View;

namespace QuoridorDelta.Controller
{
    /** 
     *Basic Logic of Controller
     *
     *
     * 
       
       Start Loop1
      Play duo or with bot

    start Loop2 -> what is a moveType, what coords, tryToMove -> (try again,move), moved
    Next player -> what is a moveType, what coords, tryToMove -> (try again,move), moved
    While flag won == true
    Winner is -> end Loop2

    Do you want to play again 
     If yes Loop1 doesn`t end, If no Loop1 ends */


    class GameController
    {
        private readonly GameData _gameData;
        private readonly IView _view;
        private readonly IRules _rules;

        public GameController(GameData gameData, IView view)
        {
            _view = view;
            _rules = new Rules();
            _gameData = gameData;
        }

        private void InitializeViews(GameType gameType)
        {
            IView view2 = gameType switch
            {
                GameType.PlayerVersusBot => new Bot(),
                GameType.PlayerVersusPlayer => _view,
                _ => throw new System.ArgumentOutOfRangeException(),
            };
        }

        public void Run()
        {
            bool doWePlay = true;
            bool weHaveWinner = false;
            GameType gameType = _view.GetGameType();



            while (doWePlay)
            {
                while (!weHaveWinner)
                {

                    switch (gameType)
                    {
                        case GameType.PlayerVersusBot:
                            break;
                        case GameType.PlayerVersusPlayer:
                            PlayUntillWeHaveWinner();
                            weHaveWinner = true;
                            break;
                        default:
                            break;
                    }
                }
                doWePlay = _view.ShouldRestart();
                if (doWePlay)
                {
                    _gameData.ClearAndRegenerateData();
                }
            }
        }


        private void PlayUntillWeHaveWinner()
        {
            PlayerType currentPlayer = PlayerType.First;

            bool doWeHaveWinner = false;
            while (!doWeHaveWinner)
            {
                MakeMove(currentPlayer);
                doWeHaveWinner = _rules.DidPlayerWin(currentPlayer);
                if (doWeHaveWinner)
                {
                    _view.ShowWinner(currentPlayer);
                }
                else
                {
                    currentPlayer = ChangePlayers(currentPlayer);
                }
            }
        }

        private void MakeMove(PlayerType currentPlayer)
        {
            MoveType moveType = _view.GetMoveType(currentPlayer);
            Move(currentPlayer, moveType);
        }

        private void Move(PlayerType currentPlayer, MoveType moveType)
        {
            Field currentField = _gameData.GetField();
            switch (moveType)
            {
                case MoveType.MovePawn:
                    PlayerMovePawn(currentPlayer, currentField, moveType);
                    break;
                case MoveType.PlaceWall:
                    PlayerPlaceWall(currentPlayer, currentField, moveType);
                    break;
                default:
                    break;
            }
        }

        private void PlayerMovePawn(PlayerType currentPlayer, Field currentField, MoveType moveType)
        {
            bool didMovePawn = false;
            Pawn pawnOfCurrentPlayer = _gameData.GetPlayerPawn(currentPlayer);
            Coords[] possbileMoves = _rules.GetPossibleMoves(pawnOfCurrentPlayer, currentField);
            while (!didMovePawn)
            {
                didMovePawn = TryToMovePawn(currentPlayer, possbileMoves, currentField, pawnOfCurrentPlayer, moveType);
            }
        }


        private bool TryToMovePawn(PlayerType currentPlayer, Coords[] possibleMoves, Field currentField, Pawn pawnOfCurrentPlayer, MoveType moveType)
        {
            bool didMovePawn = false;
            Coords newPawnCoords = _view.GetMovePawnCoords(currentPlayer, possibleMoves);
            bool isMoveRight = _rules.CanMovePawn(pawnOfCurrentPlayer, currentField, newPawnCoords);
            if (isMoveRight)
            {
                MovePawn(currentPlayer, newPawnCoords);
                didMovePawn = true;
            }
            else
            {
                _view.ShowWrongMove(currentPlayer, moveType);
            }
            return didMovePawn;
        }



        private void MovePawn(PlayerType currentPlayer, Coords newPawnCoords)
        {
            _gameData.MovePlayerPawn(currentPlayer, newPawnCoords);
            _view.MovePlayerPawn(currentPlayer, newPawnCoords);
        }

        private void PlayerPlaceWall(PlayerType currentPlayer, Field field, MoveType moveType)
        {
            bool didPlayerPlaceWall = false;
            Player playerObject = _gameData.GetPlayerByType(currentPlayer);
            while (!didPlayerPlaceWall)
            {
                didPlayerPlaceWall = TryToPlaceWall(currentPlayer, playerObject, field, moveType);
            }
        }

        private bool TryToPlaceWall(PlayerType currentPlayer, Player playerObject, Field field, MoveType moveType)
        {
            bool didPlayerPlaceWall = false;
            WallCoords wallPlacementCoords = _view.GetPlaceWallCoords(currentPlayer);
            bool isMoveRight = _rules.CanPlaceWall(playerObject, field, wallPlacementCoords);
            if (isMoveRight)
            {
                PlaceWall(currentPlayer, wallPlacementCoords);
                didPlayerPlaceWall = true;
            }
            else
            {
                _view.ShowWrongMove(currentPlayer, moveType);
            }
            return didPlayerPlaceWall;
        }
        private void PlaceWall(PlayerType currentPlayer, WallCoords wallPlacementCoords)
        {
            _gameData.PlacePlayerWall(currentPlayer, wallPlacementCoords);
            _view.PlacePlayerWall(currentPlayer, wallPlacementCoords);

        }
        private PlayerType ChangePlayers(PlayerType playerType)
        {
            PlayerType playerTypeToReturn = PlayerType.First;
            if (playerType == PlayerType.First)
            {
                playerTypeToReturn = PlayerType.Second;
            }

            return playerTypeToReturn;
        }

    }


}
