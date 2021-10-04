using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QuoridorDelta.Model
{
    public sealed class Rules : IRules
    {
        private const int MinCoords = 0;
        private const int MinWallCoords = MinCoords;
        private const int MaxCoords = 8;
        private const int MaxWallCoords = 7;

        private static Pawn GetOtherPawn(Pawn pawn, Field field)
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

        private static int GetDistance(Coords c1, Coords c2) => Math.Abs(c1.X - c2.X) + Math.Abs(c1.Y - c2.Y);

        private bool IsThereEnemyNearby(Pawn pawn, Field field) =>
            GetDistance(pawn.Coords, GetOtherPawn(pawn, field).Coords) == 1;

        private static bool IsWithinFieldRange(Coords coords)
        {
            (int x, int y) = coords;

            return (x >= MinCoords) && (x <= MaxCoords) && (y >= MinCoords) && (y <= MaxCoords);
        }

        private static bool IsWithinFieldRange(WallCoords coords)
        {
            ((int x, int y), _) = coords;

            return (x >= MinWallCoords) && (x <= MaxWallCoords) && (y >= MinWallCoords) && (y <= MaxWallCoords);
        }

        private static bool CanJump2StepsOverCloseEnemy(Coords pawnCoords, Coords enemyCoords, Coords newCoords)
        {
            int coordsIndex;

            if (pawnCoords[0] == enemyCoords[0])
            {
                coordsIndex = 1;
            }
            else if (pawnCoords[1] == enemyCoords[1])
            {
                coordsIndex = 0;
            }
            else
            {
                return false;
            }

            return Math.Sign(enemyCoords[coordsIndex] - pawnCoords[coordsIndex]) ==
                   Math.Sign(newCoords[coordsIndex] - pawnCoords[coordsIndex]);
        }

        private static bool IsThereWallBetweenNeighbors(Coords c1, Coords c2, Field field)
        {
            Coords minCoord;
            (int x, int y) = c2 - c1;
            WallCoords possibleWall1;
            WallOrientation wallOrientation;

            if (x == 0)
            {
                minCoord = y > 0 ? c1 : c2;

                wallOrientation = WallOrientation.Horizontal;
                possibleWall1 = new WallCoords(minCoord - (1, 0), wallOrientation);
            }
            else if (y == 0)
            {
                minCoord = x > 0 ? c1 : c2;

                wallOrientation = WallOrientation.Vertical;
                possibleWall1 = new WallCoords(minCoord - (0, 1), wallOrientation);
            }
            else
            {
                throw new InvalidOperationException();
            }

            var possibleWall2 = new WallCoords(minCoord, wallOrientation);

            return field.Walls.Contains(possibleWall1) || field.Walls.Contains(possibleWall2);
        }

        private static bool HasBadNeighbors(WallCoords wallCoords, ICollection<WallCoords> walls)
        {
            (Coords coords, WallOrientation orientation) = wallCoords;

            Coords deltaWallCoords = orientation switch
            {
                WallOrientation.Horizontal => (1, 0),
                WallOrientation.Vertical => (0, 1),
                _ => throw new ArgumentOutOfRangeException(),
            };

            return walls.Contains((coords + deltaWallCoords, orientation))
                || walls.Contains((coords - deltaWallCoords, orientation));
        }

        public bool CanMovePawn(Pawn pawn, Field field, Coords newCoords)
        {
            int moveDistance = GetDistance(pawn.Coords, newCoords);
            Coords otherPawnCoords = GetOtherPawn(pawn, field).Coords;

            if ((newCoords == pawn.Coords)
             || (!IsWithinFieldRange(newCoords))
             || (moveDistance > 2)
             || (otherPawnCoords == newCoords)
             || (!IsThereEnemyNearby(pawn, field) && moveDistance > 1)
             || (moveDistance > 1 && !CanJump2StepsOverCloseEnemy(pawn.Coords, otherPawnCoords, newCoords)))
            {
                return false;
            }

            if (moveDistance == 2)
            {
                return !IsThereWallBetweenNeighbors(pawn.Coords, otherPawnCoords, field)
                    && !IsThereWallBetweenNeighbors(otherPawnCoords, newCoords, field);
            }
            else if (moveDistance == 1)
            {
                return !IsThereWallBetweenNeighbors(pawn.Coords, newCoords, field);
            }
            else
            {
                throw new InvalidProgramException();
            }
        }

        private bool CanPlaceWallWallCountUnchecked(ICollection<WallCoords> walls, WallCoords newWallCoords) =>
            (walls.All(wall => wall.Coords != newWallCoords.Coords))
         && (IsWithinFieldRange(newWallCoords))
         && (!HasBadNeighbors(newWallCoords, walls));

        public bool CanPlaceWall(Player player, ICollection<WallCoords> walls, WallCoords newWallCoords) =>
            (player.WallCount > 0)
         && CanPlaceWallWallCountUnchecked(walls, newWallCoords);

        public Coords[] GetPossibleMoves(Pawn pawn, Field field)
        {
            Coords pawnCoords = pawn.Coords;

            var possibleMoves = new List<Coords>();

            for (int x = pawnCoords.X - 2; x <= pawnCoords.X + 2; x++)
            {
                for (int y = pawnCoords.Y - 2; y <= pawnCoords.Y + 2; y++)
                {
                    Coords newCoords = (x, y);

                    if (GetDistance(pawnCoords, newCoords) <= 2 && CanMovePawn(pawn, field, newCoords))
                    {
                        possibleMoves.Add(newCoords);
                    }
                }
            }

            return possibleMoves.ToArray();
        }

        public bool DidPlayerWin(PlayerType playerType, Player player)
        {
            int coordsY = player.Pawn.Coords.Y;

            return playerType switch
            {
                PlayerType.First => coordsY == MaxCoords,
                PlayerType.Second => coordsY == MinCoords,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public WallCoords[] GetPossibleWallPlacements(IEnumerable<WallCoords> placedWallCoords)
            => placedWallCoords.ToArray();

        private bool DoWeHavePathToWin(Pawn pawn, Field field, PlayerType playerType)
        {
            Coords currentCoords = pawn.Coords;
            Rules rules = new Rules();
            int winY = 0;
            if(playerType == PlayerType.First)
            {
                winY = 8;
            }
            SortedSet<Coords> closed = new SortedSet<Coords>();
            bool haveAPath = false;
            List<Coords> next = new List<Coords>();
            int j = 0;

            while (!haveAPath) {    
                List <Coords> neighbours = GetNeighbours(currentCoords);
                List<Coords> neighboursToCheck = new List<Coords>();

                for(int i = 0; i < neighbours.Count; i++)
                {
                    if (!closed.Contains(neighbours[i]))
                    {
                        neighboursToCheck.Add(neighbours[i]);
                    }
                }


                for (int i = 0; i < neighboursToCheck.Count; i++)
                {
                    if (rules.CanMovePawn(new Pawn(currentCoords), field, neighbours[i]))
                    {
                        if(neighbours[i].Y == winY)
                        {
                            haveAPath = true;
                        }
                        next.Add(neighbours[i]);
                    }                  
                }
                closed.Add(currentCoords);
                currentCoords = next[j];
                j++;
                if(next.Count == j)
                {
                    break;
                }
            }
            return haveAPath;
        }

        private List<Coords> GetNeighbours(Coords coords)
        {
            List<Coords> neighbours = new List<Coords>();
            if(coords.X - 1 >= 0)
            {
                neighbours.Add(new Coords(coords.X-1,coords.Y));
            }
            if(coords.X + 1 <= 8)
            {
                neighbours.Add(new Coords(coords.X + 1, coords.Y));
            }
            if(coords.Y - 1 >= 0)
            {
                neighbours.Add(new Coords(coords.X, coords.Y - 1));
            }
            if(coords.Y + 1 <= 8)
            {
                neighbours.Add(new Coords(coords.X, coords.Y + 1));
            }
            return neighbours;


        }
    }

            for (int y = 0; y < MaxWallCoords; y++)
            {
                for (int x = 0; x < MaxWallCoords; x++)
                {
                    for (var o = WallOrientation.Horizontal;
                         o != WallOrientation.Vertical;
                         o = WallOrientation.Vertical)
                    {
                        WallCoords wallCoords = ((x, y), o);

                        if (CanPlaceWallWallCountUnchecked(placedWallCoords, wallCoords))
                        {
                            possibleCoords.Add(wallCoords);
                        }
                    }
                }
            }

            return possibleCoords.ToArray();
        }
    }
}