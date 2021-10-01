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

       public void ChangePawnCoords(Pawn pawn, Coords newCoords)
        {
            pawn.Coords = newCoords;
        }
        
        public void AddWallOnField(Player player,WallCoords wallCoords)
        {
            Field.AddWall(wallCoords);
            DecremntPlayerWallCount(player);
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
