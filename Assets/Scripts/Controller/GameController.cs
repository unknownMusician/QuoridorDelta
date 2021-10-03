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
                _ => throw new System.ArgumentOutOfRangeException(),
            };
        }

        private IView GetView(PlayerType playerType) => playerType switch
        {
            PlayerType.First => _view1,
            PlayerType.Second => _view2,
            _ => throw new System.ArgumentOutOfRangeException(),
        };

        public void Run()
        {
            InitializeViews(_view1.GetGameType());
            bool doWePlay;
            do
            {
                PlayUntillWeHaveWinner();

                doWePlay = _view1.ShouldRestart();
                if (doWePlay)
                {
                    _gameData.ClearAndRegenerateData();
                }
            }
            while (doWePlay);
        }

        private void PlayUntillWeHaveWinner()
        {
            PlayerType currentPlayer = PlayerType.First;

            bool doWeHaveWinner;
            do
            {
                MakeMove(currentPlayer);
                doWeHaveWinner = _rules.DidPlayerWin(currentPlayer,_gameData.GetPlayerByType(currentPlayer));
                if (doWeHaveWinner)
                {
                    _view1.ShowWinner(currentPlayer);
                }
                else
                {
                    currentPlayer = ChangePlayers(currentPlayer);
                }
            }
            while (!doWeHaveWinner);
        }

        private void MakeMove(PlayerType currentPlayer)
        {
            MoveType moveType = GetView(currentPlayer).GetMoveType(currentPlayer);
            Move(currentPlayer, moveType);
        }

        private void Move(PlayerType currentPlayer, MoveType moveType)
        {
            Field currentField = _gameData.GetField();
            switch (moveType)
            {
                case MoveType.MovePawn:
                    PlayerMovePawn(currentPlayer, currentField);
                    break;
                case MoveType.PlaceWall:
                    PlayerPlaceWall(currentPlayer, currentField);
                    break;
                default:
                    break;
            }
        }

        private void PlayerMovePawn(PlayerType currentPlayer, Field currentField)
        {
            bool didMovePawn = false;
            Pawn pawnOfCurrentPlayer = _gameData.GetPlayerPawn(currentPlayer);
            Coords[] possbileMoves = _rules.GetPossibleMoves(pawnOfCurrentPlayer, currentField);
            while (!didMovePawn)
            {
                didMovePawn = TryToMovePawn(currentPlayer, possbileMoves, currentField, pawnOfCurrentPlayer);
            }
        }


        private bool TryToMovePawn(PlayerType currentPlayer, Coords[] possibleMoves, Field currentField, Pawn pawnOfCurrentPlayer)
        {
            bool didMovePawn = false;
            Coords newPawnCoords = GetView(currentPlayer).GetMovePawnCoords(currentPlayer, possibleMoves);
            bool isMoveRight = _rules.CanMovePawn(pawnOfCurrentPlayer, currentField, newPawnCoords);
            if (isMoveRight)
            {
                MovePawn(currentPlayer, newPawnCoords);
                didMovePawn = true;
            }
            else
            {
                _view1.ShowWrongMove(currentPlayer, MoveType.MovePawn);
            }
            return didMovePawn;
        }



        private void MovePawn(PlayerType currentPlayer, Coords newPawnCoords)
        {
            _gameData.MovePlayerPawn(currentPlayer, newPawnCoords);
            _view1.MovePlayerPawn(currentPlayer, newPawnCoords);
        }

        private void PlayerPlaceWall(PlayerType currentPlayer, Field field)
        {
            bool didPlayerPlaceWall = false;
            Player playerObject = _gameData.GetPlayerByType(currentPlayer);
            while (!didPlayerPlaceWall)
            {
                didPlayerPlaceWall = TryToPlaceWall(currentPlayer, playerObject, field);
            }
        }

        private bool TryToPlaceWall(PlayerType currentPlayer, Player playerObject, Field field)
        {
            bool didPlayerPlaceWall = false;
            WallCoords wallPlacementCoords = GetView(currentPlayer).GetPlaceWallCoords(currentPlayer);
            bool isMoveRight = _rules.CanPlaceWall(playerObject, field, wallPlacementCoords);
            if (isMoveRight)
            {
                PlaceWall(currentPlayer, wallPlacementCoords);
                didPlayerPlaceWall = true;
            }
            else
            {
                _view1.ShowWrongMove(currentPlayer, MoveType.PlaceWall);
            }
            return didPlayerPlaceWall;
        }
        private void PlaceWall(PlayerType currentPlayer, WallCoords wallPlacementCoords)
        {
            _gameData.PlacePlayerWall(currentPlayer, wallPlacementCoords);
            _view1.PlacePlayerWall(currentPlayer, wallPlacementCoords);
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
