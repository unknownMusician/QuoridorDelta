using System;
using System.Collections.Generic;

namespace Quoridor.Model
{
    public sealed class Field
    {
        public readonly Pawn FirstPawn;
        public readonly Pawn SecondPawn;
        public readonly List<Wall> Walls;
       
        public Field(Pawn pawn1,Pawn pawn2)
        {
            FirstPawn = pawn1;
            SecondPawn = pawn2;
            Walls = new List<Wall>();
        }

        public void AddWallOnField(Wall wall)
        {
            Walls.Add(wall);
        }
    }
}
