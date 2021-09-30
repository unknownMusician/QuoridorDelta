using System;

namespace Quoridor.Model
{
    public interface IRules
    {
        bool CanPlaceWall(Player player,Field field, Coords newWallCoords );
        bool CanMovePawn(Pawn pawn, Field field, Coords newCoords);
        Coords[] GetPossibleMoves(Pawn pawn, Field field);
    }

    public class Rules : IRules
    {
        private Pawn GetOtherPawn(Pawn pawn, Field field)
        {
            if (pawn == field.FirstPawn)
            {
                return field.SecondPawn;
            }
            else if (pawn == field.SecondPawn)
            {
                return field.FirstPawn;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        private bool IsThereEnemyNearby(Pawn pawn, Field field)
        {
            Pawn otherPawn = GetOtherPawn(pawn, field);

            return Math.Abs(pawn.PawnCoords.X - otherPawn.PawnCoords.X) + Math.Abs(pawn.PawnCoords.Y - otherPawn.PawnCoords.Y) == 1;
        }
        public bool CanMovePawn(Pawn pawn, Field field, Coords newCoords)
        {
            
            throw new System.NotImplementedException();
        }

        public bool CanPlaceWall(Player player, Field field, Coords newWallCoords) => throw new System.NotImplementedException();
        public Coords[] GetPossibleMoves(Pawn pawn, Field field) => throw new System.NotImplementedException();
    }
}
