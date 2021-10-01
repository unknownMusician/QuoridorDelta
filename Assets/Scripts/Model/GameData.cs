
using QuoridorDelta.Data;
namespace QuoridorDelta.Model

{
    public class GameData
    {

        private Data _data;


        public GameData()
        {
            _data.GenerateData();
        }


        public bool ChangePawnCoords(Pawn pawn)
        {
            return false;
        }

        public bool AddWallOnField()
        {
            return false;
        }

        public Field GetField()
        {
            return null;
        }

        public Player GetPlayer()
        {
            return null;
        }



    }

}
