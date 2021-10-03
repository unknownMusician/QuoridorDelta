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
        readonly GameData GameData;
        readonly IView View;
        readonly Rules Rules;

        public GameController(GameData gameData, IView view)
        {
            GameData = gameData;
            View = view;
            Rules = new Rules();
        }

        public void Run()
        {
            bool weHaveWinner = false;
            GameType gameType = View.GetGameType();
            

            while (!weHaveWinner)
            {

                PlayerType currentPlayer = PlayerType.First;
                // Maybe View StartGame
                switch (gameType)
                {
                    case GameType.PlayerVersusBot:                                      
                        break;
                    case GameType.PlayerVersusPlayer:
                        


                        break;
                    default:
                        break;
                }
            }     
            
        }


        private void PlayUntillWeHaveWinner(PlayerType currentPlayer)
        {
            bool doWeHaveWinner = false;
            while (!doWeHaveWinner)
            {
                Play(currentPlayer);
                doWeHaveWinner = Rules.didPlayerWin(currentPlayer);
                if (doWeHaveWinner)
                {
                    View.ShowWinner(currentPlayer)
                            }
                else
                {
                    currentPlayer = ChangePlayers(currentPlayer);
                }

            }
        }

        private void Play(PlayerType currentPlayer)
        {
            MoveType moveType = View.GetMoveType(currentPlayer);
            Move(currentPlayer, moveType);
   
        }


        private bool Move(PlayerType currentPlayer, MoveType moveType)
        {
                bool didPlayerMadeMove = false;     
                Field currentField = GameData.GetField();
                switch (moveType)
                {
                    case MoveType.MovePawn:
                        didPlayerMadeMove = DidPlayerMovePawn(currentPlayer, currentField, moveType);
                        break;
                    case MoveType.PlaceWall:
                        didPlayerMadeMove = DidPlayerPlaceWall(currentPlayer, currentField, moveType);
                        break;
                    default:
                        break;
                }
            return didPlayerMadeMove;
            
        }






        private bool DidPlayerMovePawn(PlayerType currentPlayer,Field currentField,MoveType moveType)
        {
            bool didMovePawn = false;
            Pawn pawnOfCurrentPlayer = GameData.GetPlayerPawn(currentPlayer);
            Coords[] possbileMoves = Rules.GetPossibleMoves(pawnOfCurrentPlayer, currentField);
            while (!didMovePawn)
            {
                didMovePawn = TryToMovePawn(currentPlayer, possbileMoves, currentField, pawnOfCurrentPlayer, moveType);  
            }
            return didMovePawn;
        }


        private bool TryToMovePawn(PlayerType currentPlayer,Coords[] possibleMoves,Field currentField,Pawn pawnOfCurrentPlayer, MoveType moveType)
        {
            bool didMovePawn = false;
            Coords newPawnCoords = View.GetMovePawnCoords(currentPlayer, possibleMoves);
            bool isMoveRight = Rules.CanMovePawn(pawnOfCurrentPlayer, currentField, newPawnCoords);
            if (isMoveRight)
            {
                MovePawn(currentPlayer, newPawnCoords);
                didMovePawn = true;
            }
            else
            {
                View.ShowWrongMove(currentPlayer, moveType);
            }
            return didMovePawn;
        }



        private void MovePawn(PlayerType currentPlayer,Coords newPawnCoords)
        {
            GameData.MovePlayerPawn(currentPlayer, newPawnCoords);
            View.MovePlayerPawn(currentPlayer, newPawnCoords);
        }
        
        private bool DidPlayerPlaceWall(PlayerType currentPlayer, Field field, MoveType moveType)
        {
            bool didPlayerPlaceWall = false;
            WallCoords wallPlacementCoords = View.GetPlaceWallCoords(currentPlayer);
            Player playerObject = GameData.GetPlayerByType(currentPlayer);
            while (!didPlayerPlaceWall)
            {
                didPlayerPlaceWall= TryToPlaceWall(currentPlayer, playerObject, field, moveType);
            }
            return didPlayerPlaceWall;

            
        }
        private bool TryToPlaceWall(PlayerType currentPlayer, Player playerObject, Field field, MoveType moveType)
        {
            bool didPlayerPlaceWall = false;
            WallCoords wallPlacementCoords = View.GetPlaceWallCoords(currentPlayer);
            bool isMoveRight = Rules.CanPlaceWall(playerObject, field, wallPlacementCoords);
            if (isMoveRight)
            {
                PlaceWall(currentPlayer, wallPlacementCoords);
                didPlayerPlaceWall = true;
            }
            else
            {
                View.ShowWrongMove(currentPlayer, moveType);
            }
            return didPlayerPlaceWall;
        }
        private void PlaceWall(PlayerType currentPlayer,WallCoords wallPlacementCoords)
        {
            GameData.PlacePlayerWall(currentPlayer, wallPlacementCoords);
            View.PlacePlayerWall(currentPlayer, wallPlacementCoords); 
            
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
