using System;
using System.Collections.Generic;
using System.Linq;

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
                throw new ArgumentOutOfRangeException();
            }
        }
        private int GetDistance(Coords c1, Coords c2) => Math.Abs(c1.X - c2.X) + Math.Abs(c1.Y - c2.Y);
        private bool IsThereEnemyNearby(Pawn pawn, Field field) => GetDistance(pawn.Coords, GetOtherPawn(pawn, field).Coords) == 1;

        private bool IsWithinFieldRange(Coords coords)
        {
            float x = coords.X;
            float y = coords.Y;

            const int Min = 0;
            const int Max = 8;

            return (x >= Min) && (x <= Max) && (y >= Min) && (y <= Max);
        }

        private bool IsWithinFieldRange(WallCoords coords)
        {
            float x = coords.Coords.X;
            float y = coords.Coords.Y;

            const int Min = 0;
            const int Max = 7;

            return (x >= Min) && (x <= Max) && (y >= Min) && (y <= Max);
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

        private bool IsThereWallBetweenNeighbors(Coords c1, Coords c2, Field field)
        {
            Coords minCoord;
            Coords difference = c2 - c1;
            if (difference.X == 0)
            {
                minCoord = difference.Y > 0 ? c1 : c2;

                const WallOrientation wallOrientation = WallOrientation.Horizontal;
                WallCoords possibleWall1 = new WallCoords(minCoord - (1, 0), wallOrientation);
                WallCoords possibleWall2 = new WallCoords(minCoord, wallOrientation);

                return field.Walls.Contains(possibleWall1) || field.Walls.Contains(possibleWall2);
            }
            else // if (difference.Y == 0)
            {
                minCoord = difference.X > 0 ? c1 : c2;

                const WallOrientation wallOrientation = WallOrientation.Vertical;
                WallCoords possibleWall1 = new WallCoords(minCoord - (0, 1), wallOrientation);
                WallCoords possibleWall2 = new WallCoords(minCoord, wallOrientation);

                return field.Walls.Contains(possibleWall1) || field.Walls.Contains(possibleWall2);
            }
        }

        private bool HasBadNeighbors(WallCoords wallCoords, Field field) => wallCoords.Orientation switch
        {
            WallOrientation.Horizontal => field.Walls.Contains((wallCoords.Coords + (1, 0), wallCoords.Orientation))
            || field.Walls.Contains((wallCoords.Coords - (1, 0), wallCoords.Orientation)),

            WallOrientation.Vertical => field.Walls.Contains((wallCoords.Coords + (0, 1), wallCoords.Orientation))
            || field.Walls.Contains((wallCoords.Coords - (0, 1), wallCoords.Orientation)),

            _ => throw new ArgumentOutOfRangeException(),
        };

        public bool CanMovePawn(Pawn pawn, Field field, Coords newCoords)
        {
            int moveDistance = GetDistance(pawn.Coords, newCoords);

            if ((newCoords == pawn.Coords)
                || (!IsWithinFieldRange(newCoords))
                || (moveDistance > 2)
                || (GetOtherPawn(pawn, field).Coords == newCoords)
                || (!IsThereEnemyNearby(pawn, field) && moveDistance > 1)
                || (moveDistance > 1 && !CanJump2StepsOverCloseEnemy(pawn.Coords, GetOtherPawn(pawn, field).Coords, newCoords)))
            {
                return false;
            }

            if (moveDistance == 2)
            {
                return !IsThereWallBetweenNeighbors(pawn.Coords, GetOtherPawn(pawn, field).Coords, field)
                    && !IsThereWallBetweenNeighbors(GetOtherPawn(pawn, field).Coords, newCoords, field);
            }
            else // if(moveDistance == 1)
            {
                return !IsThereWallBetweenNeighbors(pawn.Coords, newCoords, field);
            }
        }

        public bool CanPlaceWall(Player player, Field field, WallCoords newWallCoords)
        {
            return (player.WallCount > 0)
                && (!field.Walls.Any(wall => wall.Coords == newWallCoords.Coords))
                && (IsWithinFieldRange(newWallCoords))
                && (!HasBadNeighbors(newWallCoords, field));
        }

        public Coords[] GetPossibleMoves(Pawn pawn, Field field)
        {
            Coords pawnCoords = pawn.Coords;

            var possibleMoves = new List<Coords>();

            for (int x = pawnCoords.X - 2; x <= pawnCoords.X + 2; x++)
            {
                for (int y = pawnCoords.Y - 2; y <= pawnCoords.Y + 2; y++)
                {
                    Coords newCoords = (x, y);
                    if (CanMovePawn(pawn, field, newCoords))
                    {
                        possibleMoves.Add(newCoords);
                    }
                }
            }

            return possibleMoves.ToArray();
        }

        public bool DidPlayerWin(PlayerType playerType, Player player)
        {
            const int MaxFieldYValue = 8;
            const int MinFieldYValue = 0;

            int coordsY = player.Pawn.Coords.Y;

            return playerType switch
            {
                PlayerType.First => coordsY == MaxFieldYValue,
                PlayerType.Second => coordsY == MinFieldYValue,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}