using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuoridorDelta.Model;

namespace QuoridorDelta.Data
{
    public class TestData
    {

        public Field Field { get; private set; }
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        
        public void GenerateData()
        {
            const int playerWallCount = 10;

            Pawn firstPawn = new Pawn((4, 0));
            Pawn secondPawn = new Pawn((4, 8));

            Player1 = new Player(firstPawn, playerWallCount);
            Player2 = new Player(secondPawn, playerWallCount);

            Field = new Field(firstPawn, secondPawn);
        }

       public void ChangePawnCoords(PlayerType playerType, Coords newCoords)
        {
            ParsePlayerTypeToPlayer(playerType).Pawn.Coords = newCoords;
        }
        
        public void AddWallOnField(PlayerType player,WallCoords wallCoords)
        {
            Field.Walls.Add(wallCoords);
            DecremntPlayerWallCount(ParsePlayerTypeToPlayer(player));
        }

        public Player ParsePlayerTypeToPlayer(PlayerType playerType)
        {
            Player playerToReturn;
            switch (playerType)
            {
                case PlayerType.First:
                    playerToReturn = Player1;
                    break;
                case PlayerType.Second:
                    playerToReturn = Player2;
                    break;
                default:
                    playerToReturn = null; // Maybe throw exception
                    break;

            }
            return playerToReturn;

        }

        public void DecremntPlayerWallCount(Player player)
        {
            if (player.WallCount > 0)
            {
                player.WallCount--;
            }
            else
            {
                throw new System.InvalidOperationException();
            }
        }

    }
}
