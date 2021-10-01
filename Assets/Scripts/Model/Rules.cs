
using System;

namespace QuoridorDelta.Model
{
    public class Rules : IRules
    {
        private Pawn GetOtherPawn(Pawn pawn, Field field)
        {
            if (pawn == field.Pawn1)
            {
                return field.Pawn2;
            }
            else if (pawn == field.Pawn2)
            {
                return field.Pawn1;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        private bool IsThereEnemyNearby(Pawn pawn, Field field)
        {
            Pawn otherPawn = GetOtherPawn(pawn, field);

            return Math.Abs(pawn.Coords.X - otherPawn.Coords.X) + Math.Abs(pawn.Coords.Y - otherPawn.Coords.Y) == 1;
        }
        public bool CanMovePawn(Pawn pawn, Field field, Coords newCoords)
        {

            throw new NotImplementedException();
        }

        public bool CanPlaceWall(Player player, Field field, WallCoords newWallCoords) => throw new NotImplementedException();
        public Coords[] GetPossibleMoves(Pawn pawn, Field field) => throw new NotImplementedException();
    }
}
