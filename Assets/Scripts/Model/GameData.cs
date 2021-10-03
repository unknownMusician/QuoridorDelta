
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
            _data.ChangePawnCoords(playerType, newCoords);
        }

        public void AddWallOnField(PlayerType playerType, WallCoords wallCoords)
        {
            _data.AddWallOnField(playerType, wallCoords);
        }

        public Field GetField()
        {
            return _data.Field;
        }

    }

}
