using System;

namespace QuoridorDelta.Model
{
    public sealed class Game
    {
        public readonly Field Field;
        public readonly Player Player1;
        public readonly Player Player2;

        public Game()
        {
            const int playerWallCount = 10;

            Pawn firstPawn = new Pawn((4, 0));
            Pawn secondPawn = new Pawn((4, 8));

            Player1 = new Player(firstPawn, playerWallCount);
            Player2 = new Player(secondPawn, playerWallCount);

            Field = new Field(firstPawn, secondPawn);
        }
    }
}
