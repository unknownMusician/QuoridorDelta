using System;
using QuoridorDelta.Model;

namespace QuoridorDelta.Data
{
    public class TestData
    {
        public Field Field { get; }
        private readonly Player _player1;
        private readonly Player _player2;

        public TestData()
        {
            const int playerWallCount = 10;

            var firstPawn = new Pawn((4, 0));
            var secondPawn = new Pawn((4, 8));

            _player1 = new Player(firstPawn, playerWallCount);
            _player2 = new Player(secondPawn, playerWallCount);



            Field = new Field(firstPawn, secondPawn,new System.Collections.Generic.List<WallCoords>());
        }

        public void ChangePlayerPawnCoords(PlayerType playerType, Coords newCoords)
            => ParsePlayerTypeToPlayer(playerType).Pawn.Coords = newCoords;

        public void AddPlayerWallOnField(PlayerType player, WallCoords wallCoords)
        {
            Field.Walls.Add(wallCoords);
            DecrementPlayerWallCount(ParsePlayerTypeToPlayer(player));
        }

        public Player ParsePlayerTypeToPlayer(PlayerType playerType) => playerType switch
        {
            PlayerType.First => _player1,
            PlayerType.Second => _player2,
            _ => throw new ArgumentOutOfRangeException(),
        };

        private static void DecrementPlayerWallCount(Player player)
        {
            if (player.WallCount > 0)
            {
                player.WallCount--;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}