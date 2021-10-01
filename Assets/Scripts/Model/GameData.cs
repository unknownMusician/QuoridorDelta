
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


        public void ChangePawnCoords(PlayerType playerType,Coords newCoords)
        {
            _data.ChangePawnCoords(GetPlayer(playerType).Pawn, newCoords);
        }

        public void AddWallOnField(PlayerType playerType, WallCoords wallCoords)
        {
            _data.AddWallOnField(GetPlayer(playerType), wallCoords);
        }

        public Field GetField()
        {
            return _data.Field;
        }

        public Player GetPlayer(PlayerType playerType)
        {
            Player playerToReturn;
            switch (playerType)
            {
                case PlayerType.First:
                    playerToReturn = _data.Player1;
                    break;
                case PlayerType.Second:
                    playerToReturn = _data.Player2;
                    break;
                default:
                    playerToReturn =  null; // Maybe throw exception
                    break;

            }
            return playerToReturn;

        }

      



    }

}
