
using QuoridorDelta.Data;
namespace QuoridorDelta.Model

{
    public class GameData
    {

        private TestData _data;


        public GameData()
        {
            _data.GenerateData();
        }

        public Player GetPlayerByType(PlayerType playerType)
        {

            Player playerToReturn = null;
            switch (playerType)
            {
                case PlayerType.First:
                    playerToReturn = _data.Player1;
                    break;
                case PlayerType.Second:
                    playerToReturn = _data.Player2;
                    break;
                default:
                    break;
            }
            return playerToReturn;
        }
        
        public Pawn GetPlayerPawn(PlayerType playerType)
        {
            Pawn playerPawnToReturn = null;
            switch (playerType)
            {
                case PlayerType.First:
                    playerPawnToReturn = GetField().Pawn1;
                    break;
                case PlayerType.Second:
                    playerPawnToReturn = GetField().Pawn2;
                    break;
                default:                 
                    break;
            }
            return playerPawnToReturn;
        }

        public void MovePlayerPawn(PlayerType playerType,Coords newCoords)
        {
            _data.ChangePlayerPawnCoords(playerType, newCoords);
        }

        public void PlacePlayerWall(PlayerType playerType, WallCoords wallCoords)
        {
            _data.AddPlayerWallOnField(playerType, wallCoords);
        }

        public Field GetField()
        {
            return _data.Field;
        }
    }
}
