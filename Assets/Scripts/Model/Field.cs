using System.Collections.Generic;

namespace QuoridorDelta.Model
{
    public sealed class Field
    {
        public readonly Pawn Pawn1;
        public readonly Pawn Pawn2;
        public readonly List<WallCoords> Walls;

        public Field(Pawn pawn1, Pawn pawn2)
        {
            Pawn1 = pawn1;
            Pawn2 = pawn2;
            Walls = new List<WallCoords>();
        }
    }
}