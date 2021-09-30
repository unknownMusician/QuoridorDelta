using System;
using System.Collections.Generic;

namespace Quoridor.Model
{
    public sealed class Field
    {
        public readonly Pawn FirstPawn;
        public readonly Pawn SecondPawn;
        private List<Wall> _walls { get; }
       
        public Field(Pawn pawn1,Pawn pawn2)
        {
            FirstPawn = pawn1;
            SecondPawn = pawn2;
            _walls = new List<Wall>();
        }

        public void AddWallOnField(Wall wall)
        {
            _walls.Add(wall);
        }
    }
}
