using QuoridorDelta.Data;

namespace QuoridorDelta.Model

{
    public class GameData
    {
        private TestData _data;
        public Field Field => _data.Field;

        public GameData() => ClearAndRegenerateData();

        public void ClearAndRegenerateData() => _data = new TestData();
        public Player GetPlayerByType(PlayerType playerType) => _data.ParsePlayerTypeToPlayer(playerType);
        public Pawn GetPlayerPawn(PlayerType playerType) => GetPlayerByType(playerType).Pawn;

        public void MovePlayerPawn(PlayerType playerType, Coords newCoords) =>
            _data.ChangePlayerPawnCoords(playerType, newCoords);

        public void PlacePlayerWall(PlayerType playerType, WallCoords wallCoords) =>
            _data.AddPlayerWallOnField(playerType, wallCoords);
    }
}