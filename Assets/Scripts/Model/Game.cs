using System;

namespace Quoridor.Model
{
    public sealed class Game
    {
        public readonly Field Field;
        public readonly Player Player1;
        public readonly Player Player2;

        public Game()
        {
            Coords startPostionOfFirstPawn = new Coords(0, 0); // default postions 
            Coords startPostionOfSecondPawn = new Coords(0, 0);

            const int amountOfPlayerWalls = 10;

            Pawn firstPawn = new Pawn(startPostionOfFirstPawn); 
            Pawn secondPawn = new Pawn(startPostionOfSecondPawn);

            Player1 = new Player(firstPawn, amountOfPlayerWalls);
            Player2 = new Player(secondPawn, amountOfPlayerWalls);

            Field = new Field(firstPawn,secondPawn);
        }
    }
}
