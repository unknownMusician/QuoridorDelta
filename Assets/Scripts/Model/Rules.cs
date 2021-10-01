
using System;
using System.Collections.Generic;

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
        private int GetDistance(Coords c1, Coords c2) => Math.Abs(c1.X - c2.X) + Math.Abs(c1.Y - c2.Y);
        private bool IsThereEnemyNearby(Pawn pawn, Field field) => GetDistance(pawn.Coords, GetOtherPawn(pawn, field).Coords) == 1;

        private bool IsWithinFieldRange(Coords coords)
        {
            float x = coords.X;
            float y = coords.Y;

            return (x >= 0) && (x <= 8) && (y >= 0) && (y <= 8);
        }

        private bool CanJump2StepsOverCloseEnemy(Coords pawnCoords, Coords enemyCoords, Coords newCoords)
        {
            if (pawnCoords.X == enemyCoords.X)
            {
                return Math.Sign(enemyCoords.Y - pawnCoords.Y) == Math.Sign(newCoords.Y - pawnCoords.Y);
            }
            else if (pawnCoords.Y == enemyCoords.Y)
            {
                return Math.Sign(enemyCoords.X - pawnCoords.X) == Math.Sign(newCoords.X - pawnCoords.X);
            }
            return false;
        }

        private bool IsThereWallBetween(Coords c1, Coords c2)
        {

        }


        public bool CanMovePawn(Pawn pawn, Field field, Coords newCoords)
        {
            int moveDistance = GetDistance(pawn.Coords, newCoords);

            if (!IsWithinFieldRange(newCoords))
            {
                return false;
            }

            if (moveDistance > 2)
            {
                return false;
            }

            if (GetOtherPawn(pawn, field).Coords == newCoords)
            {
                return false;
            }

            if (!IsThereEnemyNearby(pawn, field) && moveDistance > 1)
            {
                return false;
            }

            if (moveDistance > 1 && !CanJump2StepsOverCloseEnemy(pawn.Coords, GetOtherPawn(pawn, field).Coords, newCoords))
            {
                return false;
            }



        }

        public bool CanPlaceWall(Player player, Field field, WallCoords newWallCoords) => throw new NotImplementedException();
        public Coords[] GetPossibleMoves(Pawn pawn, Field field)
        {
            Coords pawnCoords = pawn.Coords;

            var possibleMoves = new List<Coords>();

            if (IsWithinFieldRange(pawnCoords + (0, 1)))
            {

            }

            throw new NotImplementedException();
        }
    }
}
