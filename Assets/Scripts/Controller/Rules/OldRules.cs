using System;
using System.Collections.Generic;
using System.Linq;
using Dev;
using QuoridorDelta.DataBaseManagementSystem;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public sealed class OldRules : Rules
    {
        public const int MinCoords = 0;
        public const int MinWallCoords = MinCoords;
        public const int MaxCoords = 8;
        public const int MaxWallCoords = 7;

        public static int GetDistance(in Coords c1, in Coords c2) => Math.Abs(c1.X - c2.X) + Math.Abs(c1.Y - c2.Y);

        public bool IsThereEnemyNearby(PlayerNumber playerNumber)
            => IsThereEnemyNearby(playerNumber, LastGameState.PlayerInfoContainer);

        public static bool IsThereEnemyNearby(
            PlayerNumber playerNumber, in PlayerInfoContainer<PlayerInfo> playerInfos
        )
            => GetDistance(playerInfos[playerNumber].PawnCoords,
                                    playerInfos[playerNumber.Changed()].PawnCoords)
            == 1;

        public static bool IsWithinFieldRange(in Coords coords)
        {
            (int x, int y) = coords;

            const int minCoords = MinCoords;
            const int maxCoords = MaxCoords;

            return (x >= minCoords) && (x <= maxCoords) && (y >= minCoords) && (y <= maxCoords);
        }

        public static bool IsWithinFieldRange(in WallCoords coords)
        {
            ((int x, int y), _) = coords;

            bool result = (x >= MinWallCoords)
                       && (x <= MaxWallCoords)
                       && (y >= MinWallCoords)
                       && (y <= MaxWallCoords);

            return result;
        }

        public static bool CanJump2StepsOverCloseEnemy(
            in Coords pawnCoords, in Coords enemyCoords, in Coords newCoords, IEnumerable<WallCoords> walls
        )
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

            if (Math.Sign(enemyCoords[coordsIndex] - pawnCoords[coordsIndex])
             != Math.Sign(newCoords[coordsIndex] - pawnCoords[coordsIndex]))
            {
                return false;
            }

            int otherCordsIndex = 1 - coordsIndex;

            return pawnCoords[otherCordsIndex] == newCoords[otherCordsIndex]
                || IsThereWallBetweenNeighbors(enemyCoords, (enemyCoords - pawnCoords) * 2 + pawnCoords, walls)
                || !IsWithinFieldRange(enemyCoords * 2 - pawnCoords);
        }

        public bool IsThereWallBetweenNeighbors(in Coords c1, in Coords c2)
            => IsThereWallBetweenNeighbors(c1, c2, LastGameState.Walls);

        public static bool IsThereWallBetweenNeighbors(in Coords c1, in Coords c2, IEnumerable<WallCoords> walls)
        {
            Coords minCoord;
            (int x, int y) = c2 - c1;
            WallCoords possibleWall1;
            WallRotation wallRotation;

            if (x == 0)
            {
                minCoord = y > 0 ? c1 : c2;

                wallRotation = WallRotation.Horizontal;
                possibleWall1 = (minCoord - (1, 0), wallRotation);
            }
            else if (y == 0)
            {
                minCoord = x > 0 ? c1 : c2;

                wallRotation = WallRotation.Vertical;
                possibleWall1 = (minCoord - (0, 1), wallRotation);
            }
            else
            {
                throw new InvalidOperationException();
            }

            var possibleWall2 = new WallCoords(minCoord, wallRotation);

            return walls.Contains(possibleWall1) || walls.Contains(possibleWall2);
        }

        public bool HasBadNeighbors(in WallCoords wallCoords)
            => HasBadNeighbors(wallCoords, LastGameState.Walls);

        public static bool HasBadNeighbors(in WallCoords wallCoords, IEnumerable<WallCoords> walls)
        {
            (Coords coords, WallRotation orientation) = wallCoords;

            Coords deltaWallCoords = orientation switch
            {
                WallRotation.Horizontal => (1, 0),
                WallRotation.Vertical => (0, 1),
                _ => throw new ArgumentOutOfRangeException(),
            };

            bool result = walls.Contains((coords + deltaWallCoords, orientation))
                       || walls.Contains((coords - deltaWallCoords, orientation));

            return result;
        }

        public override bool CanMovePawn(PlayerNumber playerNumber, in Coords newCoords)
            => CanMovePawn(playerNumber, newCoords, LastGameState);

        public static bool CanMovePawn(PlayerNumber playerNumber, in Coords newCoords, GameState gameState)
        {
            Coords pawnCoords = gameState.PlayerInfoContainer[playerNumber].PawnCoords;

            int moveDistance = GetDistance(pawnCoords, newCoords);
            Coords otherPawnCoords = gameState.PlayerInfoContainer[playerNumber.Changed()].PawnCoords;

            if ((newCoords == pawnCoords)
             || (!IsWithinFieldRange(newCoords))
             || (moveDistance > 2)
             || (otherPawnCoords == newCoords)
             || (moveDistance > 1 && !IsThereEnemyNearby(playerNumber, gameState.PlayerInfoContainer))
             || (moveDistance > 1
              && !CanJump2StepsOverCloseEnemy(pawnCoords, otherPawnCoords, newCoords, gameState.Walls)))
            {
                return false;
            }

            return moveDistance switch
            {
                2 => !IsThereWallBetweenNeighbors(pawnCoords, otherPawnCoords, gameState.Walls)
                  && !IsThereWallBetweenNeighbors(otherPawnCoords, newCoords, gameState.Walls),
                1 => !IsThereWallBetweenNeighbors(pawnCoords, newCoords, gameState.Walls),
                _ => throw new InvalidProgramException()
            };
        }

        public bool CanPlaceWallWallCountUnchecked(in WallCoords newCoords)
            => CanPlaceWallWallCountUnchecked(newCoords, LastGameState);

        public static bool CanPlaceWallWallCountUnchecked(WallCoords newCoords, GameState gameState)
            => (gameState.Walls.All(wall => wall.Coords != newCoords.Coords))
            && (IsWithinFieldRange(newCoords))
            && (!HasBadNeighbors(newCoords, gameState.Walls))
            && (PathExists(newCoords, gameState));

        public override bool CanPlaceWall(PlayerNumber playerNumber, in WallCoords newCoords)
            => CanPlaceWall(playerNumber, newCoords, LastGameState);

        [Optimized(0.3, 0.6)]
        public static bool CanPlaceWall(PlayerNumber playerNumber, in WallCoords newCoords, GameState gameState)
            => (gameState.PlayerInfoContainer[playerNumber].WallCount > 0)
            && CanPlaceWallWallCountUnchecked(newCoords, gameState);

        public override IEnumerable<Coords> GetPossiblePawnMoves(PlayerNumber playerNumber)
            => GetPossiblePawnMoves(playerNumber, LastGameState);

        public static IEnumerable<Coords> GetPossiblePawnMoves(PlayerNumber playerNumber, GameState gameState)
        {
            Coords pawnCoords = gameState.PlayerInfoContainer[playerNumber].PawnCoords;

            var possibleMoves = new List<Coords>();

            for (int x = pawnCoords.X - 2; x <= pawnCoords.X + 2; x++)
            {
                for (int y = pawnCoords.Y - 2; y <= pawnCoords.Y + 2; y++)
                {
                    Coords newCoords = (x, y);

                    if (GetDistance(pawnCoords, newCoords) <= 2
                     && CanMovePawn(playerNumber, newCoords, gameState))
                    {
                        possibleMoves.Add(newCoords);
                    }
                }
            }

            return possibleMoves;
        }

        public override bool IsWinner(PlayerNumber playerNumber)
            => IsWinner(playerNumber, LastGameState.PlayerInfoContainer);

        public static bool IsWinner(PlayerNumber playerNumber, in PlayerInfoContainer<PlayerInfo> playerInfos)
        {
            int coordsY = playerInfos[playerNumber].PawnCoords.Y;

            return playerNumber switch
            {
                PlayerNumber.White => coordsY == MaxCoords,
                PlayerNumber.Black => coordsY == MinCoords,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public static int GetWinCoordsY(PlayerNumber playerNumber)
            => playerNumber switch
            {
                PlayerNumber.White => MaxCoords,
                PlayerNumber.Black => MinCoords,
                _ => throw new ArgumentOutOfRangeException()
            };

        public bool PathExists(in WallCoords newCoords) => PathExists(newCoords, LastGameState);

        public static bool PathExists(in WallCoords newCoords, GameState gameState)
        {
            var wallList = new List<WallCoords>(gameState.Walls) { newCoords };

            bool result =
                PathExists(GetWinCoordsY(PlayerNumber.White),
                                    wallList,
                                    gameState.PlayerInfoContainer[PlayerNumber.White].PawnCoords,
                                    new HashSet<Coords>())
             && PathExists(GetWinCoordsY(PlayerNumber.Black),
                                    wallList,
                                    gameState.PlayerInfoContainer[PlayerNumber.Black].PawnCoords,
                                    new HashSet<Coords>());

            return result;
        }

        private static bool ShortPathAStar(in Coords start, in Coords goal)
        {
            // var g = new Dictionary<Coords, int>();
            // var f = new Dictionary<Coords, int>();
            //
            // int h(Coords coords) => GetDistance(coords, goal);
            //
            // var U = new List<Coords>();
            // var Q = new List<Coords>();
            // Q.Add(start);
            // g[start] = 0;
            // f[start] = g[start] + h(start);

            // while (Q.Count != 0)
            // {
            //     var current = Q[0]; //вершина из Q с минимальным значением f
            //
            //     if (current == goal)
            //     {
            //         return true; // нашли путь до нужной вершины
            //     }
            //
            //     Q.RemoveAt(0);
            //     U.Add(current);
            //     foreach (var v in GetNeighbors(current, coords => IsThereWallBetweenNeighbors()))
            //     for v :
            //     смежные с current вершины
            //         tentativeScore = g[current] + d(current, v) // d(current, v) — стоимость пути между current и v 
            //
            //     if v∈U and tentativeScore >= g[v]
            //
            //     continue
            //
            //     if v∉U or tentativeScore < g[v]
            //     parent[v] = current
            //     g[v] = tentativeScore
            //     f[v] = g[v] + h(v)
            //     if v∉Q
            //     Q.push(v)
            // }

            return false;
        }

        public static bool PathExists(
            int winCoordsY, IEnumerable<WallCoords> walls, in Coords coords, ISet<Coords> visitedCoords
        )
        {
            if (coords.Y == winCoordsY)
            {
                return true;
            }

            visitedCoords.Add(coords);

            foreach (Coords neighbor in GetNeighbors(coords))
            {
                if (!visitedCoords.Contains(neighbor)
                 && !IsThereWallBetweenNeighbors(coords, neighbor, walls)
                 && PathExists(winCoordsY, walls, neighbor, visitedCoords))
                {
                    return true;
                }
            }

            return false;
        }

        public static List<Coords> GetNeighbors(in Coords coords)
        {
            var neighbours = new List<Coords>();

            AddIfWithinFieldRange((coords.X - 1, coords.Y), neighbours);
            AddIfWithinFieldRange((coords.X + 1, coords.Y), neighbours);
            AddIfWithinFieldRange((coords.X, coords.Y - 1), neighbours);
            AddIfWithinFieldRange((coords.X, coords.Y + 1), neighbours);

            return neighbours;
        }

        public static Coords[] GetNeighborsWallsIncluded(Coords coords, IEnumerable<WallCoords> walls)
            => GetNeighbors(coords)
                       .Where(neighbor => IsThereWallBetweenNeighbors(neighbor, coords, walls))
                       .ToArray();

        public static void AddIfWithinFieldRange(in Coords coords, ICollection<Coords> list)
        {
            if (IsWithinFieldRange(coords))
            {
                list.Add(coords);
            }
        }

        public static List<Coords> GetNeighbors(in Coords coords, Predicate<Coords> predicate)
        {
            var neighbours = new List<Coords>();

            AddIf((coords.X - 1, coords.Y), neighbours, predicate);
            AddIf((coords.X + 1, coords.Y), neighbours, predicate);
            AddIf((coords.X, coords.Y - 1), neighbours, predicate);
            AddIf((coords.X, coords.Y + 1), neighbours, predicate);

            return neighbours;
        }

        public static void AddIf(in Coords coords, ICollection<Coords> list, Predicate<Coords> predicate)
        {
            if (predicate(coords))
            {
                list.Add(coords);
            }
        }

        public override IEnumerable<WallCoords> GetPossibleWallPlacements(PlayerNumber playerNumber)
            => GetPossibleWallPlacements(LastGameState);

        [Optimized(30, 50)]
        public static IEnumerable<WallCoords> GetPossibleWallPlacements(GameState gameState)
        {
            var possibleCoords = new List<WallCoords>();

            for (int y = 0; y <= MaxWallCoords; y++)
            {
                for (int x = 0; x <= MaxWallCoords; x++)
                {
                    for (int r = 0; r < 2; r++)
                    {
                        var wallRotation = (WallRotation)r;
                        WallCoords wallCoords = ((x, y), wallRotation);

                        if (CanPlaceWallWallCountUnchecked(wallCoords, gameState))
                        {
                            possibleCoords.Add(wallCoords);
                        }
                    }
                }
            }

            return possibleCoords;
        }
    }
}
